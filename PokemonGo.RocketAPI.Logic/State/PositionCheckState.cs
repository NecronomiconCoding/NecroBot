using PokemonGo.RocketAPI.Logic.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonGo.RocketAPI.Logic.Logging;

namespace PokemonGo.RocketAPI.Logic.State
{
    public class PositionCheckState : IState
    {
        private static Tuple<double, double> GetLatLngFromFile()
        {
            if (File.Exists(Directory.GetCurrentDirectory() + "\\Configs\\Coords.ini") &&
                File.ReadAllText(Directory.GetCurrentDirectory() + "\\Configs\\Coords.ini").Contains(":"))
            {
                var latlngFromFile = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Configs\\Coords.ini");
                var latlng = latlngFromFile.Split(':');
                if (latlng[0].Length != 0 && latlng[1].Length != 0)
                {
                    try
                    {
                        double temp_lat = Convert.ToDouble(latlng[0]);
                        double temp_long = Convert.ToDouble(latlng[1]);

                        if (temp_lat >= -90 && temp_lat <= 90 && temp_long >= -180 && temp_long <= 180)
                        {
                            return new Tuple<double, double>(temp_lat, temp_long);
                        }
                        else
                        {
                            Logger.Write("Coordinates in \"Coords.ini\" file are invalid, using the default coordinates ",
                            LogLevel.Warning);
                            return null;
                        }
                    }
                    catch (FormatException)
                    {
                        Logger.Write("Coordinates in \"Coords.ini\" file are invalid, using the default coordinates ",
                            LogLevel.Warning);
                        return null;
                    }
                }

            }

            return null;
        }

        public IState Execute(Context ctx, StateMachine machine)
        {
            string coordsPath = Directory.GetCurrentDirectory() + "\\Configs\\Coords.ini";
            if (File.Exists(coordsPath))
            {
                Tuple<double, double> latLngFromFile = GetLatLngFromFile();
                if (latLngFromFile != null)
                {
                    double distance = LocationUtils.CalculateDistanceInMeters(latLngFromFile.Item1, latLngFromFile.Item2, ctx.Settings.DefaultLatitude, ctx.Settings.DefaultLongitude);
                    DateTime? lastModified = File.Exists(coordsPath) ? (DateTime?)File.GetLastWriteTime(coordsPath) : null;
                    if (lastModified != null)
                    {
                        double? hoursSinceModified = (DateTime.Now - lastModified).HasValue ? (double?)((DateTime.Now - lastModified).Value.Minutes / 60.0) : null;
                        if (hoursSinceModified != null && hoursSinceModified != 0)
                        {
                            var kmph = (distance / 1000) / (hoursSinceModified ?? .1);
                            if (kmph < 80) // If speed required to get to the default location is < 80km/hr
                            {
                                File.Delete(coordsPath);
                                Logger.Write("Detected realistic Traveling , using UserSettings.settings", LogLevel.Warning);
                            }
                            else
                            {
                                Logger.Write("Not realistic Traveling at " + kmph + ", using last saved Coords.ini", LogLevel.Warning);
                            }
                        }
                    }
                }
            }

            Logger.Write($"Make sure Lat & Lng is right. Exit Program if not! Lat: {ctx.Client.CurrentLatitude} Lng: {ctx.Client.CurrentLongitude}", LogLevel.Warning);

            return new FarmState();
        }
    }
}
