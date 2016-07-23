#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.Exceptions;
using PokemonGo.RocketAPI.Extensions;
using PokemonGo.RocketAPI.GeneratedCode;
using PokemonGo.RocketAPI.Helpers;
using PokemonGo.RocketAPI.Logic.Utils;

#endregion

namespace PokemonGo.RocketAPI.Logic
{
    public class Logic
    {
        private readonly Client _client;
        private readonly ISettings _clientSettings;
        private readonly Inventory _inventory;
        private readonly Navigation _navigation;
        private GetPlayerResponse _playerProfile;

        public Logic(ISettings clientSettings)
        {
            _clientSettings = clientSettings;
            _client = new Client(_clientSettings);
            _inventory = new Inventory(_client);
            _navigation = new Navigation(_client);
        }

        public static float CalculatePokemonPerfection(PokemonData poke)
        {
            return (poke.IndividualAttack*2 + poke.IndividualDefense + poke.IndividualStamina)/(4.0f*15.0f)*100.0f;
        }

        private async Task CatchEncounter(EncounterResponse encounter, MapPokemon pokemon)
        {
            CatchPokemonResponse caughtPokemonResponse;
            do
            {
                var probability = encounter?.CaptureProbability?.CaptureProbability_?.FirstOrDefault();
                if ((probability.HasValue && probability.Value < 0.35 && encounter.WildPokemon?.PokemonData?.Cp > 400) ||
                    CalculatePokemonPerfection(encounter?.WildPokemon?.PokemonData) >=
                    _clientSettings.KeepMinIVPercentage)
                {
                    //Throw berry is we can
                    await UseBerry(pokemon.EncounterId, pokemon.SpawnpointId);
                }

                var pokeball = await GetBestBall(encounter?.WildPokemon);
                var distance = Navigation.DistanceBetween2Coordinates(_client.CurrentLat, _client.CurrentLng,
                    pokemon.Latitude, pokemon.Longitude);
                caughtPokemonResponse =
                    await
                        _client.CatchPokemon(pokemon.EncounterId, pokemon.SpawnpointId, pokemon.Latitude,
                            pokemon.Longitude, pokeball);
                Logger.Write(
                    caughtPokemonResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchSuccess
                        ? $"{pokemon.PokemonId} ({encounter?.WildPokemon?.PokemonData?.Cp} CP) ({Math.Round(CalculatePokemonPerfection(encounter?.WildPokemon?.PokemonData)).ToString("0.00")}% perfection) | Chance: {encounter?.CaptureProbability.CaptureProbability_.First()} | {Math.Round(distance)}m distance | with {pokeball} "
                        : $"{pokemon.PokemonId} ({encounter?.WildPokemon?.PokemonData?.Cp} CP) Chance: {Math.Round(Convert.ToDouble(encounter?.CaptureProbability?.CaptureProbability_.First()))} | {Math.Round(distance)}m distance {caughtPokemonResponse.Status} | with {pokeball}",
                    LogLevel.Caught);
                await DisplayPlayerLevelInTitle(true);
                await Task.Delay(2000);
            } while (caughtPokemonResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchMissed ||
                     caughtPokemonResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchEscape);
        }

        private async Task DisplayPlayerLevelInTitle(bool updateOnly = false)
        {
            _playerProfile = _playerProfile.Profile != null ? _playerProfile : await _client.GetProfile();
            var playerName = _playerProfile.Profile.Username ?? "";
            var playerStats = await _inventory.GetPlayerStats();
            var playerStat = playerStats.FirstOrDefault();
            if (playerStat != null)
            {
                var xpDifference = GetXPDiff(playerStat.Level);
                var message =
                    $"{playerName} | Level {playerStat.Level}: {playerStat.Experience - playerStat.PrevLevelXp - xpDifference}/{playerStat.NextLevelXp - playerStat.PrevLevelXp - xpDifference}XP";
                Console.Title = message;
                if (updateOnly == false)
                    Logger.Write(message);
            }
            if (updateOnly == false)
                await Task.Delay(5000);
        }

