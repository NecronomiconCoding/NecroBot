#region using directives

using System;
using System.IO;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Utils;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public class PositionCheckState : IState
    {
        public async Task<IState> Execute(Context ctx, StateMachine machine)
        {
            var coordsPath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Configs" +
                             Path.DirectorySeparatorChar + "Coords.ini";
            if (File.Exists(coordsPath))
            {
                var latLngFromFile = LoadPositionFromDisk(machine);
                if (latLngFromFile != null)
                {
                    var distance = LocationUtils.CalculateDistanceInMeters(latLngFromFile.Item1, latLngFromFile.Item2,
                        ctx.Settings.DefaultLatitude, ctx.Settings.DefaultLongitude);
                    var lastModified = File.Exists(coordsPath) ? (DateTime?) File.GetLastWriteTime(coordsPath) : null;
                    if (lastModified != null)
                    {
                        var hoursSinceModified = (DateTime.Now - lastModified).HasValue
                            ? (double?) ((DateTime.Now - lastModified).Value.Minutes/60.0)
                            : null;
                        if (hoursSinceModified != null && hoursSinceModified != 0)
                        {
                            var kmph = distance/1000/(double) hoursSinceModified;
                            if (kmph < 80) // If speed required to get to the default location is < 80km/hr
                            {
                                File.Delete(coordsPath);
                                machine.Fire(new WarnEvent
                                {
                                    Message = "Detected realistic Traveling , using UserSettings.settings"
                                });
                            }
                            else
                            {
                                machine.Fire(new WarnEvent
                                {
                                    Message = "Not realistic Traveling at " + kmph + ", using last saved Coords.ini"
                                });
                            }
                        }
                    }
                }
            }

            machine.Fire(new WarnEvent
            {
                Message =
                    ctx.Translations.GetTranslation(TranslationString.WelcomeWarning, ctx.Client.CurrentLatitude,
                        ctx.Client.CurrentLongitude)
            });

            return new InfoState();
        }

        private static Tuple<double, double> LoadPositionFromDisk(StateMachine machine)
        {
            if (
                File.Exists(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Configs" +
                            Path.DirectorySeparatorChar + "Coords.ini") &&
                File.ReadAllText(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Configs" +
                                 Path.DirectorySeparatorChar + "Coords.ini").Contains(":"))
            {
                var latlngFromFile =
                    File.ReadAllText(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Configs" +
                                     Path.DirectorySeparatorChar + "Coords.ini");
                var latlng = latlngFromFile.Split(':');
                if (latlng[0].Length != 0 && latlng[1].Length != 0)
                {
                    try
                    {
                        var latitude = Convert.ToDouble(latlng[0]);
                        var longitude = Convert.ToDouble(latlng[1]);

                        if (Math.Abs(latitude) <= 90 && Math.Abs(longitude) <= 180)
                        {
                            return new Tuple<double, double>(latitude, longitude);
                        }
                        machine.Fire(new WarnEvent
                        {
                            Message = "Coordinates in \"Coords.ini\" file are invalid, using the default coordinates"
                        });
                        return null;
                    }
                    catch (FormatException)
                    {
                        machine.Fire(new WarnEvent
                        {
                            Message = "Coordinates in \"Coords.ini\" file are invalid, using the default coordinates"
                        });
                        return null;
                    }
                }
            }

            return null;
        }
    }
}