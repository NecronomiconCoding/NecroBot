#region using directives

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Logging;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public class VersionCheckState : IState
    {
        public static string VersionUri =
            "https://raw.githubusercontent.com/NecronomiconCoding/NecroBot/master/PoGo.NecroBot.Logic/Properties/AssemblyInfo.cs";

        public static string LatestReleaseApi =
            "https://api.github.com/repos/NecronomiconCoding/NecroBot/releases/latest";

        public static bool AutoUpdate;

        public static Version RemoteVersion;

        public async Task<IState> Execute(Context ctx, StateMachine machine)
        {
            AutoUpdate = ctx.Settings.AutoUpdate;
            CleanupOldFiles();
            var needupdate = IsLatest();
            if (!needupdate || !AutoUpdate)
            {
                if (!AutoUpdate)
                {
                    Logger.Write(
                        "AutoUpdate is disabled. Get the latest release from:\n https://github.com/NecronomiconCoding/NecroBot/releases");
                }
                return new LoginState();
            }

            Logger.Write("AutoUpdate is enabled, please wait for the bot to begin updating.");
            const string zipName = "Release.zip";
            var url = $"https://github.com/NecronomiconCoding/NecroBot/releases/download/v{RemoteVersion}/";
            var downloadLink = url + zipName;
            var baseDir = new DirectoryInfo(Directory.GetCurrentDirectory());
            var downloadFilePath = baseDir + "\\" + zipName;
            var tempPath = baseDir + "\\tmp";
            var extractedDir = tempPath + "\\Release";
            var destinationDir = baseDir + "\\";
            if (!DownloadFile(downloadLink, downloadFilePath)) return new LoginState();
            if (!UnpackFile(downloadFilePath, tempPath)) return new LoginState();
            if (!MoveAllFiles(extractedDir, destinationDir)) return new LoginState();
            Console.WriteLine("Update finished, restarting..");
            Process.Start(Assembly.GetEntryAssembly().Location);
            Environment.Exit(0);
            return null;
        }

        public static void CleanupOldFiles()
        {
            if (Directory.Exists(Directory.GetCurrentDirectory() + "\\tmp\\"))
                Directory.Delete(Directory.GetCurrentDirectory() + "\\tmp\\", true);
            var di = new DirectoryInfo(Directory.GetCurrentDirectory());
            var files = di.GetFiles("*.old");
            foreach (var file in files)
                try
                {
                    if (file.Name.Contains("vshost")) continue;
                    File.Delete(file.FullName);
                }
                catch (Exception e)
                {
                    Logger.Write(e.ToString());
                }
        }

        public static bool DownloadFile(string url, string dest)
        {
            using (var client = new WebClient())
            {
                try
                {
                    client.DownloadFile(url, dest);
                }
                catch
                {
                    // ignored
                }
                return true;
            }
        }

        private static string DownloadServerVersion()
        {
            using (var wC = new WebClient())
            {
                return wC.DownloadString(VersionUri);
            }
        }


        public bool IsLatest()
        {
            try
            {
                var regex = new Regex(@"\[assembly\: AssemblyVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]");
                var match = regex.Match(DownloadServerVersion());

                if (!match.Success)
                    return false;

                var gitVersion = new Version($"{match.Groups[1]}.{match.Groups[2]}.{match.Groups[3]}");
                RemoteVersion = gitVersion;
                if (gitVersion >= Assembly.GetExecutingAssembly().GetName().Version)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return true; //better than just doing nothing when git server down
            }

            return false;
        }

        public static bool MoveAllFiles(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            var oldfiles = Directory.GetFiles(destFolder);
            foreach (var old in oldfiles)
            {
                if (old.Contains("vshost")) continue;
                File.Move(old, old + ".old");
            }

            try
            {
                var files = Directory.GetFiles(sourceFolder);
                foreach (var file in files)
                {
                    if (file.Contains("vshost")) continue;
                    var name = Path.GetFileName(file);
                    var dest = Path.Combine(destFolder, name);
                    File.Copy(file, dest, true);
                }

                var folders = Directory.GetDirectories(sourceFolder);

                foreach (var folder in folders)
                {
                    var name = Path.GetFileName(folder);
                    if (name == null) continue;
                    var dest = Path.Combine(destFolder, name);
                    MoveAllFiles(folder, dest);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool UnpackFile(string sourceTarget, string destPath)
        {
            var source = sourceTarget;
            var dest = destPath;
            try
            {
                ZipFile.ExtractToDirectory(source, dest);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}