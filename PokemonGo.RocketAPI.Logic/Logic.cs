#region using directives

using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PokemonGo.RocketAPI.Extensions;
using PokemonGo.RocketAPI.Helpers;
using PokemonGo.RocketAPI.Logic.Utils;
// ReSharper disable CyclomaticComplexity
// ReSharper disable FunctionNeverReturns
using PokemonGoDesktop.API.Proto;
using PokemonGoDesktop.API.Common;

#endregion

namespace PokemonGo.RocketAPI.Logic
{
    public class Logic
    {
        private readonly Client _client;
        private readonly ISettings _clientSettings;
        private readonly Inventory _inventory;
        private readonly Navigation _navigation;
        private readonly Statistics _stats;
        private GetPlayerResponse _playerProfile;

        public Logic(ISettings clientSettings)
        {
            _clientSettings = clientSettings;
            ResetCoords();
            _client = new Client(_clientSettings);
            _inventory = new Inventory(_client);
            _navigation = new Navigation(_client);
            _stats = new Statistics();
        }

        /// <summary>
        /// Resets coords if someone could realistically get back to the default coords points since they were last updated (program was last run)
        /// </summary>
        private void ResetCoords()
        {
            string coordsPath = Directory.GetCurrentDirectory() + "\\Configs\\Coords.ini";
            if (!File.Exists(coordsPath)) return;
            Tuple<double, double> latLngFromFile = Client.GetLatLngFromFile();
            if (latLngFromFile == null) return;
            double distance = LocationUtils.CalculateDistanceInMeters(latLngFromFile.Item1, latLngFromFile.Item2, _clientSettings.DefaultLatitude, _clientSettings.DefaultLongitude);
            DateTime? lastModified = File.Exists(coordsPath) ? (DateTime?)File.GetLastWriteTime(coordsPath) : null;
            if (lastModified == null) return;
            double? hoursSinceModified = (DateTime.Now - lastModified).HasValue ? (double?)((DateTime.Now - lastModified).Value.Minutes / 60.0) : null;
            if (hoursSinceModified == null || hoursSinceModified == 0) return; // Shouldn't really be null, but can be 0 and that's bad for division.
            var kmph = (distance / 1000) / (hoursSinceModified ?? .1);
            if (kmph < 80) // If speed required to get to the default location is < 80km/hr
            {
                File.Delete(coordsPath);
                Logger.Write("Detected realistic Traveling , using UserSettings.settings", LogLevel.Warning);
            }
            else
            {
                Logger.Write("Not realistic Traveling at " + kmph + ", using last saved Coords.ini", LogLevel.Warning);
            }
        }

