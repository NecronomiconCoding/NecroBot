using PokemonGo.RocketAPI.Logic.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Logic.State
{
    public class PositionCheckState : IState
    {
        public IState Execute(Context ctx, StateMachine machine)
        {
            string coordsPath = Directory.GetCurrentDirectory() + "\\Configs\\Coords.ini";
            if (File.Exists(coordsPath))
            {
                Tuple<double, double> latLngFromFile = Client.GetLatLngFromFile();
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

            Logger.Write($"Make sure Lat & Lng is right. Exit Program if not! Lat: {ctx.Client.CurrentLat} Lng: {ctx.Client.CurrentLng}", LogLevel.Warning);

            return new FarmState();
        }
    }
}
