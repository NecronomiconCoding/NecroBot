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

        private static RarePokemonInfo Map(PokesniperWrap.PokesniperItem result)
        {
            long epochTicks = new DateTime(1970, 1, 1).Ticks;
            var unixBase = new DateTime(1970, 1, 1);
            long unixTime = ((result.until.AddMinutes(-15).Ticks - epochTicks) / TimeSpan.TicksPerSecond);
            //double ticks = Math.Truncate((result.expires_at.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
            //unixTime = result.expires_at.AddMinutes(-15) - 
            var arr = result.coords.Split(',');
            return new RarePokemonInfo()
            {
                latitude = Convert.ToDouble(arr[0]),
                longitude = Convert.ToDouble(arr[1]),
                pokemonId = (int)Enum.Parse(typeof(PokemonId), result.name),
                created = unixTime
            };
        }
        private static async Task<List<RarePokemonInfo>> FetchFromPokesnipers(double lat, double lng)
        {
            List<RarePokemonInfo> results = new List<RarePokemonInfo>();
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
                    catch (Exception ex)
                    {
                        //ignore if any data failed.
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write("Error loading data", LogLevel.Error, ConsoleColor.DarkRed);
            }
            return results;
        }




    }
}
