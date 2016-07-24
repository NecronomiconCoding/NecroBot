using PokemonGo.RocketAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Logic.State
{
    public class VersionCheckState : IState
    {
        public static string _versionUri = "https://raw.githubusercontent.com/NecronomiconCoding/Pokemon-Go-Bot/master/PokemonGo.RocketAPI/Properties/AssemblyInfo.cs";

        public IState Execute(Context ctx)
        {
            bool latest = Git.IsLatest();
            if (latest)
            {
                Logger.Write("Awesome! You have already got the newest version! " + Assembly.GetExecutingAssembly().GetName().Version);
            }
            else
            {
                Logger.Write("There is a new Version available: https://github.com/NecronomiconCoding/Pokemon-Go-Bot", LogLevel.Warning);
                Thread.Sleep(1000);
            }

            return new LoginState();
        }

        public bool IsLatest()
        {
            try
            {
                string gitAssemblyInfo = DownloadServerVersion();

                var regex = new Regex(@"\[assembly\: AssemblyVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]");
                var match = regex.Match(DownloadServerVersion());

                if (!match.Success)
                    return false;

                var gitVersion = new Version($"{match.Groups[1]}.{match.Groups[2]}.{match.Groups[3]}.{match.Groups[4]}");

                if (gitVersion <= Assembly.GetExecutingAssembly().GetName().Version)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return false;
        }

        private static string DownloadServerVersion()
        {
            using (var wC = new WebClient())
            {
                return wC.DownloadString(_versionUri);
            }
        }
    }
}