        private async Task CatchEncounter(EncounterResponse encounter, MapPokemon pokemon)
        {
            CatchPokemonResponse caughtPokemonResponse;
            var attemptCounter = 1;
            do
            {
                var probability = encounter?.CaptureProbability?.CaptureProbability_?.FirstOrDefault();

                var pokeball = await GetBestBall(encounter);
                if (pokeball == ItemId.ItemUnknown)
                {
                    Logger.Write(
                        $"No Pokeballs - We missed a {pokemon.PokemonId} with CP {encounter?.WildPokemon?.PokemonData?.Cp}",
                        LogLevel.Caught);
                    return;
                }
                if ((probability.HasValue && probability.Value < 0.35 && encounter.WildPokemon?.PokemonData?.Cp > 400) ||
                    PokemonInfo.CalculatePokemonPerfection(encounter?.WildPokemon?.PokemonData) >=
                    _clientSettings.KeepMinIVPercentage)
                {
                    await UseBerry(pokemon.EncounterId, pokemon.SpawnPointId);
                }

                var distance = LocationUtils.CalculateDistanceInMeters(_client.CurrentLat, _client.CurrentLng,
                    pokemon.Latitude, pokemon.Longitude);
                caughtPokemonResponse =
                    await
                        _client.CatchPokemon(pokemon.EncounterId, pokemon.SpawnPointId, pokemon.Latitude,
                            pokemon.Longitude, pokeball);
                if (caughtPokemonResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchSuccess)
                {
                    foreach (var xp in caughtPokemonResponse.CaptureAward.Xp)
                        _stats.AddExperience(xp);
                    _stats.IncreasePokemons();
                    var profile = await _client.GetPlayer();
                    _stats.GetStardust(profile.PlayerData.Currencies.ToArray()[1].Amount);
                }
                _stats.UpdateConsoleTitle(_inventory);

                if (encounter?.CaptureProbability?.CaptureProbability_ != null)
                {
                    Func<ItemId, string> returnRealBallName = a =>
                    {
                        switch (a)
                        {
                            case ItemId.ItemPokeBall:
                                return "Poke";
                            case ItemId.ItemGreatBall:
                                return "Great";
                            case ItemId.ItemUltraBall:
                                return "Ultra";
                            case ItemId.ItemMasterBall:
                                return "Master";
                            default:
                                return "Unknown";
                        }
                    };
                    var catchStatus = attemptCounter > 1
                        ? $"{caughtPokemonResponse.Status} Attempt #{attemptCounter}"
                        : $"{caughtPokemonResponse.Status}";
                    Logger.Write(
                        $"({catchStatus}) | {pokemon.PokemonId} Lvl {PokemonInfo.GetLevel(encounter.WildPokemon?.PokemonData)} ({encounter.WildPokemon?.PokemonData?.Cp}/{PokemonInfo.CalculateMaxCP(encounter.WildPokemon?.PokemonData)} CP) ({Math.Round(PokemonInfo.CalculatePokemonPerfection(encounter.WildPokemon?.PokemonData)).ToString("0.00")}% perfect) | Chance: {Math.Round(Convert.ToDouble(encounter.CaptureProbability?.CaptureProbability_.First())*100, 2)}% | {Math.Round(distance)}m dist | with a {returnRealBallName(pokeball)}Ball.",
                        LogLevel.Caught);
                }
                attemptCounter++;
                await Task.Delay(2000);
            } while (caughtPokemonResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchMissed ||
                     caughtPokemonResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchEscape);
        }

        private async Task DisplayHighests()
        {
            Logger.Write("====== DisplayHighestsCP ======", LogLevel.Info, ConsoleColor.Yellow);
            var highestsPokemonCp = await _inventory.GetHighestsCp(20);
            foreach (var pokemon in highestsPokemonCp)
                Logger.Write(
                    $"# CP {pokemon.Cp.ToString().PadLeft(4, ' ')}/{PokemonInfo.CalculateMaxCP(pokemon).ToString().PadLeft(4, ' ')} | ({PokemonInfo.CalculatePokemonPerfection(pokemon).ToString("0.00")}% perfect)\t| Lvl {PokemonInfo.GetLevel(pokemon).ToString("00")}\t NAME: '{pokemon.PokemonId.ToString()}'",
                    LogLevel.Info, ConsoleColor.Yellow);
            Logger.Write("====== DisplayHighestsPerfect ======", LogLevel.Info, ConsoleColor.Yellow);
            var highestsPokemonPerfect = await _inventory.GetHighestsPerfect(10);
            foreach (var pokemon in highestsPokemonPerfect)
            {
                Logger.Write(
                    $"# CP {pokemon.Cp.ToString().PadLeft(4, ' ')}/{PokemonInfo.CalculateMaxCP(pokemon).ToString().PadLeft(4, ' ')} | ({PokemonInfo.CalculatePokemonPerfection(pokemon).ToString("0.00")}% perfect)\t| Lvl {PokemonInfo.GetLevel(pokemon).ToString("00")}\t NAME: '{pokemon.PokemonId.ToString()}'",
                    LogLevel.Info, ConsoleColor.Yellow);
            }
        }

        private async Task EvolveAllPokemonWithEnoughCandy(IEnumerable<PokemonId> filter = null)
        {
            if (_clientSettings.useLuckyEggsWhileEvolving)
            {
                await PopLuckyEgg(_client);
            }
            var pokemonToEvolve = await _inventory.GetPokemonToEvolve(filter);
            foreach (var pokemon in pokemonToEvolve)
            {
                var evolvePokemonOutProto = await _client.EvolvePokemon(pokemon.Id);

                Logger.Write(
                    evolvePokemonOutProto.Result == EvolvePokemonResponse.Types.Result.Success
                        ? $"{pokemon.PokemonId} successfully for {evolvePokemonOutProto.ExperienceAwarded}xp"
                        : $"Failed {pokemon.PokemonId}. EvolvePokemonOutProto.Result was {evolvePokemonOutProto.Result}, stopping evolving {pokemon.PokemonId}",
                    LogLevel.Evolve);

                await Task.Delay(3000);
            }
        }

