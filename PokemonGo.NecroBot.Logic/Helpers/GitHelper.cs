using PokemonGo.RocketAPI;
using System;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;

namespace PokemonGo.NecroBot.Logic.Helpers
{
    public static class GitHelper
    {
        const string VersionCheckAssemblyUrl = "https://raw.githubusercontent.com/NecronomiconCoding/NecroBot/master/PokemonGo.NecroBot.Logic/Properties/AssemblyInfo.cs";
        
        public static void CheckVersion()
        {
            try
            {
                var match =
                    new Regex(
                        @"\[assembly\: AssemblyVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]")
                        .Match(DownloadServerVersion());

                if (!match.Success)
                    return;
                var gitVersion = new Version($"{match.Groups[1]}.{match.Groups[2]}.{match.Groups[3]}.{match.Groups[4]}");
                if (gitVersion <= Assembly.GetExecutingAssembly().GetName().Version)
                {
                    ConsoleLogger.WriteConsole("Awesome! You have already got the newest version! " + Assembly.GetExecutingAssembly().GetName().Version);
                    return;
                }
                
                ConsoleLogger.WriteConsole("There is a new Version available: https://github.com/NecronomiconCoding/NecroBot", LogLevel.Warning);
                Thread.Sleep(1000);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static string DownloadServerVersion()
        {
            using (var wC = new WebClient())
                return wC.DownloadString(VersionCheckAssemblyUrl);
        }
    }
}