        public static int GetXPDiff(int level)
        {
            switch (level)
            {
                case 1:
                    return 0;
                case 2:
                    return 1000;
                case 3:
                    return 2000;
                case 4:
                    return 3000;
                case 5:
                    return 4000;
                case 6:
                    return 5000;
                case 7:
                    return 6000;
                case 8:
                    return 7000;
                case 9:
                    return 8000;
                case 10:
                    return 9000;
                case 11:
                    return 10000;
                case 12:
                    return 10000;
                case 13:
                    return 10000;
                case 14:
                    return 10000;
                case 15:
                    return 15000;
                case 16:
                    return 20000;
                case 17:
                    return 20000;
                case 18:
                    return 20000;
                case 19:
                    return 25000;
                case 20:
                    return 25000;
                case 21:
                    return 50000;
                case 22:
                    return 75000;
                case 23:
                    return 100000;
                case 24:
                    return 125000;
                case 25:
                    return 150000;
                case 26:
                    return 190000;
                case 27:
                    return 200000;
                case 28:
                    return 250000;
                case 29:
                    return 300000;
                case 30:
                    return 350000;
                case 31:
                    return 500000;
                case 32:
                    return 500000;
                case 33:
                    return 750000;
                case 34:
                    return 1000000;
                case 35:
                    return 1250000;
                case 36:
                    return 1500000;
                case 37:
                    return 2000000;
                case 38:
                    return 2500000;
                case 39:
                    return 1000000;
                case 40:
                    return 1000000;
            }
            return 0;
        }

        private async Task EvolveAllPokemonWithEnoughCandy(IEnumerable<PokemonId> filter = null)
        {
            var pokemonToEvolve = await _inventory.GetPokemonToEvolve(filter);
            foreach (var pokemon in pokemonToEvolve)
            {
                var evolvePokemonOutProto = await _client.EvolvePokemon(pokemon.Id);

                if (evolvePokemonOutProto.Result == EvolvePokemonOut.Types.EvolvePokemonStatus.PokemonEvolvedSuccess)
                {
                    Logger.Write($"{pokemon.PokemonId} successfully for {evolvePokemonOutProto.ExpAwarded}xp",
                        LogLevel.Evolve);
                    await DisplayPlayerLevelInTitle(true);
                }
                else
                    Logger.Write(
                        $"Failed {pokemon.PokemonId}. EvolvePokemonOutProto.Result was {evolvePokemonOutProto.Result}, stopping evolving {pokemon.PokemonId}",
                        LogLevel.Evolve);

                await Task.Delay(3000);
            }
        }

        public async Task Execute()
        {
            Git.CheckVersion();
            Logger.Write(
                $"Make sure Lat & Lng is right. Exit Program if not! Lat: {_client.CurrentLat} Lng: {_client.CurrentLng}",
                LogLevel.Warning);
            Thread.Sleep(3000);
            Logger.Write($"Logging in via: {_clientSettings.AuthType}");

            while (true)
            {
                try
                {
                    if (_clientSettings.AuthType == AuthType.Ptc)
                        await _client.DoPtcLogin(_clientSettings.PtcUsername, _clientSettings.PtcPassword);
                    else if (_clientSettings.AuthType == AuthType.Google)
                        await _client.DoGoogleLogin();

                    await _client.SetServer();

                    await PostLoginExecute();
                }
                catch (AccessTokenExpiredException)
                {
                    Logger.Write("Access token expired");
                }
                catch (TaskCanceledException)
                {
                    Logger.Write("Task Canceled Exception - Restarting", LogLevel.Error);
                    await Execute();
                }
                catch (UriFormatException)
                {
                    Logger.Write("UriFormatException - Restarting", LogLevel.Error);
                    await Execute();
                }
                catch (ArgumentOutOfRangeException)
                {
                    Logger.Write("ArgumentOutOfRangeException - Restarting", LogLevel.Error);
                    await Execute();
                }
                catch (ArgumentNullException)
                {
                    Logger.Write("ArgumentNullException - Restarting", LogLevel.Error);
                    await Execute();
                }
                catch (NullReferenceException)
                {
                    Logger.Write("NullReferenceException - Restarting", LogLevel.Error);
                    await Execute();
                }
                catch (InvalidResponseException e)
                {
                    Logger.Write("InvalidResponseException - Restarting", LogLevel.Error);
                    Logger.Write("err: " + e);
                    await Execute();
                }
                catch (AggregateException)
                {
                    Logger.Write("AggregateException - Restarting", LogLevel.Error);
                    await Execute();
                }
                await Task.Delay(10000);
            }
        }

