#region using directives

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using PoGo.NecroBot.CLI.Resources;
using PoGo.NecroBot.Logic;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.Model.Google;
using PoGo.NecroBot.Logic.Model.Settings;
using PoGo.NecroBot.Logic.Service;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Tasks;
using PoGo.NecroBot.Logic.Utils;

#endregion

namespace PoGo.NecroBot.CLI
{
    internal class Program
    {
        private static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);
        private static string _subPath = "";

        private static readonly Uri StrKillSwitchUri =
            new Uri("https://raw.githubusercontent.com/NoxxDev/NecroBot/master/KillSwitch.txt");

        private static Session _session;

        private static void Main(string[] args)
        {
            var strCulture = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

            var culture = CultureInfo.CreateSpecificCulture("en");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;

            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionEventHandler;

            Console.Title = @"NecroBot";
            Console.CancelKeyPress += (sender, eArgs) =>
            {
                QuitEvent.Set();
                eArgs.Cancel = true;
            };
            if (args.Length > 0)
                _subPath = args[0];

            Logger.SetLogger(new ConsoleLogger(LogLevel.LevelUp), _subPath);

            if (CheckKillSwitch())
                return;

            var profilePath = Path.Combine(Directory.GetCurrentDirectory(), _subPath);
            var profileConfigPath = Path.Combine(profilePath, "config");
            var configFile = Path.Combine(profileConfigPath, "config.json");

            GlobalSettings settings;
            var boolNeedsSetup = false;

            if (File.Exists(configFile))
            {
                // Load the settings from the config file
                // If the current program is not the latest version, ensure we skip saving the file after loading
                // This is to prevent saving the file with new options at their default values so we can check for differences
                settings = GlobalSettings.Load(_subPath, !VersionCheckState.IsLatest());
            }
            else
            {
                settings = new GlobalSettings();
                settings.ProfilePath = profilePath;
                settings.ProfileConfigPath = profileConfigPath;
                settings.GeneralConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "config");
                settings.ConsoleConfig.TranslationLanguageCode = strCulture;

                boolNeedsSetup = true;
            }

