using System;
using System.Threading;
using System.Threading.Tasks;
using PokemonGo.RocketAPI.Exceptions;
using PokemonGo.NecroBot.Logic;

namespace PokemonGo.NecroBot.CLI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task.Run(() =>
            {
                try
                {
                    new Logic.Logic(new Settings()).Execute().Wait();
                }
                catch (PtcOfflineException)
                {
                    ConsoleLogger.WriteConsole("PTC Servers are probably down OR your credentials are wrong. Try google", LogLevel.Error);
                    ConsoleLogger.WriteConsole("Trying again in 20 seconds...");
                    Thread.Sleep(20000);
                    new Logic.Logic(new Settings()).Execute().Wait();
                }
                catch (AccountNotVerifiedException)
                {
                    ConsoleLogger.WriteConsole("Account not verified ( not able to login ). - Press any key to exit", LogLevel.Error);
                    System.Console.ReadKey();
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    ConsoleLogger.WriteConsole($"Unhandled exception: {ex}", LogLevel.Error);
                    new Logic.Logic(new Settings()).Execute().Wait();
                }
            });
            System.Console.ReadLine();
        }
    }
}