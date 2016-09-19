using Newtonsoft.Json;
using PoGo.NecroBot.Logic.Logging;
using POGOProtos.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Tasks
{
    public partial class HumanWalkSnipeTask
    {
        public class PokesniperWrap
        {
            public class PokesniperItem
            {
                public string coords { get; set; }
                public string name { get; set; }
                public DateTime until { get; set; }
            }
            public List<PokesniperItem> results { get; set; }
        }

        private static SnipePokemonInfo Map(PokesniperWrap.PokesniperItem result)
        {
            long epochTicks = new DateTime(1970, 1, 1).Ticks;
            var unixBase = new DateTime(1970, 1, 1);
            long unixTime = ((result.until.AddMinutes(-15).Ticks - epochTicks) / TimeSpan.TicksPerSecond);
            //double ticks = Math.Truncate((result.expires_at.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
            //unixTime = result.expires_at.AddMinutes(-15) - 
            var arr = result.coords.Split(',');
            return new SnipePokemonInfo()
            {
                Latitude = Convert.ToDouble(arr[0]),
                Longitude = Convert.ToDouble(arr[1]),
                Id = (int)Enum.Parse(typeof(PokemonId), result.name),
                ExpiredTime = result.until.ToLocalTime()   ,
                Source = "Pokesnipers"
            };
        }
        private static async Task<List<SnipePokemonInfo>> FetchFromPokesnipers(double lat, double lng)
        {
            List<SnipePokemonInfo> results = new List<SnipePokemonInfo>();
            // if (!_setting.HumanWalkingSnipeUsePokeRadar) return results;
            try
            {
                HttpClient client = new HttpClient();
                double offset = _setting.HumanWalkingSnipeSnipingScanOffset; //0.015 
                string url = $"http://pokesnipers.com/api/v1/pokemon.json";

                var task = await client.GetStringAsync(url);

                var data = JsonConvert.DeserializeObject<PokesniperWrap>(task);
                foreach (var item in data.results)
                {
                    try
                    {
                        var pItem = Map(item);
                        if (pItem != null)
                        {
                            results.Add(pItem);
                        }
                    }
                    catch (Exception )
                    {
                        //ignore if any data failed.
                    }
                }
            }
            catch (Exception)
            {
                Logger.Write("Error loading data from pokesnipers", LogLevel.Error, ConsoleColor.DarkRed);
            }
            return results;
        }
    }
}
