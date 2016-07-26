#region using directives

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        public static async Task Execute(Context ctx, StateMachine machine, EncounterResponse encounter, MapPokemon pokemon)
        {
            CatchPokemonResponse caughtPokemonResponse;
            var attemptCounter = 1;
            do
            {
                var probability = encounter?.CaptureProbability?.CaptureProbability_?.FirstOrDefault();

                var pokeball = await GetBestBall(ctx, encounter);
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
                    await UseBerry(ctx, machine, pokemon.EncounterId, pokemon.SpawnPointId);
                }

                var distance = LocationUtils.CalculateDistanceInMeters(ctx.Client.CurrentLatitude,
                    ctx.Client.CurrentLongitude, pokemon.Latitude, pokemon.Longitude);

                caughtPokemonResponse =
                    await ctx.Client.Encounter.CatchPokemon(pokemon.EncounterId, pokemon.SpawnPointId, pokeball);

                var evt = new PokemonCaptureEvent {Status = caughtPokemonResponse.Status};

                if (caughtPokemonResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchSuccess)
                {
                    var totalExp = 0;

                    foreach (var xp in caughtPokemonResponse.CaptureAward.Xp)
                    {
                        totalExp += xp;
                    }
                    var profile = await ctx.Client.Player.GetPlayer();

                    evt.Exp = totalExp;
                    evt.Stardust = profile.PlayerData.Currencies.ToArray()[1].Amount;

                    var pokemonSettings = await ctx.Inventory.GetPokemonSettings();
                    var pokemonFamilies = await ctx.Inventory.GetPokemonFamilies();

                    var setting = pokemonSettings.FirstOrDefault(q => q.PokemonId == pokemon.PokemonId);
                    var family = pokemonFamilies.FirstOrDefault(q => q.FamilyId == setting.FamilyId);

                    if (family != null)
                    {
                        family.Candy += caughtPokemonResponse.CaptureAward.Candy.Sum();

                        evt.FamilyCandies = family.Candy;
                    }
                    else
                    {
                        evt.FamilyCandies = caughtPokemonResponse.CaptureAward.Candy.Sum();
                    }
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
                await Task.Delay(2000);
            } while (caughtPokemonResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchMissed ||
                     caughtPokemonResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchEscape);
        }

        private static async Task<ItemId> GetBestBall(Context ctx, EncounterResponse encounter)
        {
            var pokemonCp = encounter?.WildPokemon?.PokemonData?.Cp;
            var iV = Math.Round(PokemonInfo.CalculatePokemonPerfection(encounter?.WildPokemon?.PokemonData));
            var proba = encounter?.CaptureProbability?.CaptureProbability_.First();

            var pokeBallsCount = await ctx.Inventory.GetItemAmountByType(ItemId.ItemPokeBall);
            var greatBallsCount = await ctx.Inventory.GetItemAmountByType(ItemId.ItemGreatBall);
            var ultraBallsCount = await ctx.Inventory.GetItemAmountByType(ItemId.ItemUltraBall);
            var masterBallsCount = await ctx.Inventory.GetItemAmountByType(ItemId.ItemMasterBall);

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

        private static async Task UseBerry(Context ctx, StateMachine machine, ulong encounterId, string spawnPointId)
        {
            var inventoryBalls = await ctx.Inventory.GetItems();
            var berries = inventoryBalls.Where(p => p.ItemId == ItemId.ItemRazzBerry);
            var berry = berries.FirstOrDefault();

            if (berry == null || berry.Count <= 0)
                return;

            await ctx.Client.Encounter.UseCaptureItem(encounterId, ItemId.ItemRazzBerry, spawnPointId);
            berry.Count -= 1;
            machine.Fire(new UseBerryEvent {Count = berry.Count});

            await Task.Delay(1500);
        }
    }
}