        private async Task ExecuteCatchAllNearbyPokemons()
        {
            var mapObjects = await _client.GetMapObjects();

            var pokemons =
                mapObjects.MapCells.SelectMany(i => i.CatchablePokemons)
                    .OrderBy(
                        i =>
                            LocationUtils.CalculateDistanceInMeters(
                                new Navigation.Location(_client.CurrentLat, _client.CurrentLng),
                                new Navigation.Location(i.Latitude, i.Longitude)));

            foreach (var pokemon in pokemons)
            {
                if (_clientSettings.UsePokemonToNotCatchFilter &&
                    pokemon.PokemonId.Equals(
                        _clientSettings.PokemonsNotToCatch.FirstOrDefault(i => i == pokemon.PokemonId)))
                {
                    Logger.Write("Skipped " + pokemon.PokemonId);
                    continue;
                }

                var distance = Navigation.DistanceBetween2Coordinates(_client.CurrentLat, _client.CurrentLng,
                    pokemon.Latitude, pokemon.Longitude);
                await Task.Delay(distance > 100 ? 15000 : 500);

                var encounter = await _client.EncounterPokemon(pokemon.EncounterId, pokemon.SpawnpointId);

                if (encounter.Status == EncounterResponse.Types.Status.EncounterSuccess)
                    await CatchEncounter(encounter, pokemon);
                else
                    Logger.Write($"Encounter problem: {encounter.Status}");
            }

            await Task.Delay(_clientSettings.DelayBetweenMove);
        }


        private async Task ExecuteFarmingPokestopsAndPokemons()
        {
            var mapObjects = await _client.GetMapObjects();

            var pokeStops =
                mapObjects.MapCells.SelectMany(i => i.Forts)
                    .Where(
                        i =>
                            i.Type == FortType.Checkpoint &&
                            i.CooldownCompleteTimestampMs < DateTime.UtcNow.ToUnixTime())
                    .OrderBy(
                        i =>
                            LocationUtils.CalculateDistanceInMeters(
                                new Navigation.Location(_client.CurrentLat, _client.CurrentLng),
                                new Navigation.Location(i.Latitude, i.Longitude)));

            foreach (var pokeStop in pokeStops)
            {
                var distance = Navigation.DistanceBetween2Coordinates(_client.CurrentLat, _client.CurrentLng,
                    pokeStop.Latitude, pokeStop.Longitude);

                var update =
                    await
                        _navigation.HumanLikeWalking(new Navigation.Location(pokeStop.Latitude, pokeStop.Longitude),
                            _clientSettings.WalkingSpeedInKilometerPerHour, ExecuteCatchAllNearbyPokemons);

                var fortInfo = await _client.GetFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude);
                var fortSearch = await _client.SearchFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude);
                Logger.Write($"{fortInfo.Name} in ({Math.Round(distance)}m)", LogLevel.Info, ConsoleColor.DarkRed);
                if (fortSearch.ExperienceAwarded > 0)
                {
                    Logger.Write(
                        $"XP: {fortSearch.ExperienceAwarded}, Gems: {fortSearch.GemsAwarded}, Eggs: {fortSearch.PokemonDataEgg} Items: {StringUtils.GetSummedFriendlyNameOfItemAwardList(fortSearch.ItemsAwarded)}",
                        LogLevel.Pokestop);
                    await DisplayPlayerLevelInTitle(true);
                }

