#region using directives

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public class VersionCheckState : IState
    {
        public const string VersionUri =
            "https://raw.githubusercontent.com/NecronomiconCoding/NecroBot/master/PoGo.NecroBot.Logic/Properties/AssemblyInfo.cs";

        public const string LatestReleaseApi =
            "https://api.github.com/repos/NecronomiconCoding/NecroBot/releases/latest";

        private const string LatestRelease =
            "https://github.com/NecronomiconCoding/NecroBot/releases";

        public static Version RemoteVersion;

        public async Task<IState> Execute(Context ctx, StateMachine machine)
        {
            await CleanupOldFiles();
            var autoUpdate = ctx.LogicSettings.AutoUpdate;
            var needupdate =  IsLatest();
            if (!needupdate || !autoUpdate)
            {
                if (!needupdate)
                {
                    machine.Fire(new UpdateEvent
                    {
                        Message =
                            $"Perfect! You already have the newest Version {RemoteVersion}"
                    });
                    return new LoginState();
                }
                machine.Fire(new UpdateEvent
                {
                    Message =
                        $"AutoUpdater is disabled. Get the latest release from: {LatestRelease}\n "
                });

                return new LoginState();
            }
            machine.Fire(new UpdateEvent {Message = "Downloading and apply Update...."});
            var remoteReleaseUrl =
            $"https://github.com/NecronomiconCoding/NecroBot/releases/download/v{RemoteVersion}/";
            const string zipName = "Release.zip";
            var downloadLink = remoteReleaseUrl + zipName;
            var baseDir = Directory.GetCurrentDirectory();
            var downloadFilePath = Path.Combine(baseDir, zipName);
            var tempPath = Path.Combine(baseDir, "tmp");
            var extractedDir = Path.Combine(tempPath, "Release");
            var destinationDir = baseDir + Path.DirectorySeparatorChar;
            Console.WriteLine(downloadLink);
            if (!DownloadFile(downloadLink, downloadFilePath)) return new LoginState();
            machine.Fire(new UpdateEvent {Message = "Finished downloading newest Release..."});
            if (!UnpackFile(downloadFilePath, tempPath)) return new LoginState();
            machine.Fire(new UpdateEvent {Message = "Finished unpacking files..."});

            if (!MoveAllFiles(extractedDir, destinationDir)) return new LoginState();
            machine.Fire(new UpdateEvent {Message = "Update finished, you can close this window now."});

            Process.Start(Assembly.GetEntryAssembly().Location);
            Environment.Exit(-1);
            return null;
        }

        public static async Task CleanupOldFiles()
        {
            var tmpDir = Path.Combine(Directory.GetCurrentDirectory(), "tmp");

            if (Directory.Exists(tmpDir))
            {
                Directory.Delete(tmpDir, true);
            }

            var di = new DirectoryInfo(Directory.GetCurrentDirectory());
            var files = di.GetFiles("*.old");

            foreach (var file in files)
            {
                try
                {
                    if (file.Name.Contains("vshost"))
                        continue;
                    File.Delete(file.FullName);
                }
                catch (Exception e)
                {
                    Logger.Write(e.ToString());
                }
            }
        }

        public static bool DownloadFile(string url, string dest)
        {
            using (var client = new WebClient())
            {
                try
                {
                    client.DownloadFile(url, dest);
                    Console.WriteLine(dest);
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