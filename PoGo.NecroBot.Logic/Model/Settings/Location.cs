using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Title = "Location", Description = "", ItemRequired = Required.DisallowNull)]
    public class Location
    {
        public Location()
        {
        }

        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        [Range(-90, 90)]
        [JsonProperty(Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, Order = 1)]
        public double Latitude { get; set; }

        [Range(-180, 180)]
        [JsonProperty(Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, Order = 2)]
        public double Longitude { get; set; }
    }
}
