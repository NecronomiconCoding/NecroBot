#region using directives

using System;
using System.Threading;
using System.Threading.Tasks;
using PokemonGo.RocketAPI.Exceptions;
using PokemonGo.RocketAPI.Logic.State;
using PokemonGo.RocketAPI.Logic.Logging;
using PokemonGo.RocketAPI.Logic.Utils;
using PokemonGo.RocketAPI.Logic;

#endregion

namespace PokemonGo.RocketAPI.Console
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
            machine.AsyncStart(new VersionCheckState(), new Context(new Settings()));

            System.Console.ReadLine();
        }
    }
}