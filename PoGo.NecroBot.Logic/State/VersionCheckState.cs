#region using directives

using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
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

        public static bool AutoUpdate = true; //TODO: read from clientsettings

        public IState Execute(Context ctx, StateMachine machine)
        {
            CleanupOldFiles();

            if (IsLatest())
                //has the newest version NOTE: NOT YET IMPLEMENTED. rest of the code works, just need to check current version against latest
            {
                Logger.Write("Using newest Release of NecroBot");
                return new LoginState();
            }
            Logger.Write(AutoUpdate
                ? "AutoUpdate is enabled, please wait for the bot to begin updating."
                : "AutoUpdate is disabled. Get the latest release from:\n https://github.com/NecronomiconCoding/NecroBot/releases");

            //TODO: change constant URL to latest Release url provided by the API page (see latestReleaseAPI a few lines up)
            const string url = "https://github.com/NecronomiconCoding/NecroBot/releases/download/v0.1.5/";
            const string zipName = "Release.zip";
            const string downloadLink = url + zipName;
            var baseDir = new DirectoryInfo(Directory.GetCurrentDirectory());
            var downloadFilePath = baseDir + "\\" + zipName;
            var tempPath = baseDir + "\\tmp";


            if (DownloadFile(downloadLink, downloadFilePath))
            {
                if (UnpackFile(downloadFilePath, tempPath))
                {
                    var extractedDir = tempPath + "\\Release";
                    var destinationDir = baseDir + "\\";
                    if (MoveAllFiles(extractedDir, destinationDir))
                    {

                    }
                }
            }
            CleanupOldFiles(); //cleansup most of the .old files that were created when the bot updated
            Environment.Exit(0);
            return null;
        }

        public static void CleanupOldFiles()
        {
            var di = new DirectoryInfo(Directory.GetCurrentDirectory());
            var files = di.GetFiles("*.old");
            foreach (var file in files)
                try
                {
                    //Logger.Write($"\nDEBUG: Old file found: {file.FullName}");
                    File.Delete(file.FullName);
                }
                catch
                {
                    // ignored
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
                catch (Exception)
                {
                    return false;
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


        public bool IsLatest() //TODO: actually use this function
        {
            try
            {
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
                File.Move(old, old + ".old");
            }

            try
            {
                var files = Directory.GetFiles(sourceFolder);
                foreach (var file in files)
                {
                    var name = Path.GetFileName(file);
                    if (name == null) continue;
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