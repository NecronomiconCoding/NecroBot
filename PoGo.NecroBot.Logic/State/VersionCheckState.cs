#region using directives

using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public class VersionCheckState : IState
    {
        public static string VersionUri =
            "https://raw.githubusercontent.com/NecronomiconCoding/Pokemon-Go-Bot/master/PokemonGo.RocketAPI/Properties/AssemblyInfo.cs";

        public static bool AutoUpdate = true; //TODO: read from clientsettings

        public IState Execute(Context ctx, StateMachine machine)
        {
            if (File.Exists(Assembly.GetExecutingAssembly().Location + ".old")) File.Delete(Assembly.GetExecutingAssembly().Location + ".old"); // delete old update files
            if (IsLatest())
            {
                machine.Fire(new NoticeEvent
                {
                    Message =
                        "Awesome! You have already got the newest version! " +
                        Assembly.GetExecutingAssembly().GetName().Version
                });
            }
            else
            {
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

        public static bool UnpackFile(string sourcePath, string destPath, string fileName)
        {
            var source = Directory.GetCurrentDirectory() + sourcePath + fileName;
            var dest = Directory.GetCurrentDirectory() + destPath + fileName;
            if (Directory.Exists(source)) return false;
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