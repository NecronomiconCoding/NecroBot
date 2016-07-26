#region using directives

using System;
using System.Linq;
using System.Threading;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using POGOProtos.Inventory.Item;
using POGOProtos.Map.Pokemon;
using POGOProtos.Networking.Responses;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public static class CatchPokemonTask
    {
        public static void Execute(Context ctx, StateMachine machine, EncounterResponse encounter, MapPokemon pokemon)
        {
            CatchPokemonResponse caughtPokemonResponse;
            var attemptCounter = 1;
            do
            {
                var probability = encounter?.CaptureProbability?.CaptureProbability_?.FirstOrDefault();

                var pokeball = GetBestBall(ctx, encounter);
                if (pokeball == ItemId.ItemUnknown)
                {
                    machine.Fire(new NoPokeballEvent
                    {
                        Id = pokemon.PokemonId,
                        Cp = encounter?.WildPokemon?.PokemonData?.Cp ?? 0
                    });
                    return;
                }

                var isLowProbability = probability.HasValue && probability.Value < 0.35;
                var isHighCp = encounter != null && encounter.WildPokemon?.PokemonData?.Cp > 400;
                var isHighPerfection = PokemonInfo.CalculatePokemonPerfection(encounter?.WildPokemon?.PokemonData) >=
                                       ctx.LogicSettings.KeepMinIvPercentage;

                if ((isLowProbability && isHighCp) || isHighPerfection)
                {
                    UseBerry(ctx, machine, pokemon.EncounterId, pokemon.SpawnPointId);
                }

                var distance = LocationUtils.CalculateDistanceInMeters(ctx.Client.CurrentLatitude,
                    ctx.Client.CurrentLongitude, pokemon.Latitude, pokemon.Longitude);

                caughtPokemonResponse =
                    ctx.Client.Encounter.CatchPokemon(pokemon.EncounterId, pokemon.SpawnPointId, pokeball).Result;

                var evt = new PokemonCaptureEvent {Status = caughtPokemonResponse.Status};

                if (caughtPokemonResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchSuccess)
                {
                    var totalExp = 0;

                    foreach (var xp in caughtPokemonResponse.CaptureAward.Xp)
                    {
                        totalExp += xp;
                    }
                    var profile = ctx.Client.Player.GetPlayer().Result;

                    evt.Exp = totalExp;
                    evt.Stardust = profile.PlayerData.Currencies.ToArray()[1].Amount;

                    var pokemonSettings = ctx.Inventory.GetPokemonSettings().Result;
                    var pokemonFamilies = ctx.Inventory.GetPokemonFamilies().Result;

                    var setting = pokemonSettings.Single(q => q.PokemonId == pokemon.PokemonId);
                    var family = pokemonFamilies.First(q => q.FamilyId == setting.FamilyId);

                    family.Candy += caughtPokemonResponse.CaptureAward.Candy.Sum();

                    evt.FamilyCandies = family.Candy;
                }


                if (encounter?.CaptureProbability?.CaptureProbability_ != null)
                {
                    evt.Id = pokemon.PokemonId;
                    evt.Level = PokemonInfo.GetLevel(encounter.WildPokemon?.PokemonData);
                    evt.Cp = encounter.WildPokemon?.PokemonData?.Cp ?? 0;
                    evt.MaxCp = PokemonInfo.CalculateMaxCp(encounter.WildPokemon?.PokemonData);
                    evt.Perfection =
                        Math.Round(PokemonInfo.CalculatePokemonPerfection(encounter.WildPokemon?.PokemonData));
                    evt.Probability =
                        Math.Round(Convert.ToDouble(encounter.CaptureProbability?.CaptureProbability_.First())*100, 2);
                    evt.Distance = distance;
                    evt.Pokeball = pokeball;
                    evt.Attempt = attemptCounter;

                    machine.Fire(evt);
                }
                attemptCounter++;
                Thread.Sleep(2000);
            } while (caughtPokemonResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchMissed ||
                     caughtPokemonResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchEscape);
        }

        private static ItemId GetBestBall(Context ctx, EncounterResponse encounter)
        {
            var pokemonCp = encounter?.WildPokemon?.PokemonData?.Cp;
            var iV = Math.Round(PokemonInfo.CalculatePokemonPerfection(encounter?.WildPokemon?.PokemonData));
            var proba = encounter?.CaptureProbability?.CaptureProbability_.First();

            var pokeBallsCount = ctx.Inventory.GetItemAmountByType(ItemId.ItemPokeBall).Result;
            var greatBallsCount = ctx.Inventory.GetItemAmountByType(ItemId.ItemGreatBall).Result;
            var ultraBallsCount = ctx.Inventory.GetItemAmountByType(ItemId.ItemUltraBall).Result;
            var masterBallsCount = ctx.Inventory.GetItemAmountByType(ItemId.ItemMasterBall).Result;

            if (masterBallsCount > 0 && pokemonCp >= 1200)
                return ItemId.ItemMasterBall;
            if (ultraBallsCount > 0 && pokemonCp >= 1000)
                return ItemId.ItemUltraBall;
            if (greatBallsCount > 0 && pokemonCp >= 750)
                return ItemId.ItemGreatBall;

            if (ultraBallsCount > 0 && iV >= ctx.LogicSettings.KeepMinIvPercentage && proba < 0.40)
                return ItemId.ItemUltraBall;

            if (greatBallsCount > 0 && iV >= ctx.LogicSettings.KeepMinIvPercentage && proba < 0.50)
                return ItemId.ItemGreatBall;

            if (greatBallsCount > 0 && pokemonCp >= 300)
                return ItemId.ItemGreatBall;

            if (pokeBallsCount > 0)
                return ItemId.ItemPokeBall;
            if (greatBallsCount > 0)
                return ItemId.ItemGreatBall;
            if (ultraBallsCount > 0)
                return ItemId.ItemUltraBall;
            if (masterBallsCount > 0)
                return ItemId.ItemMasterBall;

            return ItemId.ItemUnknown;
        }

        private static void UseBerry(Context ctx, StateMachine machine, ulong encounterId, string spawnPointId)
        {
            var inventoryBalls = ctx.Inventory.GetItems().Result;
            var berries = inventoryBalls.Where(p => p.ItemId == ItemId.ItemRazzBerry);
            var berry = berries.FirstOrDefault();

            if (berry == null || berry.Count <= 0)
                return;

            ctx.Client.Encounter.UseCaptureItem(encounterId, ItemId.ItemRazzBerry, spawnPointId).Wait();
            berry.Count -= 1;
            machine.Fire(new UseBerryEvent {Count = berry.Count});

            Thread.Sleep(1500);
        }
    }
}