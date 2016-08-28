using Newtonsoft.Json;
using PoGo.NecroBot.Logic.Logging;
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
        public class PokeradarWrapper
        {
            //should refactore this model - SnipeInfo
            public List<RarePokemonInfo> data { get; set; }
        }

        private static async Task<List<RarePokemonInfo>> FetchFromPokeradar(double lat, double lng)
        {
            List<RarePokemonInfo> results = new List<RarePokemonInfo>();
            if (!_setting.HumanWalkingSnipeUsePokeRadar) return results;
            try
            {

                HttpClient client = new HttpClient();
                double offset = _setting.HumanWalkingSnipeSnipingScanOffset; //0.015 
                string url = $"https://www.pokeradar.io/api/v1/submissions?deviceId=1fd29370661111e6b850a13a2bdc4ebf&minLatitude={lat - offset}&maxLatitude={lat + offset}&minLongitude={lng - offset}&maxLongitude={lng + offset}&pokemonId=0";

                var task = await client.GetStringAsync(url);

                var data = JsonConvert.DeserializeObject<PokeradarWrapper>(task);
                results = data.data;
            }
            catch (Exception ex)
            {
                Logger.Write("Error loading data", LogLevel.Error, ConsoleColor.DarkRed);
            }
            return results;
        }

    }
}
