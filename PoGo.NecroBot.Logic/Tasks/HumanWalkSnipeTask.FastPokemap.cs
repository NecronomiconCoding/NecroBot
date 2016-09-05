using Newtonsoft.Json;
using PoGo.NecroBot.Logic.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using POGOProtos.Enums;

namespace PoGo.NecroBot.Logic.Tasks
{
    public partial class HumanWalkSnipeTask
    {
        private static string ip;

        private static Task taskDataLive;
        public class FastPokemapItem
        {
            public class Lnglat
            {
                public string type { get; set; }
                public List<double> coordinates { get; set; }
            }

            public string _id { get; set; }
            public string pokemon_id { get; set; }
            public string encounter_id { get; set; }
            public string spawn_id { get; set; }
            public DateTime expireAt { get; set; }
            public int __v { get; set; }
            public Lnglat lnglat { get; set; }
        }

        private static string GetIP()
        {

            if (string.IsNullOrEmpty(ip))
            {
                var client = new HttpClient();
                var task = client.GetStringAsync("http://checkip.dyndns.org");
                task.Wait();
                ip = task.Result.Split(':')[1].Trim();
            }
            return ip;

        }
		private static void EnsureDataLive()
        {
            int liveUpdateCount = 6;

            if (taskDataLive != null && !taskDataLive.IsCompleted) return;
            taskDataLive = Task.Run(async  () =>  
            {
					while(liveUpdateCount>0)
                {
                    liveUpdateCount--;
                    var lat = _session.Client.CurrentLatitude;
                    var lng = _session.Client.CurrentLongitude;
                    var api = $"https://api.fastpokemap.se/?key=allow-all&ts=0&lat={lat}&lng={lng}";
                    await DownloadContent(api);
                    await Task.Delay(10000);
                }
            });
        }
        private static SnipePokemonInfo Map(FastPokemapItem result)
        {
            return new SnipePokemonInfo()
            {
                Latitude = result.lnglat.coordinates[1],
                Longitude = result.lnglat.coordinates[0],
                Id = GetId(result.pokemon_id),
                ExpiredTime = result.expireAt.ToLocalTime(),
                Source = "Fastpokemap"
            };
        }

        public static int GetId(string name)
        {

            var t = name[0];
            var realName = new StringBuilder(name.ToLower());
            realName[0] = t;
            try
            {
                var p = (PokemonId)Enum.Parse(typeof(PokemonId), realName.ToString());
                return (int)p;
            }
            catch (Exception)
            {

            }
            return 0;

        }
        private static async Task<string> DownloadContent(string url)
        {
            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get,
            };
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("origin", "https://fastpokemap.se");
            request.Headers.Add("authority", "cache.fastpokemap.se");

            string result = "";
            try {
                var task = await client.SendAsync(request);
                 result = await task.Content.ReadAsStringAsync();
            }
			catch(Exception ex) { }

            return result;

        }
        private static async Task<List<SnipePokemonInfo>> FetchFromFastPokemap(double lat, double lng)
        {
            List<SnipePokemonInfo> results = new List<SnipePokemonInfo>();
             if (!_setting.HumanWalkingSnipeUseFastPokemap) return results;
            try
            {
                EnsureDataLive();
                string url = $"https://cache.fastpokemap.se/?key=allow-all&ts=0&compute={GetIP()}&lat={lat}&lng={lng}";
                
                var json = await DownloadContent(url);
                var data = JsonConvert.DeserializeObject<List<FastPokemapItem>>(json);
                foreach (var item in data)
                {
                    var pItem = Map(item);
                    if (pItem != null && pItem.Id > 0)
                    {
                        results.Add(pItem);
                    }
                }

            }
            catch (Exception)
            {
                Logger.Write("Error loading data fastpokemap", LogLevel.Error, ConsoleColor.DarkRed);
            }
            return results;
        }

    }
}
