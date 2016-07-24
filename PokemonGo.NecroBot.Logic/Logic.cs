using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AllEnum;
using PokemonGo.NecroBot.Logic.Helpers;
using PokemonGo.NecroBot.Logic.Utils;
using PokemonGo.RocketAPI;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.Exceptions;
using PokemonGo.RocketAPI.Extensions;
using PokemonGo.RocketAPI.GeneratedCode;
using System.IO;

namespace PokemonGo.NecroBot.Logic
{
    public class Logic
    {
        private readonly Client _client;
        private readonly ISettings _clientSettings;
        private readonly Inventory _inventory;
        private readonly Navigation _navigation;
        private GetPlayerResponse _playerProfile;
        private readonly Statistics _stats;

        public Logic(ISettings clientSettings)
        {
            _clientSettings = clientSettings;
            _client = new Client(_clientSettings);
            _inventory = new Inventory(_client, _clientSettings);
            _navigation = new Navigation(this, _client);
            _stats = new Statistics();
        }

        public async Task Execute()
        {
            GitHelper.CheckVersion();
            ConsoleLogger.WriteConsole($"Make sure Lat & Lng is right. Exit Program if not! Lat: {_client.CurrentLat} Lng: {_client.CurrentLng}",LogLevel.Warning);
            Thread.Sleep(3000);
            ConsoleLogger.WriteConsole($"Logging in via: {_clientSettings.AuthType}");

            while (true)
            {
                try
                {
                    switch (_clientSettings.AuthType)
                    {
                        case AuthType.Ptc:
                            await _client.DoPtcLogin(_clientSettings.PtcUsername, _clientSettings.PtcPassword);
                            break;
                        case AuthType.Google:
                            if (File.Exists(Directory.GetCurrentDirectory() + "\\Configs\\GoogleAuth.ini"))
                            {
                                _client.googleRefreshToken = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Configs\\GoogleAuth.ini");
                            }
                            await _client.DoGoogleLogin();
                            break;
                        default:
                            ConsoleLogger.WriteConsole("Unknown AuthType, please use Ptc or Google as AuthType");
                            Environment.Exit(0);
                            break;
                    }

                    await _client.SetServer();
                    await PostLoginExecute();
                }
                catch (AccountNotVerifiedException)
                {
                    ConsoleLogger.WriteConsole("Account not verified ( not able to login ). - Press any key to exit", LogLevel.Error);
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                catch (Exception e)
                {
                    ConsoleLogger.WriteConsole(e.Message + " from " + e.Source);
                    ConsoleLogger.WriteConsole("Got an exception, trying automatic restart..", LogLevel.Error);
                    await Execute();
                }
                await Task.Delay(10000);
            }
        }

        public async Task PostLoginExecute()
        {
            if (_clientSettings.AuthType == AuthType.Google) {
                File.WriteAllText(Directory.GetCurrentDirectory() + "\\Configs\\GoogleAuth.ini", _client.googleRefreshToken);
            }
            while (true)
            {
                _playerProfile = await _client.GetProfile();
                _stats.SetUsername(_playerProfile);
                if (_clientSettings.EvolveAllPokemonWithEnoughCandy)
                    await EvolveAllPokemonWithEnoughCandy(_clientSettings.PokemonsToEvolve);
                if (_clientSettings.EnableHighestPokemonInfo)
                    await DisplayHighests();
                _stats.UpdateConsoleTitle(_inventory);
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

        private async Task CatchEncounter(EncounterResponse encounter, MapPokemon pokemon)
        {
            CatchPokemonResponse caughtPokemonResponse;
            int attemptCounter = 1;
            do
            {
                var pokeball = await GetBestBall(encounter?.WildPokemon);
                if (pokeball == MiscEnums.Item.ITEM_UNKNOWN)
                {
                    ConsoleLogger.WriteConsole($"No Pokeballs - We missed a {pokemon.PokemonId} with CP {encounter?.WildPokemon?.PokemonData?.Cp}", LogLevel.Caught);
                    return;
                }

                var probability = encounter?.CaptureProbability?.CaptureProbability_?.FirstOrDefault();
                if ((probability.HasValue && probability.Value < 0.35 && encounter.WildPokemon?.PokemonData?.Cp > 400) ||
                    PokemonInfo.CalculatePokemonPerfection(encounter?.WildPokemon?.PokemonData) >= _clientSettings.KeepMinIVPercentage)
                {
                    await UseBerry(pokemon.EncounterId, pokemon.SpawnpointId);
                }

                caughtPokemonResponse = await _client.CatchPokemon(pokemon.EncounterId, pokemon.SpawnpointId, pokemon.Latitude, pokemon.Longitude, pokeball);
                if (caughtPokemonResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchSuccess)
                {
                    foreach (int xp in caughtPokemonResponse.Scores.Xp)
                        _stats.AddExperience(xp);
                    _stats.IncreasePokemons();
                    var profile = await _client.GetProfile();
                    _stats.GetStardust(profile.Profile.Currency.ToArray()[1].Amount);
                }
                _stats.UpdateConsoleTitle(_inventory);

                var distance = LocationUtils.CalculateDistanceInMeters(_client.CurrentLat, _client.CurrentLng, pokemon.Latitude, pokemon.Longitude);
                if (encounter?.CaptureProbability?.CaptureProbability_ != null)
                {
                    string catchStatus = attemptCounter > 1 ? $"{caughtPokemonResponse.Status} Attempt #{attemptCounter}" : $"{caughtPokemonResponse.Status}";
                    //ConsoleLogger.WriteConsole($"({catchStatus}) | {pokemon.PokemonId} Lvl {PokemonInfo.GetLevel(encounter?.WildPokemon?.PokemonData)} " +
                    //                           $"({encounter?.WildPokemon?.PokemonData?.Cp}/{PokemonInfo.CalculateMaxCP(encounter?.WildPokemon?.PokemonData)} CP) " +
                    //                           $"({Math.Round(PokemonInfo.CalculatePokemonPerfection(encounter?.WildPokemon?.PokemonData)).ToString("0.00")}% perfect) " +
                    //                           $"| Chance: {Math.Round(Convert.ToDouble(encounter?.CaptureProbability?.CaptureProbability_.First()) * 100, 2)}% " +
                    //                           $"| {Math.Round(distance)}m dist | with {pokeball}", LogLevel.Caught);
                    ConsoleLogger.WriteConsole($"({catchStatus}) | {pokemon.PokemonId} ({encounter?.WildPokemon?.PokemonData?.Cp}/{PokemonInfo.CalculateMaxCP(encounter?.WildPokemon?.PokemonData)} CP) " +
                                               $"({Math.Round(PokemonInfo.CalculatePokemonPerfection(encounter?.WildPokemon?.PokemonData)).ToString("0.00")}% perfect) " +
                                               $"| Chance: {Math.Round(Convert.ToDouble(encounter?.CaptureProbability?.CaptureProbability_.First()) * 100, 2)}% " +
                                               $"| {Math.Round(distance)}m dist | with {pokeball}", LogLevel.Caught);
                }
                attemptCounter++;
                await Task.Delay(2000);
            } while (caughtPokemonResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchMissed ||
                     caughtPokemonResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchEscape);
        }

        /*
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
                     $"{playerName} | Level {playerStat.Level}: {playerStat.Experience - playerStat.PrevLevelXp - xpDifference}/{playerStat.NextLevelXp - playerStat.PrevLevelXp - xpDifference}XP Stardust: {_playerProfile.Profile.Currency.ToArray()[1].Amount}";
                Console.Title = message;
                if (updateOnly == false)
                    Logger.Write(message);
            }
            if (updateOnly == false)
                await Task.Delay(5000);
        }
        */

        private async Task EvolveAllPokemonWithEnoughCandy(IEnumerable<PokemonId> filter = null)
        {
            var pokemonToEvolve = await _inventory.GetPokemonToEvolve(filter);
            foreach (var pokemon in pokemonToEvolve)
            {
                var evolvePokemonOutProto = await _client.EvolvePokemon(pokemon.Id);
                ConsoleLogger.WriteConsole(evolvePokemonOutProto.Result == EvolvePokemonOut.Types.EvolvePokemonStatus.PokemonEvolvedSuccess
                        ? $"{pokemon.PokemonId} successfully for {evolvePokemonOutProto.ExpAwarded}xp"
                        : $"Failed {pokemon.PokemonId}. EvolvePokemonOutProto.Result was {evolvePokemonOutProto.Result}, stopping evolving {pokemon.PokemonId}",LogLevel.Evolve);
                await Task.Delay(3000);
            }
        }

        private async Task ExecuteCatchAllNearbyPokemons()
        {
            ConsoleLogger.WriteConsole("Looking for pokemon..", LogLevel.Debug);
            var mapObjects = await _client.GetMapObjects();
            var pokemons = mapObjects.MapCells.SelectMany(i => i.CatchablePokemons).OrderBy(i => LocationUtils.CalculateDistanceInMeters(_client.CurrentLat, _client.CurrentLng, i.Latitude, i.Longitude));
            foreach (var pokemon in pokemons)
            {
                if (_clientSettings.UsePokemonToNotCatchFilter && pokemon.PokemonId.Equals(_clientSettings.PokemonsNotToCatch.FirstOrDefault(i => i == pokemon.PokemonId)))
                {
                    ConsoleLogger.WriteConsole("Skipped " + pokemon.PokemonId);
                    continue;
                }

                var distance = LocationUtils.CalculateDistanceInMeters(_client.CurrentLat, _client.CurrentLng, pokemon.Latitude, pokemon.Longitude);
                await Task.Delay(distance > 100 ? 15000 : 500);

                var encounter = await _client.EncounterPokemon(pokemon.EncounterId, pokemon.SpawnpointId);

                if (encounter.Status == EncounterResponse.Types.Status.EncounterSuccess)
                    await CatchEncounter(encounter, pokemon);
                else
                    ConsoleLogger.WriteConsole($"Encounter problem: {encounter.Status}");
                if (pokemons.ElementAtOrDefault(pokemons.Count() - 1) != pokemon) // If pokemon is not last pokemon in list, create delay between catches, else keep moving.
                {
                    await Task.Delay(_clientSettings.DelayBetweenPokemonCatch);
                }
            }
        }


        private async Task ExecuteFarmingPokestopsAndPokemons()
        {
            var mapObjects = await _client.GetMapObjects();
            
            var pokeStops =
                mapObjects.MapCells.SelectMany(i => i.Forts)
                    .Where(
                        i =>
                            i.Type == FortType.Checkpoint &&
                            i.CooldownCompleteTimestampMs < DateTime.UtcNow.ToUnixTime() &&
                            ( // Make sure PokeStop is within max travel distance, unless it's set to 0.
                            LocationUtils.CalculateDistanceInMeters(
                                _clientSettings.DefaultLatitude, _clientSettings.DefaultLongitude,
                                    i.Latitude, i.Longitude) < _clientSettings.MaxTravelDistanceInMeters) ||
                                        _clientSettings.MaxTravelDistanceInMeters == 0
                            );
            
            var pokestopList = pokeStops.ToList();

            while (pokestopList.Any())
            {
                pokestopList = pokestopList.OrderBy(i => LocationUtils.CalculateDistanceInMeters(_client.CurrentLat, _client.CurrentLng, i.Latitude, i.Longitude)).ToList();
                var pokeStop = pokestopList[0];
                pokestopList.RemoveAt(0);
                
                var distance = LocationUtils.CalculateDistanceInMeters(_client.CurrentLat, _client.CurrentLng, pokeStop.Latitude, pokeStop.Longitude);
                var fortInfo = await _client.GetFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude);
                
                var distanceToTarget = LocationUtils.CalculateDistanceInMeters(new GeoCoordinate(_client.CurrentLat, _client.CurrentLng), new GeoCoordinate(pokeStop.Latitude, pokeStop.Longitude));
                ConsoleLogger.WriteConsole($"{fortInfo.Name} in {distanceToTarget:0.##} meters. Will take about {distanceToTarget / (_clientSettings.WalkingSpeedInKilometerPerHour/3.6):0.##} seconds to walk there.", LogLevel.Pokestop);
                var update = await _navigation.HumanLikeWalking(new GeoCoordinate(pokeStop.Latitude, pokeStop.Longitude), _clientSettings.WalkingSpeedInKilometerPerHour, ExecuteCatchAllNearbyPokemons);

                var fortSearch = await _client.SearchFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude);
                if (fortSearch.ExperienceAwarded > 0)
                {
                    _stats.AddExperience(fortSearch.ExperienceAwarded);
                    _stats.UpdateConsoleTitle(_inventory);
                    //todo: fix egg crash
                    ConsoleLogger.WriteConsole($"{fortInfo.Name} XP: {fortSearch.ExperienceAwarded}, Gems: {fortSearch.GemsAwarded}, Items: {StringUtils.GetSummedFriendlyNameOfItemAwardList(fortSearch.ItemsAwarded)}", LogLevel.Pokestop);
                }

                await Task.Delay(1000);
                await RecycleItems();
                if (_clientSettings.TransferDuplicatePokemon)
                    await TransferDuplicatePokemon();
            }
        }

        private async Task RecycleItems()
        {
            var items = await _inventory.GetItemsToRecycle(_clientSettings);

            foreach (var item in items)
            {
                var transfer = await _client.RecycleItem((ItemId) item.Item_, item.Count);
                ConsoleLogger.WriteConsole($"{item.Count}x {(ItemId) item.Item_}", LogLevel.Recycling);
                _stats.AddItemsRemoved(item.Count);
                _stats.UpdateConsoleTitle(_inventory);
                await Task.Delay(500);
            }
        }

        private async Task TransferDuplicatePokemon(bool keepPokemonsThatCanEvolve = false)
        {
            var duplicatePokemons = await _inventory.GetDuplicatePokemonToTransfer(keepPokemonsThatCanEvolve, _clientSettings.PrioritizeIVOverCP,_clientSettings.PokemonsNotToTransfer);

            foreach (var duplicatePokemon in duplicatePokemons)
            {
                if (duplicatePokemon.Cp >= _clientSettings.KeepMinCP || PokemonInfo.CalculatePokemonPerfection(duplicatePokemon) > _clientSettings.KeepMinIVPercentage)
                    continue;
                var transfer = await _client.TransferPokemon(duplicatePokemon.Id);
                _stats.IncreasePokemonsTransfered();
                _stats.UpdateConsoleTitle(_inventory);
                var bestPokemonOfType = await _inventory.GetHighestPokemonOfTypeByCP(duplicatePokemon);
                ConsoleLogger.WriteConsole($"{duplicatePokemon.PokemonId} with {duplicatePokemon.Cp} ({PokemonInfo.CalculatePokemonPerfection(duplicatePokemon).ToString("0.00")} % perfect) CP (Best: {bestPokemonOfType.Cp} | ({PokemonInfo.CalculatePokemonPerfection(bestPokemonOfType).ToString("0.00")} % perfect))", LogLevel.Transfer);
                await Task.Delay(500);
            }
        }

        public async Task UseBerry(ulong encounterId, string spawnPointId)
        {
            var inventoryBalls = await _inventory.GetItems();
            var berries = inventoryBalls.Where(p => (ItemId)p.Item_ == ItemId.ItemRazzBerry);
            var berry = berries.FirstOrDefault();

            if (berry == null || berry.Count <= 0)
                return;

            var useRaspberry = await _client.UseCaptureItem(encounterId, ItemId.ItemRazzBerry, spawnPointId);
            ConsoleLogger.WriteConsole($"Used, remaining: {berry.Count}", LogLevel.Berry);
            await Task.Delay(3000);
        }

        private async Task DisplayHighests()
        {
            ConsoleLogger.WriteConsole("====== Pokemon with highest CP ======", LogLevel.Info, ConsoleColor.Yellow);
            var highestsPokemonCP = await _inventory.GetHighestsCP(10);
            foreach (var pokemon in highestsPokemonCP)
                ConsoleLogger.WriteConsole($"# CP {pokemon.Cp.ToString().PadLeft(4, ' ')}/{PokemonInfo.CalculateMaxCP(pokemon).ToString().PadLeft(4, ' ')} " +
                                           $"| ({PokemonInfo.CalculatePokemonPerfection(pokemon).ToString("0.00")}% perfect)\t| Lvl {PokemonInfo.GetLevel(pokemon).ToString("00")}\t NAME: '{pokemon.PokemonId}'", LogLevel.Info, ConsoleColor.Yellow);

            ConsoleLogger.WriteConsole("====== Pokemon with highest Perfect ======", LogLevel.Info, ConsoleColor.Yellow);
            var highestsPokemonPerfect = await _inventory.GetHighestsPerfect(10);
            foreach (var pokemon in highestsPokemonPerfect)
                ConsoleLogger.WriteConsole($"# CP {pokemon.Cp.ToString().PadLeft(4, ' ')}/{PokemonInfo.CalculateMaxCP(pokemon).ToString().PadLeft(4, ' ')} " +
                                           $"| ({PokemonInfo.CalculatePokemonPerfection(pokemon).ToString("0.00")}% perfect)\t| Lvl {PokemonInfo.GetLevel(pokemon).ToString("00")}\t NAME: '{pokemon.PokemonId}'", LogLevel.Info, ConsoleColor.Yellow);
        }
        
        public async Task<PlayerUpdateResponse> UpdatePlayerLocation(double lat, double lng, double alt)
        {
            //If LasPosition not set to 0, save current position
            if (_clientSettings.LastPositionLatitude != 0 && _clientSettings.LastPositionLongitude != 0)
            {
                _clientSettings.LastPositionLatitude = lat;
                _clientSettings.LastPositionLongitude = lng;
                _clientSettings.LastPositionAltitude = alt;
            }
            return await _client.UpdatePlayerLocation(lat, lng, alt);
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
    }
}
