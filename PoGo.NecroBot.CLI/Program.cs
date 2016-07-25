#region using directives

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

            StateMachine machine = new StateMachine();
            Statistics stats = new Statistics();
            stats.DirtyEvent += () => System.Console.Title = stats.ToString();

            StatisticsAggregator aggregator = new StatisticsAggregator(stats);
            ConsoleEventListener listener = new ConsoleEventListener();

            machine.EventListener += listener.Listen;
            machine.EventListener += aggregator.Listen;

            machine.SetFailureState(new LoginState());


            machine.AsyncStart(new VersionCheckState(), new Context(new GetClientSettings(), new GetLogicSettings()));

            System.Console.ReadLine();
        }
    }
}