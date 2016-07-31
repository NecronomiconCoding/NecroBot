#region using directives

using System;
using System.Linq;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Common;
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
        public static async Task Execute(ISession session, dynamic encounter, MapPokemon pokemon,
            FortData currentFortData = null, ulong encounterId = 0)
        {
            CatchPokemonResponse caughtPokemonResponse;
            var attemptCounter = 1;
            do
            {
                if (session.LogicSettings.MaxPokeballsPerPokemon > 0 &&
                    attemptCounter > session.LogicSettings.MaxPokeballsPerPokemon)
                    break;

                float probability = encounter?.CaptureProbability?.CaptureProbability_[0];

                var pokeball = await GetBestBall(session, encounter, probability);
                if (pokeball == ItemId.ItemUnknown)
                {
                    session.EventDispatcher.Send(new NoPokeballEvent
                    {
                        Id = encounter is EncounterResponse ? pokemon.PokemonId : encounter?.PokemonData.PokemonId,
                        Cp =
                            (encounter is EncounterResponse
                                ? encounter.WildPokemon?.PokemonData?.Cp
                                : encounter?.PokemonData?.Cp) ?? 0
                    });
                    return;
                }

                var isLowProbability = probability < 0.35;
                var isHighCp = encounter != null &&
                               (encounter is EncounterResponse
                                   ? encounter.WildPokemon?.PokemonData?.Cp
                                   : encounter.PokemonData?.Cp) > 400;
                var isHighPerfection =
                    PokemonInfo.CalculatePokemonPerfection(encounter is EncounterResponse
                        ? encounter.WildPokemon?.PokemonData
                        : encounter?.PokemonData) >= session.LogicSettings.KeepMinIvPercentage;

                if ((isLowProbability && isHighCp) || isHighPerfection)
                {
                    await
                        UseBerry(session,
                            encounter is EncounterResponse || encounter is IncenseEncounterResponse
                                ? pokemon.EncounterId
                                : encounterId,
                            encounter is EncounterResponse || encounter is IncenseEncounterResponse
                                ? pokemon.SpawnPointId
                                : currentFortData?.Id);
                }

                var distance = LocationUtils.CalculateDistanceInMeters(session.Client.CurrentLatitude,
                    session.Client.CurrentLongitude,
                    encounter is EncounterResponse || encounter is IncenseEncounterResponse
                        ? pokemon.Latitude
                        : currentFortData.Latitude,
                    encounter is EncounterResponse || encounter is IncenseEncounterResponse
                        ? pokemon.Longitude
                        : currentFortData.Longitude);

                caughtPokemonResponse =
                    await session.Client.Encounter.CatchPokemon(
                        encounter is EncounterResponse || encounter is IncenseEncounterResponse
                            ? pokemon.EncounterId
                            : encounterId,
                        encounter is EncounterResponse || encounter is IncenseEncounterResponse
                            ? pokemon.SpawnPointId
                            : currentFortData.Id, pokeball);

                var lat = encounter is EncounterResponse || encounter is IncenseEncounterResponse
                             ? pokemon.Latitude : currentFortData.Latitude;
                var lng = encounter is EncounterResponse || encounter is IncenseEncounterResponse
                            ? pokemon.Longitude : currentFortData.Longitude;
                var evt = new PokemonCaptureEvent()
                {
                    Status = caughtPokemonResponse.Status,
                    Latitude = lat,
                    Longitude = lng
                };


                if (caughtPokemonResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchSuccess)
                {
                    var totalExp = 0;

                    foreach (var xp in caughtPokemonResponse.CaptureAward.Xp)
                    {
                        totalExp += xp;
                    }
                    var profile = await session.Client.Player.GetPlayer();

                    evt.Exp = totalExp;
                    evt.Stardust = profile.PlayerData.Currencies.ToArray()[1].Amount;

                    var pokemonSettings = await session.Inventory.GetPokemonSettings();
                    var pokemonFamilies = await session.Inventory.GetPokemonFamilies();

                    var setting =
                        pokemonSettings.FirstOrDefault(q => pokemon != null && q.PokemonId == pokemon.PokemonId);
                    var family = pokemonFamilies.FirstOrDefault(q => setting != null && q.FamilyId == setting.FamilyId);

                    if (family != null)
                    {
                        family.Candy_ += caughtPokemonResponse.CaptureAward.Candy.Sum();

                        evt.FamilyCandies = family.Candy_;
                    }
                    else
                    {
                        evt.FamilyCandies = caughtPokemonResponse.CaptureAward.Candy.Sum();
                    }
                }


                evt.CatchType = encounter is EncounterResponse
                    ? session.Translation.GetTranslation(TranslationString.CatchTypeNormal)
                    : encounter is DiskEncounterResponse
                        ? session.Translation.GetTranslation(TranslationString.CatchTypeLure)
                        : session.Translation.GetTranslation(TranslationString.CatchTypeIncense);
                evt.Id = encounter is EncounterResponse ? pokemon.PokemonId : encounter?.PokemonData.PokemonId;
                evt.Level =
                    PokemonInfo.GetLevel(encounter is EncounterResponse
                        ? encounter.WildPokemon?.PokemonData
                        : encounter?.PokemonData);
                evt.Cp = encounter is EncounterResponse
                    ? encounter.WildPokemon?.PokemonData?.Cp
                    : encounter?.PokemonData?.Cp ?? 0;
                evt.MaxCp =
                    PokemonInfo.CalculateMaxCp(encounter is EncounterResponse
                        ? encounter.WildPokemon?.PokemonData
                        : encounter?.PokemonData);
                evt.Perfection =
                    Math.Round(
                        PokemonInfo.CalculatePokemonPerfection(encounter is EncounterResponse
                            ? encounter.WildPokemon?.PokemonData
                            : encounter?.PokemonData));
                evt.Probability =
                    Math.Round(probability * 100, 2);
                evt.Distance = distance;
                evt.Pokeball = pokeball;
                evt.Attempt = attemptCounter;
                await session.Inventory.RefreshCachedInventory();
                evt.BallAmount = await session.Inventory.GetItemAmountByType(pokeball);

                session.EventDispatcher.Send(evt);

                attemptCounter++;

                DelayingUtils.Delay(session.LogicSettings.DelayBetweenPokemonCatch, 2000);
            } while (caughtPokemonResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchMissed ||
                     caughtPokemonResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchEscape);
        }

        private static async Task<ItemId> GetBestBall(ISession session, dynamic encounter, float probability)
        {
            var pokemonCp = encounter is EncounterResponse
                ? encounter.WildPokemon?.PokemonData?.Cp
                : encounter?.PokemonData?.Cp;
            var pokemonId = encounter is EncounterResponse
                ? encounter.WildPokemon?.PokemonData?.PokemonId
                : encounter?.PokemonData?.PokemonId;
            var iV =
                Math.Round(
                    PokemonInfo.CalculatePokemonPerfection(encounter is EncounterResponse
                        ? encounter.WildPokemon?.PokemonData
                        : encounter?.PokemonData), 2);

            var pokeBallsCount = await session.Inventory.GetItemAmountByType(ItemId.ItemPokeBall);
            var greatBallsCount = await session.Inventory.GetItemAmountByType(ItemId.ItemGreatBall);
            var ultraBallsCount = await session.Inventory.GetItemAmountByType(ItemId.ItemUltraBall);
            var masterBallsCount = await session.Inventory.GetItemAmountByType(ItemId.ItemMasterBall);

            if (masterBallsCount > 0 && (
                    (!session.LogicSettings.PokemonToUseMasterball.Any() && (
                        pokemonCp >= session.LogicSettings.UseMasterBallAboveCp ||
                        probability < session.LogicSettings.UseMasterBallBelowCatchProbability)) ||
                    session.LogicSettings.PokemonToUseMasterball.Contains(pokemonId)))
                return ItemId.ItemMasterBall;

            if (ultraBallsCount > 0 && (
                    pokemonCp >= session.LogicSettings.UseUltraBallAboveCp ||
                    iV >= session.LogicSettings.UseUltraBallAboveIv ||
                    probability < session.LogicSettings.UseUltraBallBelowCatchProbability))
                return ItemId.ItemUltraBall;

            if (greatBallsCount > 0 && (
                    pokemonCp >= session.LogicSettings.UseGreatBallAboveCp ||
                    iV >= session.LogicSettings.UseGreatBallAboveIv ||
                    probability < session.LogicSettings.UseGreatBallBelowCatchProbability))
                return ItemId.ItemGreatBall;

            if (pokeBallsCount > 0)
                return ItemId.ItemPokeBall;
            if (greatBallsCount > 0)
                return ItemId.ItemGreatBall;
            if (ultraBallsCount > 0)
                return ItemId.ItemUltraBall;
            if (masterBallsCount > 0 && !session.LogicSettings.PokemonToUseMasterball.Any())
                return ItemId.ItemMasterBall;

            return ItemId.ItemUnknown;
        }

        private static async Task UseBerry(ISession session, ulong encounterId, string spawnPointId)
        {
            var inventoryBalls = await session.Inventory.GetItems();
            var berries = inventoryBalls.Where(p => p.ItemId == ItemId.ItemRazzBerry);
            var berry = berries.FirstOrDefault();

            if (berry == null || berry.Count <= 0)
                return;

            await session.Client.Encounter.UseCaptureItem(encounterId, ItemId.ItemRazzBerry, spawnPointId);
            berry.Count -= 1;
            session.EventDispatcher.Send(new UseBerryEvent {BerryType = ItemId.ItemRazzBerry, Count = berry.Count});
        }
    }
}