using POGOProtos.Inventory.Item;
using POGOProtos.Map.Pokemon;
using POGOProtos.Networking.Responses;
using PokemonGo.RocketAPI.Logic.Event;
using PokemonGo.RocketAPI.Logic.State;
using PokemonGo.RocketAPI.Logic.Utils;
using System;
using System.Linq;
using System.Threading;

namespace PokemonGo.RocketAPI.Logic.Tasks
{
    public static class CatchPokemonTask
    {
        private static void UseBerry(Context ctx, ulong encounterId, string spawnPointId)
        {
            var inventoryBalls = ctx.Inventory.GetItems().Result;
            var berries = inventoryBalls.Where(p => (ItemId)p.ItemId == ItemId.ItemRazzBerry);
            var berry = berries.FirstOrDefault();

            if (berry == null || berry.Count <= 0)
                return;

            ctx.Client.Encounter.UseCaptureItem(encounterId, ItemId.ItemRazzBerry, spawnPointId).Wait();
            berry.Count -= 1;
            Logger.Write($"Used, remaining: {berry.Count}", LogLevel.Berry);
            Thread.Sleep(1500);
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

            if (ultraBallsCount > 0 && iV >= ctx.Settings.KeepMinIVPercentage && proba < 0.40)
                return ItemId.ItemUltraBall;

            if (greatBallsCount > 0 && iV >= ctx.Settings.KeepMinIVPercentage && proba < 0.50)
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
                    machine.Fire(new NoPokeballEvent { Id = pokemon.PokemonId, Cp = encounter?.WildPokemon?.PokemonData?.Cp ?? 0 });
                    return;
                }

                bool isLowProbability = probability.HasValue && probability.Value < 0.35;
                bool isHighCp = encounter.WildPokemon?.PokemonData?.Cp > 400;
                bool isHighPerfection = PokemonInfo.CalculatePokemonPerfection(encounter?.WildPokemon?.PokemonData) >= ctx.Settings.KeepMinIVPercentage;

                if ((isLowProbability && isHighCp) || isHighPerfection)
                {
                    UseBerry(ctx, pokemon.EncounterId, pokemon.SpawnPointId);
                }

                var distance = LocationUtils.CalculateDistanceInMeters(ctx.Client.CurrentLat, ctx.Client.CurrentLng, pokemon.Latitude, pokemon.Longitude);

                caughtPokemonResponse = ctx.Client.Encounter.CatchPokemon(pokemon.EncounterId, pokemon.SpawnPointId, pokemon.Latitude, pokemon.Longitude, pokeball).Result;

                PokemonCaptureEvent evt = new PokemonCaptureEvent();
                evt.Status = caughtPokemonResponse.Status;

                if (caughtPokemonResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchSuccess)
                {
                    int totalExp = 0;

                    foreach (var xp in caughtPokemonResponse.Scores.Xp)
                    {
                        totalExp += xp;
                    }
                    var profile = ctx.Client.GetProfile().Result;

                    evt.Exp = totalExp;
                    evt.Stardust = profile.Profile.Currency.ToArray()[1].Amount;
                }


                if (encounter?.CaptureProbability?.CaptureProbability_ != null)
                {
                    evt.Id = pokemon.PokemonId;
                    evt.Level = PokemonInfo.GetLevel(encounter.WildPokemon?.PokemonData);
                    evt.Cp = encounter.WildPokemon?.PokemonData?.Cp ?? 0;
                    evt.MaxCp = PokemonInfo.CalculateMaxCP(encounter.WildPokemon?.PokemonData);
                    evt.Perfection = Math.Round(PokemonInfo.CalculatePokemonPerfection(encounter.WildPokemon?.PokemonData));
                    evt.Probability = Math.Round(Convert.ToDouble(encounter.CaptureProbability?.CaptureProbability_.First()) * 100, 2);
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
    }
}
