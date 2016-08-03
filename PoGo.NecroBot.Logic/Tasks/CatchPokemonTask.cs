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
using System.Threading;
using PoGo.NecroBot.Logic.Logging;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public static class CatchPokemonTask
    {
        private static Random Random => new Random((int)DateTime.Now.Ticks);

        public static async Task Execute(ISession session, CancellationToken cancellationToken, dynamic encounter, MapPokemon pokemon,
            FortData currentFortData = null, ulong encounterId = 0)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // If the encounter is null nothing will work below, so exit now
            if (encounter == null) return;

            float probability = encounter.CaptureProbability?.CaptureProbability_[0];

            // Check for pokeballs before proceeding
            var pokeball = await GetBestBall(session, encounter, probability);
            if (pokeball == ItemId.ItemUnknown) return;

            //Calculate CP and IV
            var pokemonCp = (encounter is EncounterResponse
                               ? encounter.WildPokemon?.PokemonData?.Cp
                               : encounter.PokemonData?.Cp);
            var pokemonIv = PokemonInfo.CalculatePokemonPerfection(encounter is EncounterResponse
                    ? encounter.WildPokemon?.PokemonData
                    : encounter?.PokemonData);

            // Determine whether to use berries or not
            if ((session.LogicSettings.UseBerriesOperator.ToLower().Equals("and") &&
                    pokemonIv >= session.LogicSettings.UseBerriesMinIv &&
                    pokemonCp >= session.LogicSettings.UseBerriesMinCp &&
                    probability < session.LogicSettings.UseBerriesBelowCatchProbability) || 
                (session.LogicSettings.UseBerriesOperator.ToLower().Equals("or") && (
                    pokemonIv >= session.LogicSettings.UseBerriesMinIv ||
                    pokemonCp >= session.LogicSettings.UseBerriesMinCp ||
                    probability < session.LogicSettings.UseBerriesBelowCatchProbability)))
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

            // Calculate distance away
            var distance = LocationUtils.CalculateDistanceInMeters(session.Client.CurrentLatitude,
                session.Client.CurrentLongitude,
                encounter is EncounterResponse || encounter is IncenseEncounterResponse
                    ? pokemon.Latitude
                    : currentFortData.Latitude,
                encounter is EncounterResponse || encounter is IncenseEncounterResponse
                    ? pokemon.Longitude
                    : currentFortData.Longitude);

            CatchPokemonResponse caughtPokemonResponse;
            var attemptCounter = 1;
            do
            {
                if ((session.LogicSettings.MaxPokeballsPerPokemon > 0 && 
                    attemptCounter > session.LogicSettings.MaxPokeballsPerPokemon))
                    break;

                pokeball = await GetBestBall(session, encounter, probability);
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

                //default to excellent throw
                var normalizedRecticleSize = 1.95;
                //default spin
                var spinModifier = 1.0;

                //Humanized throws
                if (session.LogicSettings.EnableHumanizedThrows)
                {
                    //thresholds: https://gist.github.com/anonymous/077d6dea82d58b8febde54ae9729b1bf
                    var spinTxt = "Curve";
                    var hitTxt = "Excellent";
                    if (pokemonCp > session.LogicSettings.ForceExcellentThrowOverCp ||
                        pokemonIv > session.LogicSettings.ForceExcellentThrowOverIv)
                    {
                        normalizedRecticleSize = Random.NextDouble() * (1.95 - 1.7) + 1.7;
                    }
                    else if (pokemonCp >= session.LogicSettings.ForceGreatThrowOverCp ||
                             pokemonIv >= session.LogicSettings.ForceGreatThrowOverIv)
                    {
                        normalizedRecticleSize = Random.NextDouble() * (1.95 - 1.3) + 1.3;
                        hitTxt = "Great";
                    }
                    else
                    {
                        var regularThrow = 100 - (session.LogicSettings.ExcellentThrowChance +
                                                  session.LogicSettings.GreatThrowChance +
                                                  session.LogicSettings.NiceThrowChance);
                        var rnd = Random.Next(1 , 101);

                        if (rnd <= regularThrow)
                        {
                            normalizedRecticleSize = Random.NextDouble() * (1 - 0.1) + 0.1;
                            hitTxt = "Ordinary";
                        }
                        else if (rnd <= regularThrow + session.LogicSettings.NiceThrowChance)
                        {
                            normalizedRecticleSize = Random.NextDouble() * (1.3 - 1) + 1;
                            hitTxt = "Nice";
                        }
                        else if (rnd <=
                                 regularThrow + session.LogicSettings.NiceThrowChance +
                                 session.LogicSettings.GreatThrowChance)
                        {
                            normalizedRecticleSize = Random.NextDouble() * (1.7 - 1.3) + 1.3;
                            hitTxt = "Great";
                        }

                        if (Random.NextDouble() * 100 > session.LogicSettings.CurveThrowChance)
                        {
                            spinModifier = 0.0;
                            spinTxt = "Straight";
                        }
                    }

                    //round to 2 decimals
                    normalizedRecticleSize = Math.Round(normalizedRecticleSize, 2);

                    Logger.Write($"(Threw ball) {hitTxt} hit. {spinTxt}-ball...", LogLevel.Debug);
                }

                caughtPokemonResponse =
                    await session.Client.Encounter.CatchPokemon(
                        encounter is EncounterResponse || encounter is IncenseEncounterResponse
                            ? pokemon.EncounterId
                            : encounterId,
                        encounter is EncounterResponse || encounter is IncenseEncounterResponse
                            ? pokemon.SpawnPointId
                            : currentFortData.Id, pokeball, normalizedRecticleSize, spinModifier);

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

                    if (session.LogicSettings.TransferDuplicatePokemonOnCapture && session.LogicSettings.TransferDuplicatePokemon)
                        await TransferDuplicatePokemonTask.Execute(session, cancellationToken);
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