                await Task.Delay(1000);
                await RecycleItems();
                await ExecuteCatchAllNearbyPokemons();
                if (_clientSettings.TransferDuplicatePokemon) await TransferDuplicatePokemon();
            }
        }

        private async Task<MiscEnums.Item> GetBestBall(WildPokemon pokemon)
        {
            var pokemonCp = pokemon?.PokemonData?.Cp;

            var pokeBallsCount = await _inventory.GetItemAmountByType(MiscEnums.Item.ITEM_POKE_BALL);
            var greatBallsCount = await _inventory.GetItemAmountByType(MiscEnums.Item.ITEM_GREAT_BALL);
            var ultraBallsCount = await _inventory.GetItemAmountByType(MiscEnums.Item.ITEM_ULTRA_BALL);
            var masterBallsCount = await _inventory.GetItemAmountByType(MiscEnums.Item.ITEM_MASTER_BALL);

            if (masterBallsCount > 0 && pokemonCp >= 2000)
                return MiscEnums.Item.ITEM_MASTER_BALL;
            if (ultraBallsCount > 0 && pokemonCp >= 2000)
                return MiscEnums.Item.ITEM_ULTRA_BALL;
            if (greatBallsCount > 0 && pokemonCp >= 2000)
                return MiscEnums.Item.ITEM_GREAT_BALL;

            if (ultraBallsCount > 0 && pokemonCp >= 1000)
                return MiscEnums.Item.ITEM_ULTRA_BALL;
            if (greatBallsCount > 0 && pokemonCp >= 1000)
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

        public async Task PostLoginExecute()
        {
            while (true)
            {
                _playerProfile = await _client.GetProfile();
                await DisplayPlayerLevelInTitle();
                if (_clientSettings.EvolveAllPokemonWithEnoughCandy)
                    await EvolveAllPokemonWithEnoughCandy(_clientSettings.PokemonsToEvolve);
                if (_clientSettings.TransferDuplicatePokemon) await TransferDuplicatePokemon();
                await DisplayHighests();
                await RecycleItems();
                await ExecuteFarmingPokestopsAndPokemons();

                /*
            * Example calls below
            *
            var profile = await _client.GetProfile();
            var settings = await _client.GetSettings();
            var mapObjects = await _client.GetMapObjects();
            var inventory = await _client.GetInventory();
            var pokemons = inventory.InventoryDelta.InventoryItems.Select(i => i.InventoryItemData?.Pokemon).Where(p => p != null && p?.PokemonId > 0);
            */

                await Task.Delay(10000);
            }
        }

        private async Task RecycleItems()
        {
            var items = await _inventory.GetItemsToRecycle(_clientSettings);

            foreach (var item in items)
            {
                var transfer = await _client.RecycleItem((ItemId) item.Item_, item.Count);
                Logger.Write($"{item.Count}x {item.Item_}", LogLevel.Recycling);
                await Task.Delay(500);
            }
        }

        public async Task RepeatAction(int repeat, Func<Task> action)
        {
            for (var i = 0; i < repeat; i++)
                await action();
        }

        private async Task TransferDuplicatePokemon(bool keepPokemonsThatCanEvolve = false)
        {
            var duplicatePokemons =
                await
                    _inventory.GetDuplicatePokemonToTransfer(keepPokemonsThatCanEvolve,
                        _clientSettings.PokemonsNotToTransfer);

            foreach (var duplicatePokemon in duplicatePokemons)
            {
                if (CalculatePokemonPerfection(duplicatePokemon) >= _clientSettings.KeepMinIVPercentage ||
                    duplicatePokemon.Cp > _clientSettings.KeepMinCP)
                    continue;

                var transfer = await _client.TransferPokemon(duplicatePokemon.Id);
                var bestPokemonOfType = await _inventory.GetHighestPokemonOfTypeByCP(duplicatePokemon);
                Logger.Write(
                    $"{duplicatePokemon.PokemonId} with {duplicatePokemon.Cp} ({CalculatePokemonPerfection(duplicatePokemon).ToString("0.00")} % perfect) CP (Best: {bestPokemonOfType.Cp} | ({CalculatePokemonPerfection(bestPokemonOfType).ToString("0.00")} % perfect))",
                    LogLevel.Transfer);
                await Task.Delay(500);
            }
        }

        public async Task UseBerry(ulong encounterId, string spawnPointId)
        {
            var inventoryBalls = await _inventory.GetItems();
            var berries = inventoryBalls.Where(p => (ItemId) p.Item_ == ItemId.ItemRazzBerry);
            var berry = berries.FirstOrDefault();

            if (berry == null)
                return;

            var useRaspberry = await _client.UseCaptureItem(encounterId, ItemId.ItemRazzBerry, spawnPointId);
            Logger.Write($"Used, remaining: {berry.Count}", LogLevel.Berry);
            await Task.Delay(3000);
        }

        private async Task DisplayHighests()
        {
            Logger.Write("====== DisplayHighestsCP ======", LogLevel.Info, ConsoleColor.Yellow);
            var highestsPokemonCP = await _inventory.GetHighestsCP(20);
            foreach (var pokemon in highestsPokemonCP)
                Logger.Write(
                    $"# CP {pokemon.Cp}\t| ({CalculatePokemonPerfection(pokemon).ToString("0.00")}\t% perfect) NAME: '{pokemon.PokemonId}'",
                    LogLevel.Info, ConsoleColor.Yellow);
            Logger.Write("====== DisplayHighestsPerfect ======", LogLevel.Info, ConsoleColor.Yellow);
            var highestsPokemonPerfect = await _inventory.GetHighestsPerfect(10);
            foreach (var pokemon in highestsPokemonPerfect)
            {
                Logger.Write(
                    $"# CP {pokemon.Cp}\t| ({CalculatePokemonPerfection(pokemon).ToString("0.00")}\t% perfect) NAME: '{pokemon.PokemonId}'",
                    LogLevel.Info, ConsoleColor.Yellow);
            }
        }
    }
}