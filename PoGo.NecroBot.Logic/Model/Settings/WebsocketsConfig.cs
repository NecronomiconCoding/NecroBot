using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Title = "Websockets Config", Description = "Set your websockets settings.", ItemRequired = Required.DisallowNull)]
    public class WebsocketsConfig
    {
        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 1)]
        public bool UseWebsocket = true;

        [DefaultValue(14251)]
        [Range(1, 65535)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 2)]
        public int WebSocketPort = 14251;
    }
}