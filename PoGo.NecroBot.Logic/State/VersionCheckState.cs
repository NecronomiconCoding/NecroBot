#region using directives

using System;
<<<<<<< HEAD
=======
using System.Diagnostics;
>>>>>>> refs/remotes/NecronomiconCoding/master
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
<<<<<<< HEAD
using System.Threading;
using PoGo.NecroBot.Logic.Event;
=======
using System.Threading.Tasks;
>>>>>>> refs/remotes/NecronomiconCoding/master
using PoGo.NecroBot.Logic.Logging;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public class VersionCheckState : IState
    {
        public static string VersionUri =
            "https://raw.githubusercontent.com/NecronomiconCoding/NecroBot/master/PoGo.NecroBot.Logic/Properties/AssemblyInfo.cs";

<<<<<<< HEAD
        public static bool AutoUpdate = true; //TODO: read from clientsettings

        public IState Execute(Context ctx, StateMachine machine)
        {
            if (File.Exists(Assembly.GetExecutingAssembly().Location + ".old")) File.Delete(Assembly.GetExecutingAssembly().Location + ".old"); // delete old update files
            if (IsLatest())
=======
        public static string LatestReleaseApi =
            "https://api.github.com/repos/NecronomiconCoding/NecroBot/releases/latest";

        public static Version RemoteVersion;

        public async Task<IState> Execute(Context ctx, StateMachine machine)
        {
            bool AutoUpdate = ctx.LogicSettings.AutoUpdate;
            CleanupOldFiles();
            var needupdate = IsLatest();
            if (!needupdate || !AutoUpdate)
>>>>>>> refs/remotes/NecronomiconCoding/master
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
<<<<<<< HEAD
                var msg = AutoUpdate
                    ? "Updating to newest Version...."
                    : "There is a new Version available: https://github.com/NecronomiconCoding/Pokemon-Go-Bot";
                machine.Fire(new WarnEvent
                {
                    Message = msg
                });


                const string url = "https://github.com/NecronomiconCoding/NecroBot/archive/master.zip";
                const string zipName = "update.zip";
                var baseDir = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.Parent?.Parent;
                var downloadedFilePath = baseDir + "/tmp/";
                var wrongProjectFilePath = baseDir + "NecroBot-master/"; //TODO:change
                
                //Download binary , unpack it and move it
                if (DownloadFile(url, downloadedFilePath + zipName))
                {
                    if (UnpackFile(downloadedFilePath, baseDir?.ToString(), zipName))
                    {
                        var curFile = Assembly.GetExecutingAssembly().Location;
                        File.Move(curFile, curFile + ".old"); //renames current exe to .exe.old
                        if (MoveAllFiles(wrongProjectFilePath, baseDir?.ToString()))
                        {
                            Logger.Write("Update finished... restarting"); //after moving the new .exe restart
                            Thread.Sleep(2000);
                            System.Diagnostics.Process.Start(curFile); 
                            Environment.Exit(0);
                        }
                        else
                        {
                            Logger.Write("Error moving Files inside " + wrongProjectFilePath);
                        }
                    }
                    else
                    {
                        Logger.Write("Error Unpacking update. Located in /tmp/");
                    }
                }
                else
                {
                    Logger.Write("Error Downloading Update, please download from:");
                    Logger.Write(url);
                }
            }
            return new LoginState();
=======
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
>>>>>>> refs/remotes/NecronomiconCoding/master
        }

        public static bool DownloadFile(string url, string dest)
        {
            using (var client = new WebClient())
            {
                try
                {
                    client.DownloadFile(url, Directory.GetCurrentDirectory() + dest);
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

<<<<<<< HEAD
=======
            var oldfiles = Directory.GetFiles(destFolder);
            foreach (var old in oldfiles)
            {
                if (old.Contains("vshost")) continue;
                File.Move(old, old + ".old");
            }

>>>>>>> refs/remotes/NecronomiconCoding/master
            try
            {
                var files = Directory.GetFiles(sourceFolder);
                foreach (var file in files)
                {
<<<<<<< HEAD
                    var name = Path.GetFileName(file);
                    if (name == null) continue;
=======
                    if (file.Contains("vshost")) continue;
                    var name = Path.GetFileName(file);
>>>>>>> refs/remotes/NecronomiconCoding/master
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

<<<<<<< HEAD
        public static bool UnpackFile(string sourcePath, string destPath, string fileName)
        {
            var source = Directory.GetCurrentDirectory() + sourcePath + fileName;
            var dest = Directory.GetCurrentDirectory() + destPath + fileName;
            if (Directory.Exists(source)) return false;
=======
        public static bool UnpackFile(string sourceTarget, string destPath)
        {
            var source = sourceTarget;
            var dest = destPath;
>>>>>>> refs/remotes/NecronomiconCoding/master
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