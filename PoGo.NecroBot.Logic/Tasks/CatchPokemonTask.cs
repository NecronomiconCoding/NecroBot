#region using directives

using System;
using System.Linq;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using POGOProtos.Inventory.Item;
using POGOProtos.Map.Fort;
using POGOProtos.Map.Pokemon;
using POGOProtos.Networking.Responses;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public static class CatchPokemonTask
    {
        public static async Task Execute(Context ctx, StateMachine machine, dynamic encounter, MapPokemon pokemon,
            FortData currentFortData = null, ulong encounterId = 0)
        {
            CatchPokemonResponse caughtPokemonResponse;
            var attemptCounter = 1;
            do
            {
                float probability = encounter?.CaptureProbability?.CaptureProbability_[0];

                if (!(encounter is DiskEncounterResponse))
                    encounterId = pokemon.EncounterId;

                var spawnPointId = encounter is DiskEncounterResponse
                    ? currentFortData?.Id
                    : pokemon.SpawnPointId;
                var latitude = encounter is DiskEncounterResponse
                    ? currentFortData.Latitude
                    : pokemon.Latitude;
                var longitude = encounter is DiskEncounterResponse
                    ? currentFortData.Longitude
                    : pokemon.Longitude;

                var pokemonData = encounter is EncounterResponse
                    ? encounter?.WildPokemon?.PokemonData
                    : encounter?.PokemonData;

                var pokemonId = encounter is EncounterResponse
                    ? pokemon.PokemonId
                    : encounter?.PokemonData.PokemonId;

                var Cp = pokemonData?.Cp ?? 0;
                var perfection = PokemonInfo.CalculatePokemonPerfection(pokemonData);

                var pokeball = await GetBestBall(ctx, encounter, probability);
                if (pokeball == ItemId.ItemUnknown)
                {
                    machine.Fire(new NoPokeballEvent
                    {
                        Id = pokemonId,
                        Cp = Cp
                    });
                    return;
                }

                var isLowProbability = probability < 0.35;
                var isHighCp = encounter != null && Cp > 400;
                var isHighPerfection = perfection >= ctx.LogicSettings.KeepMinIvPercentage;

                if ((isLowProbability && isHighCp) || isHighPerfection)
                {
                    await UseBerry(ctx, machine, encounterId, spawnPointId);
                }

                var distance = LocationUtils.CalculateDistanceInMeters(
                    ctx.Client.CurrentLatitude, ctx.Client.CurrentLongitude,
                    latitude, longitude
                );

                caughtPokemonResponse =
                    await ctx.Client.Encounter.CatchPokemon(encounterId, spawnPointId, pokeball);

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

                    var setting = pokemonSettings.FirstOrDefault(q => pokemon != null && q.PokemonId == pokemonId);
                    var family = pokemonFamilies.FirstOrDefault(q => setting != null && q.FamilyId == setting.FamilyId);

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


                evt.CatchType = encounter is EncounterResponse
                    ? "Normal"
                    : encounter is DiskEncounterResponse ? "Lure" : "Incense";
                evt.Id = pokemonId;
                evt.Level =
                    PokemonInfo.GetLevel(pokemonData);
                evt.Cp = Cp;
                evt.MaxCp =
                    PokemonInfo.CalculateMaxCp(pokemonData);
                evt.Perfection =
                    Math.Round(perfection);
                evt.Probability =
                    Math.Round(probability*100, 2);
                evt.Distance = distance;
                evt.Pokeball = pokeball;
                evt.Attempt = attemptCounter;
                await ctx.Inventory.RefreshCachedInventory();
                evt.BallAmount = await ctx.Inventory.GetItemAmountByType(pokeball);

                machine.Fire(evt);

                attemptCounter++;
                await Task.Delay(2000);
            } while (caughtPokemonResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchMissed ||
                     caughtPokemonResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchEscape);
        }

        private static async Task<ItemId> GetBestBall(Context ctx, dynamic encounter, float probability)
        {
            var pokemonCp = encounter is EncounterResponse
                ? encounter.WildPokemon?.PokemonData?.Cp
                : encounter?.PokemonData?.Cp;
            var iV =
                Math.Round(
                    PokemonInfo.CalculatePokemonPerfection(encounter is EncounterResponse
                        ? encounter.WildPokemon?.PokemonData
                        : encounter?.PokemonData));

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

            if (ultraBallsCount > 0 && iV >= ctx.LogicSettings.KeepMinIvPercentage && probability < 0.40)
                return ItemId.ItemUltraBall;

            if (greatBallsCount > 0 && iV >= ctx.LogicSettings.KeepMinIvPercentage && probability < 0.50)
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
        }
    }
}