        public async Task Execute()
        {
            Git.CheckVersion();
            DirectoryInfo diConfigs = Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Configs"); //create config folder if not exist for those who can't build
            DirectoryInfo diLogs = Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Logs");
            Logger.Write(
                $"Make sure Lat & Lng is right. Exit Program if not! Lat: {_client.CurrentLat} Lng: {_client.CurrentLng}",
                LogLevel.Warning);
            Thread.Sleep(3000);
            Logger.Write($"Logging in via: {_clientSettings.AuthType}");

            while (true)
            {
                try
                {
                    switch (_clientSettings.AuthType)
                    {
                        case AuthType.PTC:
                            await _client.DoPtcLogin(_clientSettings.PtcUsername, _clientSettings.PtcPassword);
                            break;
                        case AuthType.Google:
                            await _client.DoGoogleLogin();
                            break;
                        default:
                            Logger.Write("wrong AuthType");
                            Environment.Exit(0);
                            break;
                    }

                    await _client.SetServer();

                    await PostLoginExecute();
                }
                catch (Exception e)
                {
                    Logger.Write(e.Message + " from " + e.Source + " at " + e.StackTrace);
                    Logger.Write("Got an exception, trying automatic restart..", LogLevel.Error);
                    await Execute();
                }
                await Task.Delay(10000);
            }
        }

        private async Task ExecuteCatchAllNearbyPokemons()
        {
            Logger.Write("Looking for pokemon..", LogLevel.Debug);
            var mapObjects = await _client.GetMapObjects();

            var pokemons =
                mapObjects.MapCells.SelectMany(i => i.CatchablePokemons)
                    .OrderBy(
                        i =>
                            LocationUtils.CalculateDistanceInMeters(_client.CurrentLat, _client.CurrentLng, i.Latitude,
                                i.Longitude));

            foreach (var pokemon in pokemons)
            {
                if (_clientSettings.UsePokemonToNotCatchFilter &&
                    pokemon.PokemonId.Equals(
                        _clientSettings.PokemonsNotToCatch.FirstOrDefault(i => i == pokemon.PokemonId)))
                {
                    Logger.Write("Skipped " + pokemon.PokemonId);
                    continue;
                }

                var distance = LocationUtils.CalculateDistanceInMeters(_client.CurrentLat, _client.CurrentLng,
                    pokemon.Latitude, pokemon.Longitude);
                await Task.Delay(distance > 100 ? 15000 : 500);

                var encounter = await _client.EncounterPokemon(pokemon.EncounterId, pokemon.SpawnPointId);

                if (encounter.Status == EncounterResponse.Types.Status.EncounterSuccess)
                    await CatchEncounter(encounter, pokemon);
                else
                    Logger.Write($"Encounter problem: {encounter.Status}");
                if (!Equals(pokemons.ElementAtOrDefault(pokemons.Count() - 1), pokemon))
                    // If pokemon is not last pokemon in list, create delay between catches, else keep moving.
                {
                    await Task.Delay(_clientSettings.DelayBetweenPokemonCatch);
                }
            }
        }

