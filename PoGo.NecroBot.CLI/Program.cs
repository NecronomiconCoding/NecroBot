#region using directives

using System;
using PoGo.NecroBot.Logic;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;

#endregion

namespace PoGo.NecroBot.CLI
{
    internal class Program
    {
        private static void Main()
        {
            Logger.SetLogger(new ConsoleLogger(LogLevel.Info));

            var machine = new StateMachine();
            var stats = new Statistics();
            stats.DirtyEvent += () => Console.Title = stats.ToString();

            var aggregator = new StatisticsAggregator(stats);
            var listener = new ConsoleEventListener();

            machine.EventListener += listener.Listen;
            machine.EventListener += aggregator.Listen;

            machine.SetFailureState(new LoginState());


            machine.AsyncStart(new VersionCheckState(), new Context(new GetClientSettings(), new GetLogicSettings()));

            Console.ReadLine();
        }
    }
}