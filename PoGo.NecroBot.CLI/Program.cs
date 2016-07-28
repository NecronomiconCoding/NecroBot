#region using directives
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using PoGo.NecroBot.Logic;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using PoGo.NecroBot.Logic.Localization;
using PoGo.NecroBot.Logic.Service;
using PoGo.NecroBot.Logic.Tasks;
using PoGo.NecroBot.Logic.Settings;
using PoGo.NecroBot.Logic.Profiles;
using PokemonGo.RocketAPI.Enums;

using Newtonsoft.Json;

#endregion

namespace PoGo.NecroBot.CLI
{
    internal class Program
    {
        static bool IsNewProfile = false;

        private static void Main(string[] args) {
            Logger.SetLogger(new ConsoleLogger(LogLevel.Info));

            List<IProfile> profiles = LoadProfiles(args);
            List<Session> sessions = new List<Logic.State.Session>();

            foreach (var profile in profiles) {
                sessions.Add(new Session(profile));
            }

            if (IsNewProfile) {
                Logger.Write("This is your first start and the bot has generated the default config!", LogLevel.Warning);
                Logger.Write("We will now shutdown to let you configure the bot and then launch it again.", LogLevel.Warning);
                Logger.Write("Press Enter to continue...");
                Console.ReadLine();
                return;
            }
            WebSocketInterface server = new WebSocketInterface();

            foreach (var session in sessions) {
                StartSessionAsync(session, server);    
            }

            while (true) {
                if (Console.KeyAvailable) {
                    var info = Console.ReadKey();
                    if (info.Key == ConsoleKey.Enter)
                        break;
                    Thread.Sleep(5);
                }
            }
        }

        static async Task StartSessionAsync(Session session, WebSocketInterface ws) {
            Logger.SetLogger(new ConsoleLogger(LogLevel.Info), session.BotProfile.FilePath);

            var machine = new StateMachine();
            var stats = new Statistics();
            stats.DirtyEvent += () => Console.Title = stats.GetTemplatedStats(session.Translation.GetTranslation(Logic.Common.TranslationString.StatsTemplateString),
                session.Translation.GetTranslation(Logic.Common.TranslationString.StatsXpTemplateString));

            var aggregator = new StatisticsAggregator(stats);
            var listener = new ConsoleEventListener();
            //var websocket = new WebSocketInterface(settings.WebSocketPort, session.Translation);

            session.EventDispatcher.EventReceived += (IEvent evt) => listener.Listen(evt, session);
            session.EventDispatcher.EventReceived += (IEvent evt) => aggregator.Listen(evt, session);
            session.EventDispatcher.EventReceived += (IEvent evt) => ws.Listen(evt, session);

            machine.SetFailureState(new LoginState());
            Logger.SetLoggerContext(session);

            session.Navigation.UpdatePositionEvent +=
                (lat, lng) => session.EventDispatcher.Send(new UpdatePositionEvent { Latitude = lat, Longitude = lng });

            await machine.AsyncStart(new VersionCheckState(), session);
        }

        static List<IProfile> LoadProfiles(string[] args) {
            List<IProfile> profiles = new List<IProfile>();

            string profileName = "";
            if (args.Length > 0) {
                profileName = args[0];
                if (!Profile.Exists(profileName))
                    profileName = "";
            }

            if (!string.IsNullOrEmpty(profileName)) {
                profiles.Add(Profile.Load(profileName));
            } else {
                profiles = (List<IProfile>)Profile.LoadAll();
            }

            if (profiles.Count < 1) {
                profiles.Add(CaptureProfile());
            }

            return profiles;
        }

        static IProfile CaptureProfile() {
            IProfile profile = null;
            do {
                Console.WriteLine("Creating profile, please enter name:");
                string profileName = Console.ReadLine();
                profile = Profile.Load(profileName);

                if (profile != null) {
                    Console.WriteLine("How would you like to log in with this profile?");
                    Console.WriteLine("(0) Google");
                    Console.WriteLine("(1) Pokemon Trainer Club");
                    string type;
                    do {
                        type = Console.ReadLine();
                    } while (type != "0" && type != "1");

                    AuthType authType = (type == "0") ? AuthType.Google : AuthType.Ptc;
                    string ptcUser = "", ptcPassword = "";
                    if (authType == AuthType.Ptc) {
                        do {
                            Console.WriteLine("Username:");
                            ptcUser = Console.ReadLine();
                        } while (string.IsNullOrEmpty(ptcUser));

                        do {
                            Console.WriteLine("Password:");
                            ptcPassword = Console.ReadLine();
                        } while (string.IsNullOrEmpty(ptcPassword));
                    }

                    profile.Settings.Account.AuthenticationType = authType;
                    if (authType == AuthType.Ptc) {
                        profile.Settings.Account.PtcUsername = ptcUser;
                        profile.Settings.Account.PtcPassword = ptcPassword;
                    }
                }
            } while (profile == null);

            IsNewProfile = true;
            return profile;
        }

      
    }
}