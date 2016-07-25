using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.GeneratedCode;
using PokemonGo.RocketAPI.Logic.Event;
using PokemonGo.RocketAPI.Logic.State;
using PokemonGo.RocketAPI.Logic.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Logic.Tasks
{
    public static class CatchPokemonTask
    {
        private static void UseBerry(Context ctx, ulong encounterId, string spawnPointId)
        {
            var inventoryBalls = ctx.Inventory.GetItems().Result;
            var berries = inventoryBalls.Where(p => (ItemId)p.Item_ == ItemId.ItemRazzBerry);
            var berry = berries.FirstOrDefault();

            if (berry == null || berry.Count <= 0)
                return;

            ctx.Client.UseCaptureItem(encounterId, ItemId.ItemRazzBerry, spawnPointId).Wait();
            Logger.Write($"Used, remaining: {berry.Count - 1}", LogLevel.Berry);
            Thread.Sleep(1500);
        }

        private static MiscEnums.Item GetBestBall(Context ctx, EncounterResponse encounter)
        {
            var pokemonCp = encounter?.WildPokemon?.PokemonData?.Cp;
            var iV = Math.Round(PokemonInfo.CalculatePokemonPerfection(encounter?.WildPokemon?.PokemonData));
            var proba = encounter?.CaptureProbability?.CaptureProbability_.First();

            var pokeBallsCount = ctx.Inventory.GetItemAmountByType(MiscEnums.Item.ITEM_POKE_BALL).Result;
            var greatBallsCount = ctx.Inventory.GetItemAmountByType(MiscEnums.Item.ITEM_GREAT_BALL).Result;
            var ultraBallsCount = ctx.Inventory.GetItemAmountByType(MiscEnums.Item.ITEM_ULTRA_BALL).Result;
            var masterBallsCount = ctx.Inventory.GetItemAmountByType(MiscEnums.Item.ITEM_MASTER_BALL).Result;

            if (masterBallsCount > 0 && pokemonCp >= 1200)
                return MiscEnums.Item.ITEM_MASTER_BALL;
            if (ultraBallsCount > 0 && pokemonCp >= 1000)
                return MiscEnums.Item.ITEM_ULTRA_BALL;
            if (greatBallsCount > 0 && pokemonCp >= 750)
                return MiscEnums.Item.ITEM_GREAT_BALL;

            if (ultraBallsCount > 0 && iV >= ctx.Settings.KeepMinIVPercentage && proba < 0.40)
                return MiscEnums.Item.ITEM_ULTRA_BALL;

            if (greatBallsCount > 0 && iV >= ctx.Settings.KeepMinIVPercentage && proba < 0.50)
                return MiscEnums.Item.ITEM_GREAT_BALL;

            if (greatBallsCount > 0 && pokemonCp >= 300)
                return MiscEnums.Item.ITEM_GREAT_BALL;

            if (pokeBallsCount > 0)
                return MiscEnums.Item.ITEM_POKE_BALL;
            if (greatBallsCount > 0)
                return MiscEnums.Item.ITEM_GREAT_BALL;
            if (ultraBallsCount > 0)
                return MiscEnums.Item.ITEM_ULTRA_BALL;
            if (masterBallsCount > 0)
                return MiscEnums.Item.ITEM_MASTER_BALL;

            return MiscEnums.Item.ITEM_UNKNOWN;
        }

        public static void Execute(Context ctx, StateMachine machine, EncounterResponse encounter, MapPokemon pokemon)
        {
            CatchPokemonResponse caughtPokemonResponse;
            var attemptCounter = 1;
            do
            {
                var probability = encounter?.CaptureProbability?.CaptureProbability_?.FirstOrDefault();

                var pokeball = GetBestBall(ctx, encounter);
                if (pokeball == MiscEnums.Item.ITEM_UNKNOWN)
                {
                    machine.Fire(new NoPokeballEvent { Id = pokemon.PokemonId, Cp = encounter?.WildPokemon?.PokemonData?.Cp ?? 0 });
                    return;
                }

                bool isLowProbability = probability.HasValue && probability.Value < 0.35;
                bool isHighCp = encounter.WildPokemon?.PokemonData?.Cp > 400;
                bool isHighPerfection = PokemonInfo.CalculatePokemonPerfection(encounter?.WildPokemon?.PokemonData) >= ctx.Settings.KeepMinIVPercentage;

                if ((isLowProbability && isHighCp) || isHighPerfection)
                {
                    UseBerry(ctx, pokemon.EncounterId, pokemon.SpawnpointId);
                }

                var distance = LocationUtils.CalculateDistanceInMeters(ctx.Client.CurrentLat, ctx.Client.CurrentLng, pokemon.Latitude, pokemon.Longitude);

                caughtPokemonResponse = ctx.Client.CatchPokemon(pokemon.EncounterId, pokemon.SpawnpointId, pokemon.Latitude, pokemon.Longitude, pokeball).Result;

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