        private async Task ExecuteFarmingPokestopsAndPokemons(bool path)
        {
            if (!path)
                await ExecuteFarmingPokestopsAndPokemons();
            else
            {
                var tracks = GetGpxTracks();
                var curTrkPt = 0;
                var curTrk = 0;
                var maxTrk = tracks.Count - 1;
                var curTrkSeg = 0;
                while (curTrk <= maxTrk)
                {
                    var track = tracks.ElementAt(curTrk);
                    var trackSegments = track.Segments;
                    var maxTrkSeg = trackSegments.Count - 1;
                    while (curTrkSeg <= maxTrkSeg)
                    {
                        var trackPoints = track.Segments.ElementAt(0).TrackPoints;
                        var maxTrkPt = trackPoints.Count - 1;
                        while (curTrkPt <= maxTrkPt)
                        {
                            var nextPoint = trackPoints.ElementAt(curTrkPt);
                            if (
                                LocationUtils.CalculateDistanceInMeters(_client.CurrentLat, _client.CurrentLng,
                                    Convert.ToDouble(nextPoint.Lat), Convert.ToDouble(nextPoint.Lon)) > 5000)
                            {
                                Logger.Write(
                                    $"Your desired destination of {nextPoint.Lat}, {nextPoint.Lon} is too far from your current position of {_client.CurrentLat}, {_client.CurrentLng}",
                                    LogLevel.Error);
                                break;
                            }

                            Logger.Write(
                                $"Your desired destination is {nextPoint.Lat}, {nextPoint.Lon} your location is {_client.CurrentLat}, {_client.CurrentLng}",
                                LogLevel.Warning);

                            // Wasn't sure how to make this pretty. Edit as needed.
                            var mapObjects = await _client.GetMapObjects();
                            var pokeStops =
                                mapObjects.MapCells.SelectMany(i => i.Forts)
                                    .Where(
                                        i =>
                                            i.Type == FortType.Checkpoint &&
                                            i.CooldownCompleteTimestampMs < DateTime.UtcNow.ToUnixTime() &&
                                            ( // Make sure PokeStop is within 40 meters, otherwise we cannot hit them.
                                                LocationUtils.CalculateDistanceInMeters(
                                                    _client.CurrentLat, _client.CurrentLng,
                                                    i.Latitude, i.Longitude) < 40)
                                    );

                            var pokestopList = pokeStops.ToList();

                            while (pokestopList.Any())
                            {
                                pokestopList =
                                    pokestopList.OrderBy(
                                        i =>
                                            LocationUtils.CalculateDistanceInMeters(_client.CurrentLat,
                                                _client.CurrentLng, i.Latitude, i.Longitude)).ToList();
                                var pokeStop = pokestopList[0];
                                pokestopList.RemoveAt(0);

                                await _client.GetFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude);

                                var fortSearch =
                                    await _client.SearchFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude);
                                if (fortSearch.ExperienceAwarded > 0)
                                {
                                    _stats.AddExperience(fortSearch.ExperienceAwarded);
                                    _stats.UpdateConsoleTitle(_inventory);
                                    //todo: fix egg crash
                                    Logger.Write(
                                        $"XP: {fortSearch.ExperienceAwarded}, Gems: {fortSearch.GemsAwarded}, Items: {StringUtils.GetSummedFriendlyNameOfItemAwardList(fortSearch.ItemsAwarded)}",
                                        LogLevel.Pokestop);
                                }

                                await Task.Delay(1000);
                                await RecycleItems();
                                if (_clientSettings.TransferDuplicatePokemon) await TransferDuplicatePokemon();
                            }

                                await
                                    _navigation.HumanPathWalking(trackPoints.ElementAt(curTrkPt),
                                        _clientSettings.WalkingSpeedInKilometerPerHour, ExecuteCatchAllNearbyPokemons);

                            if (curTrkPt >= maxTrkPt)
                                curTrkPt = 0;
                            else
                                curTrkPt++;
                        } //end trkpts
                        if (curTrkSeg >= maxTrkSeg)
                            curTrkSeg = 0;
                        else
                            curTrkSeg++;
                    } //end trksegs
                    if (curTrk >= maxTrkSeg)
                        curTrk = 0;
                    else
                        curTrk++;
                } //end tracks
            }
        }

        private async Task ExecuteFarmingPokestopsAndPokemons()
        {
            var distanceFromStart = LocationUtils.CalculateDistanceInMeters(
                _clientSettings.DefaultLatitude, _clientSettings.DefaultLongitude,
                _client.CurrentLat, _client.CurrentLng);

            // Edge case for when the client somehow ends up outside the defined radius
            if (_clientSettings.MaxTravelDistanceInMeters != 0 &&
                distanceFromStart > _clientSettings.MaxTravelDistanceInMeters)
            {
                Logger.Write(
                    $"You're outside of your defined radius! Walking to start ({distanceFromStart}m away) in 5 seconds. Is your Coords.ini file correct?",
                    LogLevel.Warning);
                await Task.Delay(5000);
                Logger.Write("Moving to start location now.");
                await _navigation.HumanLikeWalking(
                    new GeoCoordinate(_clientSettings.DefaultLatitude, _clientSettings.DefaultLongitude),
                    _clientSettings.WalkingSpeedInKilometerPerHour, null);
            }

            var mapObjects = await _client.GetMapObjects();

            // Wasn't sure how to make this pretty. Edit as needed.
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
            var stopsHit = 0;

            if (pokestopList.Count <= 0)
                Logger.Write("No usable PokeStops found in your area. Is your maximum distance too small?",
                    LogLevel.Warning);

            while (pokestopList.Any())
            {
                //resort
                pokestopList =
                    pokestopList.OrderBy(
                        i =>
                            LocationUtils.CalculateDistanceInMeters(_client.CurrentLat, _client.CurrentLng, i.Latitude,
                                i.Longitude)).ToList();
                var pokeStop = pokestopList[0];
                pokestopList.RemoveAt(0);


                var distance = LocationUtils.CalculateDistanceInMeters(_client.CurrentLat, _client.CurrentLng,
                    pokeStop.Latitude, pokeStop.Longitude);
                var fortInfo = await _client.GetFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude);

                Logger.Write($"{fortInfo.Name} in ({Math.Round(distance)}m)", LogLevel.Info, ConsoleColor.DarkRed);
                    await
                        _navigation.HumanLikeWalking(new GeoCoordinate(pokeStop.Latitude, pokeStop.Longitude),
                            _clientSettings.WalkingSpeedInKilometerPerHour, ExecuteCatchAllNearbyPokemons);

                var fortSearch = await _client.SearchFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude);
                if (fortSearch.ExperienceAwarded > 0)
                {
                    _stats.AddExperience(fortSearch.ExperienceAwarded);
                    _stats.UpdateConsoleTitle(_inventory);
                    //todo: fix egg crash
                    Logger.Write(
                        $"XP: {fortSearch.ExperienceAwarded}, Gems: {fortSearch.GemsAwarded}, Items: {StringUtils.GetSummedFriendlyNameOfItemAwardList(fortSearch.ItemsAwarded)}",
                        LogLevel.Pokestop);
                }

                await Task.Delay(1000);
                if (++stopsHit%5 == 0) //TODO: OR item/pokemon bag is full
                {
                    stopsHit = 0;
                    await RecycleItems();
                    if (_clientSettings.EvolveAllPokemonWithEnoughCandy || _clientSettings.EvolveAllPokemonAboveIV)
                        await EvolveAllPokemonWithEnoughCandy(_clientSettings.PokemonsToEvolve);
                    if (_clientSettings.TransferDuplicatePokemon) await TransferDuplicatePokemon();
                }
            }
        }


        private async Task<ItemId> GetBestBall(EncounterResponse encounter)
        {
            var pokemonCp = encounter?.WildPokemon?.PokemonData?.Cp;
            var iV = Math.Round(PokemonInfo.CalculatePokemonPerfection(encounter?.WildPokemon?.PokemonData));
            var proba = encounter?.CaptureProbability?.CaptureProbability_.First();

            var pokeBallsCount = await _inventory.GetItemAmountByType(ItemId.ItemPokeBall);
            var greatBallsCount = await _inventory.GetItemAmountByType(ItemId.ItemGreatBall);
            var ultraBallsCount = await _inventory.GetItemAmountByType(ItemId.ItemUltraBall);
            var masterBallsCount = await _inventory.GetItemAmountByType(ItemId.ItemMasterBall);

            if (masterBallsCount > 0 && pokemonCp >= 1200)
                return ItemId.ItemMasterBall;
            if (ultraBallsCount > 0 && pokemonCp >= 1000)
                return ItemId.ItemUltraBall;
            if (greatBallsCount > 0 && pokemonCp >= 750)
                return ItemId.ItemGreatBall;

            if (ultraBallsCount > 0 && iV >= _clientSettings.KeepMinIVPercentage && proba < 0.40)
                return ItemId.ItemUltraBall;

            if (greatBallsCount > 0 && iV >= _clientSettings.KeepMinIVPercentage && proba < 0.50)
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


/*
        private GpxReader.Trk GetGpxTrack(string gpxFile)
        {
            var xmlString = File.ReadAllText(_clientSettings.GPXFile);
            var readgpx = new GpxReader(xmlString);
            return readgpx.Tracks.ElementAt(0);
        }
*/

        private List<GpxReader.Trk> GetGpxTracks()
        {
            var xmlString = File.ReadAllText(_clientSettings.GPXFile);
            var readgpx = new GpxReader(xmlString);
            return readgpx.Tracks;
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

        public static int GetXpDiff(int level)
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

/*
        private async Task LoadAndDisplayGpxFile()
        {
            var xmlString = File.ReadAllText(_clientSettings.GPXFile);
            var readgpx = new GpxReader(xmlString);
            foreach (var trk in readgpx.Tracks)
            {
                foreach (var trkseg in trk.Segments)
                {
                    foreach (var trpkt in trkseg.TrackPoints)
                    {
                        Console.WriteLine(trpkt.ToString());
                    }
                }
            }
            await Task.Delay(0);
        }
*/

        private async Task PopLuckyEgg(Client client)
        {
            await Task.Delay(1000);
            await UseLuckyEgg(client);
            await Task.Delay(1000);
        }

        public async Task PostLoginExecute()
        {
            while (true)
            {
                _playerProfile = await _client.GetPlayer();
                _stats.SetUsername(_playerProfile);
                if (_clientSettings.EvolveAllPokemonWithEnoughCandy || _clientSettings.EvolveAllPokemonAboveIV)
                    await EvolveAllPokemonWithEnoughCandy(_clientSettings.PokemonsToEvolve);
                if (_clientSettings.TransferDuplicatePokemon) await TransferDuplicatePokemon();
                await DisplayHighests();
                _stats.UpdateConsoleTitle(_inventory);
                await RecycleItems();
                await ExecuteFarmingPokestopsAndPokemons(_clientSettings.UseGPXPathing);

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
                await _client.RecycleItem(item.ItemId, item.Count);
                Logger.Write($"{item.Count}x {item.ItemId}", LogLevel.Recycling);
                _stats.AddItemsRemoved(item.Count);
                _stats.UpdateConsoleTitle(_inventory);
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
                        _clientSettings.PrioritizeIVOverCP, _clientSettings.PokemonsNotToTransfer);

            foreach (var duplicatePokemon in duplicatePokemons)
            {
                if (duplicatePokemon.Cp >= _clientSettings.KeepMinCP ||
                    PokemonInfo.CalculatePokemonPerfection(duplicatePokemon) > _clientSettings.KeepMinIVPercentage)
                    continue;
                await _client.TransferPokemon(duplicatePokemon.Id);
                _inventory.DeletePokemonFromInvById(duplicatePokemon.Id);
                _stats.IncreasePokemonsTransfered();
                _stats.UpdateConsoleTitle(_inventory);
                var bestPokemonOfType = _client.Settings.PrioritizeIVOverCP
                    ? await _inventory.GetHighestPokemonOfTypeByIv(duplicatePokemon)
                    : await _inventory.GetHighestPokemonOfTypeByCp(duplicatePokemon);
                Logger.Write(
                    $"{duplicatePokemon.PokemonId} with {duplicatePokemon.Cp} ({PokemonInfo.CalculatePokemonPerfection(duplicatePokemon).ToString("0.00")} % perfect) CP (Best: {bestPokemonOfType.Cp} | ({PokemonInfo.CalculatePokemonPerfection(bestPokemonOfType).ToString("0.00")} % perfect))",
                    LogLevel.Transfer);
                await Task.Delay(500);
            }
        }

        public async Task UseBerry(ulong encounterId, string spawnPointId)
        {
            var inventoryBalls = await _inventory.GetItems();
            var berries = inventoryBalls.Where(p => p.ItemId == ItemId.ItemRazzBerry);
            var berry = berries.FirstOrDefault();

            if (berry == null || berry.Count <= 0)
                return;

            await _client.UseCaptureItem(encounterId, ItemId.ItemRazzBerry, spawnPointId);
            Logger.Write($"Used, remaining: {berry.Count}", LogLevel.Berry);
            await Task.Delay(3000);
        }

        public async Task UseLuckyEgg(Client client)
        {
            var inventory = await _inventory.GetItems();
            var luckyEggs = inventory.Where(p => (ItemId) p.ItemId == ItemId.ItemLuckyEgg);
            var luckyEgg = luckyEggs.FirstOrDefault();

            if (luckyEgg == null || luckyEgg.Count <= 0)
                return;

            await _client.UseItemXpBoost(ItemId.ItemLuckyEgg);
            Logger.Write($"Used Lucky Egg, remaining: {luckyEgg.Count - 1}", LogLevel.Egg);
            await Task.Delay(3000);
        }
    }
}
