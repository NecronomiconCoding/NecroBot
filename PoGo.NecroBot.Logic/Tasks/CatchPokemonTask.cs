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

        // Lure encounter
        public static void Execute(Context ctx, StateMachine machine, DiskEncounterResponse encounter, ulong EncounterId, string SpawnPointId)
        {
            var pokemonData = encounter?.PokemonData;
            var captureProbability = encounter?.CaptureProbability?.CaptureProbability_?.FirstOrDefault();
            var pokemonLatitude = ctx.Client.CurrentLatitude;
            var pokemonLongitude = ctx.Client.CurrentLongitude;

            Execute(ctx, machine, pokemonData, captureProbability, EncounterId, SpawnPointId, pokemonLatitude, pokemonLongitude);
        }

        // Incense encounter
        public static void Execute(Context ctx, StateMachine machine, IncenseEncounterResponse encounter, ulong EncounterId, string SpawnPointId)
        {
            var pokemonData = encounter?.PokemonData;
            var captureProbability = encounter?.CaptureProbability?.CaptureProbability_?.FirstOrDefault();
            var pokemonLatitude = ctx.Client.CurrentLatitude;
            var pokemonLongitude = ctx.Client.CurrentLongitude;

            Execute(ctx, machine, pokemonData, captureProbability, EncounterId, SpawnPointId, pokemonLatitude, pokemonLongitude);
        }

        // Regular encounter
        public static void Execute(Context ctx, StateMachine machine, EncounterResponse encounter, MapPokemon pokemon)
        {
            var pokemonData = encounter?.WildPokemon?.PokemonData;
            var captureProbability = encounter?.CaptureProbability?.CaptureProbability_?.FirstOrDefault();
            var EncounterId = pokemon.EncounterId;
            var SpawnPointId = pokemon.SpawnPointId;
            var pokemonLatitude = pokemon.Latitude;
            var pokemonLongitude = pokemon.Longitude;

            Execute(ctx, machine, pokemonData, captureProbability, EncounterId, SpawnPointId, pokemonLatitude, pokemonLongitude);
        }

        private static void Execute(Context ctx, StateMachine machine, POGOProtos.Data.PokemonData pokemonData, float? captureProbability, 
            ulong encounterId, string spawnPointId, double pokemonLatitude, double pokemonLongitude)
        {
            CatchPokemonResponse caughtPokemonResponse;
            var attemptCounter = 1;
            do {
                var pokemonCp = pokemonData?.Cp;
                var pokemonIV = Math.Round(PokemonInfo.CalculatePokemonPerfection(pokemonData));

                var pokeball = GetBestBall(ctx, pokemonCp, pokemonIV, captureProbability);
                if (pokeball == ItemId.ItemUnknown)
                {
                    machine.Fire(new NoPokeballEvent
                    {
                        Id = pokemonData.PokemonId,
                        Cp = pokemonCp ?? 0
                    });
                    return;
                }

                var isLowProbability = captureProbability.HasValue && captureProbability.Value < 0.35;
                var isHighCp = pokemonCp != null && pokemonCp > 400;
                var isHighPerfection = PokemonInfo.CalculatePokemonPerfection(pokemonData) >=
                                       ctx.LogicSettings.KeepMinIvPercentage;

                if ((isLowProbability && isHighCp) || isHighPerfection)
                {
                    UseBerry(ctx, machine, encounterId, spawnPointId);
                }

                var distance = LocationUtils.CalculateDistanceInMeters(ctx.Client.CurrentLatitude,
                    ctx.Client.CurrentLongitude, pokemonLatitude, pokemonLongitude);

                caughtPokemonResponse =
                    ctx.Client.Encounter.CatchPokemon(encounterId, spawnPointId, pokeball).Result;

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

                    var setting = pokemonSettings.FirstOrDefault(q => q.PokemonId == q.PokemonId);
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


                if (captureProbability != null)
                {
                    evt.Id = pokemonData.PokemonId;
                    evt.Level = PokemonInfo.GetLevel(pokemonData);
                    evt.Cp = pokemonCp ?? 0;
                    evt.MaxCp = PokemonInfo.CalculateMaxCp(pokemonData);
                    evt.Perfection =
                        Math.Round(PokemonInfo.CalculatePokemonPerfection(pokemonData));
                    evt.Probability =
                        Math.Round(Convert.ToDouble(captureProbability)*100, 2);
                    evt.Distance = distance;
                    evt.Pokeball = pokeball;
                    evt.Attempt = attemptCounter;
                    ctx.Inventory.RefreshCachedInventory().Wait();
                    evt.BallAmount = ctx.Inventory.GetItemAmountByType(pokeball).Result;
                    machine.Fire(evt);
                }
                attemptCounter++;
                Thread.Sleep(2000);
            } while (caughtPokemonResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchMissed ||
                     caughtPokemonResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchEscape);
        }

        private static ItemId GetBestBall(Context ctx, int? pokemonCp, double iV, float? proba)
        {
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