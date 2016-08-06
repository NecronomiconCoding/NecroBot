#region using directives

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using System.Collections.Generic;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public class VersionCheckState : IState
    {
        public const string VersionUri =
            "https://rawgit.com/NECROBOTIO/NecroBot/master/PoGo.NecroBot.Logic/Properties/AssemblyInfo.cs";

        public const string LatestReleaseApi =
            "https://api.github.com/repos/NECROBOTIO/NecroBot/releases/latest";

        private const string LatestRelease =
            "https://github.com/NECROBOTIO/NecroBot/releases";

        public static Version RemoteVersion;

        public async Task<IState> Execute(ISession session, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await CleanupOldFiles();
            var autoUpdate = session.LogicSettings.AutoUpdate;
            var isLatest = IsLatest();
            if ( isLatest || !autoUpdate )
            {
                if ( isLatest )
                {
                    session.EventDispatcher.Send(new UpdateEvent
                    {
                        Message =
                            session.Translation.GetTranslation(TranslationString.GotUpToDateVersion, Assembly.GetExecutingAssembly().GetName().Version.ToString(3))
                    });
                    return new LoginState();
                }
                session.EventDispatcher.Send(new UpdateEvent
                {
                    Message = session.Translation.GetTranslation(TranslationString.AutoUpdaterDisabled, LatestRelease)
                });

                return new LoginState();
            } else
            {
                Logger.Write( "New update detected, would you like to update? Y/N", LogLevel.Update );

                bool boolBreak = false;
                while( !boolBreak )
                {
                    string strInput = Console.ReadLine().ToLower();

                    switch( strInput )
                    {
                        case "y":
                            boolBreak = true;
                            break;
                        case "n":
                            Logger.Write( "Update Skipped", LogLevel.Update );
                            return new LoginState();
                        default:
                            Logger.Write( session.Translation.GetTranslation( TranslationString.PromptError, "Y", "N" ), LogLevel.Error );
                            continue;
                    }
                }
            }

            session.EventDispatcher.Send(new UpdateEvent
            {
                Message = session.Translation.GetTranslation(TranslationString.DownloadingUpdate)
            });
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

            if (!DownloadFile(downloadLink, downloadFilePath))
                return new LoginState();

            session.EventDispatcher.Send(new UpdateEvent
            {
                Message = session.Translation.GetTranslation(TranslationString.FinishedDownloadingRelease)
            });

            if (!UnpackFile(downloadFilePath, tempPath))
                return new LoginState();

            session.EventDispatcher.Send(new UpdateEvent
            {
                Message = session.Translation.GetTranslation(TranslationString.FinishedUnpackingFiles)
            });

            if (!MoveAllFiles(extractedDir, destinationDir))
                return new LoginState();

            session.EventDispatcher.Send(new UpdateEvent
            {
                Message = session.Translation.GetTranslation(TranslationString.UpdateFinished)
            });

            if( TransferConfig( baseDir, session ) )
                Utils.ErrorHandler.ThrowFatalError( session.Translation.GetTranslation( TranslationString.FinishedTransferringConfig ), 5, LogLevel.Update );
            

            await Task.Delay(2000, cancellationToken);

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
            var files = di.GetFiles("*.old", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                try
                {
                    if (file.Name.Contains("vshost") || file.Name.Contains(".gpx.old"))
                        continue;
                    File.Delete(file.FullName);
                }
                catch (Exception e)
                {
                    Logger.Write(e.ToString());
                }
            }
            await Task.Delay(200);
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

        private static JObject GetJObject(string filePath)
        {
            return JObject.Parse(File.ReadAllText(filePath));
        }


        public static bool IsLatest()
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
                    return false;
            }
            catch (Exception)
            {
                return true; //better than just doing nothing when git server down
            }

            return true;
        }

        public static bool MoveAllFiles(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            var oldfiles = Directory.GetFiles(destFolder);
            foreach (var old in oldfiles)
            {
                if (old.Contains("vshost") || old.Contains(".gpx")) continue;
                File.Move(old, old + ".old");
            }

            try
            {
                var files = Directory.GetFiles(sourceFolder);
                foreach (var file in files)
                {
                    if (file.Contains("vshost") || file.Contains(".gpx")) continue;
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

        private static bool TransferConfig(string baseDir, ISession session)
        {
            if (!session.LogicSettings.TransferConfigAndAuthOnUpdate)
                return false;

            var configDir = Path.Combine(baseDir, "Config");
            if (!Directory.Exists(configDir))
                return false;

            var oldConf = GetJObject(Path.Combine(configDir, "config.json.old"));
            var oldAuth = GetJObject(Path.Combine(configDir, "auth.json.old"));
            GlobalSettings.Load("");

            var newConf = GetJObject(Path.Combine(configDir, "config.json"));
            var newAuth = GetJObject(Path.Combine(configDir, "auth.json"));

            List<JProperty> lstNewOptions = TransferJson(oldConf, newConf);
            TransferJson(oldAuth, newAuth);
            
            File.WriteAllText(Path.Combine(configDir, "config.json"), newConf.ToString());
            File.WriteAllText(Path.Combine(configDir, "auth.json"), newAuth.ToString());

            if( lstNewOptions != null && lstNewOptions.Count > 0 )
            {
                Console.Write( "\n" );
                Logger.Write( "### New Options found ###", LogLevel.New );

                foreach( JProperty prop in lstNewOptions )
                    Logger.Write( prop.ToString(), LogLevel.New );

                Logger.Write( "Would you like to open the Config file? Y/N", LogLevel.Info );
                
                while( true )
                {
                    string strInput = Console.ReadLine().ToLower();

                    switch( strInput )
                    {
                        case "y":
                            Process.Start( Path.Combine( configDir, "config.json" ) );
                            return true;
                        case "n":
                            return true;
                        default:
                            Logger.Write( session.Translation.GetTranslation( TranslationString.PromptError, "y", "n" ), LogLevel.Error );
                            continue;
                    }
                }
            }
            
            return true;
        }

        private static List<JProperty> TransferJson(JObject oldFile, JObject newFile)
        {
            try
            {
                // Figuring out the best method to detect new settings \\
                List<JProperty> lstNewOptions = new List<JProperty>();
                
                foreach( var newProperty in newFile.Properties() )
                {
                    bool boolFound = false;
                    
                    foreach( var oldProperty in oldFile.Properties() )
                    {
                        if( newProperty.Name.Equals( oldProperty.Name ) )
                        {
                            boolFound = true;
                            newFile[ newProperty.Name ] = oldProperty.Value;
                            break;
                        }
                    }

                    if( !boolFound )
                        lstNewOptions.Add( newProperty );
                }

                return lstNewOptions;

                /*foreach (var newProperty in newFile.Properties())
                    foreach (var oldProperty in oldFile.Properties())
                        if (newProperty.Name.Equals(oldProperty.Name))
                        {
                            newFile[newProperty.Name] = oldProperty.Value;
                            break;
                        }*/
            }
            catch( Exception error )
            {
                Console.WriteLine( error.Message );
            }

            return null;
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