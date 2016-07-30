#region using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using PoGo.NecroBot.Logic;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using PoGo.NecroBot.Logic.Localization;
using PoGo.NecroBot.Logic.Service;
using PoGo.NecroBot.Logic.Tasks;
using PoGo.NecroBot.Logic.Common;

#endregion

namespace PoGo.NecroBot.CLI
{
    internal class Program
    {
        static int lastStartDustCount = 0;
        private static void Main(string[] args)
        {
            var subPath = "";
            if (args.Length > 0)
                subPath = args[0];

            Logger.SetLogger(new ConsoleLogger(LogLevel.Info), subPath);

            var settings = GlobalSettings.Load(subPath);


            if (settings == null)
            {
                Logger.Write("This is your first start and the bot has generated the default config!", LogLevel.Warning);
                Logger.Write("We will now shutdown to let you configure the bot and then launch it again.", LogLevel.Warning);
                Thread.Sleep(2000);
                Environment.Exit(0);
                return;
            }
            var session = new Session(new ClientSettings(settings), new LogicSettings(settings));
            //session.Client.ApiFailure = new ApiFailureStrategy(session);


            /*SimpleSession session = new SimpleSession
            {
                _client = new PokemonGo.RocketAPI.Client(new ClientSettings(settings)),
                _dispatcher = new EventDispatcher(),
                _localizer = new Localizer()
            };

            BotService service = new BotService
            {
                _session = session,
                _loginTask = new Login(session)
            };

            service.Run();
            */

            var machine = new StateMachine();
            var stats = new Statistics();
            stats.DirtyEvent += () =>
            {
                Console.Title = stats.GetTemplatedStats(session.Translation.GetTranslation(Logic.Common.TranslationString.StatsTemplateString),
session.Translation.GetTranslation(Logic.Common.TranslationString.StatsXpTemplateString));
                lastStartDustCount = stats.TotalStardust;
            };

            var aggregator = new StatisticsAggregator(stats);
            var listener = new ConsoleEventListener();
            var websocket = new WebSocketInterface(settings.WebSocketPort, session.Translation);

            session.EventDispatcher.EventReceived += (IEvent evt) => listener.Listen(evt, session);
            session.EventDispatcher.EventReceived += (IEvent evt) => aggregator.Listen(evt, session);
            session.EventDispatcher.EventReceived += (IEvent evt) => websocket.Listen(evt, session);

            machine.SetFailureState(new LoginState());

            Logger.SetLoggerContext(session);

            session.Navigation.UpdatePositionEvent +=
                (lat, lng) => session.EventDispatcher.Send(new UpdatePositionEvent { Latitude = lat, Longitude = lng });

            machine.AsyncStart(new VersionCheckState(), session);

            //Non-blocking key reader
            //This will allow to process console key presses in another code parts
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var info = Console.ReadKey();
                    if (info.Key == ConsoleKey.Enter)
                        break;
                    else if (info.Key == ConsoleKey.F1)
                    {
                        var pokemons = session.Inventory.GetPokemonsOrderByCP().Result;
                        var i = 0;
                        foreach (var item in pokemons)
                        {
                            i++;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Name: " + item.PokemonId + " - Cp: " + item.Cp + " - Hp: " + item.Stamina);
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Total Count: " + i);
                    }
                    else if (info.Key == ConsoleKey.F2)
                    {
                        var items = session.Inventory.GetItems().Result;
                        foreach (var item in items)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Name: " + (item.ItemId.ToString().Substring(0, 4).Equals("Item") ? item.ItemId.ToString().Substring(4) : item.ItemId.ToString()) + " - Count: " + item.Count);
                        }
                    }
                    else if (info.Key == ConsoleKey.F3)
                    {
                        var playerStats = session.Inventory.GetPlayerStats().Result;
                        foreach (var item in playerStats)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Level: " + item.Level);
                            Console.WriteLine("Exp: " + item.Experience + "/" + item.NextLevelXp);
                            Console.WriteLine("Thrown PokeBall: " + item.PokeballsThrown);
                            Console.WriteLine("Deployed Pokemon: " + item.PokemonDeployed);
                            Console.WriteLine("Captured Pokemon: " + item.PokemonsCaptured);
                            Console.WriteLine("Visited Pokestop: " + item.PokeStopVisits);
                            Console.WriteLine("Encountered Pokemon: " + item.PokemonsEncountered);
                            Console.WriteLine("Total Battle Attack: " + item.BattleAttackTotal);
                            Console.WriteLine("Total Battle Attack Won: " + item.BattleAttackWon);
                            Console.WriteLine("Total Battle Defend Won: " + item.BattleDefendedWon);
                            Console.WriteLine("Total Battle Training: " + item.BattleTrainingTotal);
                            Console.WriteLine("Total Battle Training Won: " + item.BattleTrainingWon);
                            Console.WriteLine("Hatched Eggs: " + item.EggsHatched);
                            Console.WriteLine("Evolutions: " + item.Evolutions);
                            Console.WriteLine("Total Walked: " + item.KmWalked + " km");
                            Console.WriteLine("Unique Pokedex Entries: " + item.UniquePokedexEntries);
                            Console.WriteLine("Total Stardust: " + (lastStartDustCount != 0 ? lastStartDustCount.ToString() : "getting..."));
                        }
                    }
                }
                Thread.Sleep(5);
            }
        }
    }
}
