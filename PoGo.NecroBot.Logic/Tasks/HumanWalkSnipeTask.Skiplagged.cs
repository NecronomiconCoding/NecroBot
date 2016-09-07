using GeoCoordinatePortable;
using Newtonsoft.Json;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Interfaces.Configuration;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.Model.Settings;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using POGOProtos.Enums;
using POGOProtos.Inventory.Item;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using WebSocket4Net;

namespace PoGo.NecroBot.Logic.Tasks
{
    public class SkiplaggedItem
    {
        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public DateTime expires_date
        {
            get
            {
                return UnixTimeStampToDateTime(expires);
            }
        }

        public double expires { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int pokemon_id { get; set; }
        public string pokemon_name { get; set; }
    }
    public class SkiplaggedWrap
    {
        public double duration { get; set; }
        public List<SkiplaggedItem> pokemons { get; set; }
        public SkiplaggedWrap()
        {
            pokemons = new List<SkiplaggedItem>();
        }
    }
    //need refactor this class, move list snipping pokemon to session and split function out to smaller class.
    public partial class HumanWalkSnipeTask
    {
        private static async Task<List<SnipePokemonInfo>> FetchFromSkiplagged(double lat, double lng)
        {
            List<SnipePokemonInfo> results = new List<SnipePokemonInfo>();
            if (!_setting.HumanWalkingSnipeUseSkiplagged) return results;

            var lat1 = lat - _setting.HumanWalkingSnipeSnipingScanOffset;
            var lat2 = lat + _setting.HumanWalkingSnipeSnipingScanOffset;
            var lng1 = lng - _setting.HumanWalkingSnipeSnipingScanOffset;
            var lng2 = lng + _setting.HumanWalkingSnipeSnipingScanOffset;

            string url = $"https://skiplagged.com/api/pokemon.php?bounds={lat1},{lng1},{lat2},{lng2}";

            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.TryParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip, deflate, sdch, br");
                client.DefaultRequestHeaders.Host = "skiplagged.com";
                client.DefaultRequestHeaders.UserAgent.TryParseAdd("Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36");

                var json = await client.GetStringAsync(url);

                results = GetJsonList(json);
            }
            catch (Exception )
            {
                Logger.Write("Error loading data from skiplagged", LogLevel.Error, ConsoleColor.DarkRed);
            }
            return results;
        }

        private static List<SnipePokemonInfo> GetJsonList(string reader)
        {
            var wrapper = JsonConvert.DeserializeObject<SkiplaggedWrap>(reader);
            var list = new List<SnipePokemonInfo>();
            foreach (var result in wrapper.pokemons)
            {
                var sniperInfo = Map(result);
                if (sniperInfo != null)
                {
                    list.Add(sniperInfo);
                }
            }
            return list;
        }
        private static SnipePokemonInfo Map(SkiplaggedItem result)
        {
            return new SnipePokemonInfo()
            {
                Latitude = result.latitude,
                Longitude = result.longitude,
                Id = result.pokemon_id,
                ExpiredTime = UnixTimeStampToDateTime(result.expires) ,
                Source = "Skiplagged"
            };
        }

    }

}
