using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PokemonGo.RocketAPI.Helpers
{
    public class JsonHelper
    {
        public static string GetValue(string json, string key)
        {
            var jObject = JObject.Parse(json);
            return jObject[key].ToString();
        }
    }
}