            if (args.Length > 1)
            {
                var crds = args[1].Split(',');
                try
                {
                    var lat = double.Parse(crds[0]);
                    var lng = double.Parse(crds[1]);
                    settings.LocationConfig.DefaultLatitude = lat;
                    settings.LocationConfig.DefaultLongitude = lng;
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            var lastPosFile = Path.Combine(profileConfigPath, "LastPos.ini");
            if (File.Exists(lastPosFile) && settings.LocationConfig.StartFromLastPosition)
            {
                var text = File.ReadAllText(lastPosFile);
                var crds = text.Split(':');
                try
                {
                    var lat = double.Parse(crds[0]);
                    var lng = double.Parse(crds[1]);
                    settings.LocationConfig.DefaultLatitude = lat;
                    settings.LocationConfig.DefaultLongitude = lng;
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            var logicSettings = new LogicSettings(settings);
            var translation = Translation.Load(logicSettings);

            if (settings.GPXConfig.UseGpxPathing)
            {
                var xmlString = File.ReadAllText(settings.GPXConfig.GpxFile);
                var readgpx = new GpxReader(xmlString, translation);
                var nearestPt = readgpx.Tracks.SelectMany(
                    (trk, trkindex) =>
                        trk.Segments.SelectMany(
                            (seg, segindex) =>
                                seg.TrackPoints.Select(
                                    (pt, ptindex) =>
                                        new
                                        {
                                            TrackPoint = pt,
                                            TrackIndex = trkindex,
                                            SegIndex = segindex,
                                            PtIndex = ptindex,
                                            Latitude = Convert.ToDouble(pt.Lat, CultureInfo.InvariantCulture),
                                            Longitude = Convert.ToDouble(pt.Lon, CultureInfo.InvariantCulture),
                                            Distance = LocationUtils.CalculateDistanceInMeters(
                                                settings.LocationConfig.DefaultLatitude,
                                                settings.LocationConfig.DefaultLongitude,
                                                Convert.ToDouble(pt.Lat, CultureInfo.InvariantCulture),
                                                Convert.ToDouble(pt.Lon, CultureInfo.InvariantCulture)
                                                )
                                        }
                                    )
                            )
                    ).OrderBy(pt => pt.Distance).FirstOrDefault(pt => pt.Distance <= 5000);

                if (nearestPt != null)
                {
                    settings.LocationConfig.DefaultLatitude = nearestPt.Latitude;
                    settings.LocationConfig.DefaultLongitude = nearestPt.Longitude;
                    settings.LocationConfig.ResumeTrack = nearestPt.TrackIndex;
                    settings.LocationConfig.ResumeTrackSeg = nearestPt.SegIndex;
                    settings.LocationConfig.ResumeTrackPt = nearestPt.PtIndex;
                }
            }

            _session = new Session(new ClientSettings(settings), logicSettings, translation);

            if (boolNeedsSetup)
            {
                Logger.SetLoggerContext(_session);
                if (GlobalSettings.PromptForSetup(_session.Translation))
                {
                    _session = GlobalSettings.SetupSettings(_session, settings, configFile);

                    var fileName = Assembly.GetExecutingAssembly().Location;
                    Process.Start(fileName);
                    Environment.Exit(0);
                }
                else
                {
                    GlobalSettings.Load(_subPath);

                    Logger.Write("Press a Key to continue...",
                        LogLevel.Warning);
                    Console.ReadKey();
                    return;
                }
            }

            ProgressBar.Start("NecroBot is starting up", 10);

            _session.Client.ApiFailure = new ApiFailureStrategy(_session);
            ProgressBar.Fill(20);

            var machine = new StateMachine();
            var stats = new Statistics();

            ProgressBar.Fill(30);
            var strVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            stats.DirtyEvent +=
                () =>
                    Console.Title = $"[Necrobot v{strVersion}] " +
                                    stats.GetTemplatedStats(
                                        _session.Translation.GetTranslation(TranslationString.StatsTemplateString),
                                        _session.Translation.GetTranslation(TranslationString.StatsXpTemplateString));
            ProgressBar.Fill(40);

            var aggregator = new StatisticsAggregator(stats);
            ProgressBar.Fill(50);
            var listener = new ConsoleEventListener();
            ProgressBar.Fill(60);
            var snipeEventListener = new SniperEventListener();

            _session.EventDispatcher.EventReceived += evt => listener.Listen(evt, _session);
            _session.EventDispatcher.EventReceived += evt => aggregator.Listen(evt, _session);
            _session.EventDispatcher.EventReceived += evt => snipeEventListener.Listen(evt, _session);

            if (settings.WebsocketsConfig.UseWebsocket)
            {
                var websocket = new WebSocketInterface(settings.WebsocketsConfig.WebSocketPort, _session);
                _session.EventDispatcher.EventReceived += evt => websocket.Listen(evt, _session);
            }

            ProgressBar.Fill(70);

            machine.SetFailureState(new LoginState());
            ProgressBar.Fill(80);

            Logger.SetLoggerContext(_session);
            ProgressBar.Fill(90);

            _session.Navigation.WalkStrategy.UpdatePositionEvent +=
                (lat, lng) => _session.EventDispatcher.Send(new UpdatePositionEvent {Latitude = lat, Longitude = lng});
            _session.Navigation.WalkStrategy.UpdatePositionEvent += SaveLocationToDisk;
            UseNearbyPokestopsTask.UpdateTimeStampsPokestop += SaveTimeStampsPokestopToDisk;
            CatchPokemonTask.UpdateTimeStampsPokemon += SaveTimeStampsPokemonToDisk;

            ProgressBar.Fill(100);

            machine.AsyncStart(new VersionCheckState(), _session, _subPath);

            try
            {
                Console.Clear();
            }
            catch (IOException)
            {
            }

            if (settings.TelegramConfig.UseTelegramAPI)
                _session.Telegram = new TelegramService(settings.TelegramConfig.TelegramAPIKey, _session);

            if (_session.LogicSettings.UseSnipeLocationServer || _session.LogicSettings.HumanWalkingSnipeUsePogoLocationFeeder)
                SnipePokemonTask.AsyncStart(_session);

            settings.checkProxy(_session.Translation);

            QuitEvent.WaitOne();
        }

        private static void SaveLocationToDisk(double lat, double lng)
        {
            var coordsPath = Path.Combine(_session.LogicSettings.ProfileConfigPath, "LastPos.ini");
            File.WriteAllText(coordsPath, $"{lat}:{lng}");
        }

        private static void SaveTimeStampsPokestopToDisk()
        {
            if (_session == null) return;

            var path = Path.Combine(_session.LogicSettings.ProfileConfigPath, "PokestopTS.txt");
            var fileContent = _session.Stats.PokeStopTimestamps.Select(t => t.ToString()).ToList();

            if (fileContent.Count > 0)
                File.WriteAllLines(path, fileContent.ToArray());
        }

        private static void SaveTimeStampsPokemonToDisk()
        {
            if (_session == null) return;

            var path = Path.Combine(_session.LogicSettings.ProfileConfigPath, "PokemonTS.txt");

            var fileContent = _session.Stats.PokemonTimestamps.Select(t => t.ToString()).ToList();

            if (fileContent.Count > 0)
                File.WriteAllLines(path, fileContent.ToArray());
        }

        private static bool CheckKillSwitch()
        {
            using (var wC = new WebClient())
            {
                try
                {
                    var strResponse = WebClientExtensions.DownloadString(wC, StrKillSwitchUri);

                    if (strResponse == null)
                        return false;

                    var strSplit = strResponse.Split(';');

                    if (strSplit.Length > 1)
                    {
                        var strStatus = strSplit[0];
                        var strReason = strSplit[1];

                        if (strStatus.ToLower().Contains("disable"))
                        {
                            Console.WriteLine(strReason + $"\n");

                            Logger.Write("The bot will now close, please press enter to continue", LogLevel.Error);
                            Console.ReadLine();
                            return true;
                        }
                    }
                    else
                        return false;
                }
                catch (WebException)
                {
                    // ignored
                }
            }

            return false;
        }

        private static void UnhandledExceptionEventHandler(object obj, UnhandledExceptionEventArgs args)
        {
            Logger.Write("Exception caught, writing LogBuffer.", force: true);
            throw new Exception();
        }
    }
}