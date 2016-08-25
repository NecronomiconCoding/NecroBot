using Caching;
using GeoCoordinatePortable;
using Newtonsoft.Json;
using PoGo.NecroBot.Logic.State;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace PoGo.NecroBot.Logic.Service.Elevation
{
    public class MapQuestResponse
    {
        public List<ElevationProfiles> elevationProfile { get; set; }
    }

    public class ElevationProfiles
    {
        public double distance { get; set; }
        public double height { get; set; }
    }

    public class MapQuestElevationService : BaseElevationService
    {
        private string mapQuestDemoApiKey = $"Kmjtd|luua2qu7n9,7a=o5-lzbgq";

        public MapQuestElevationService(ISession session, LRUCache<string, double> cache) : base(session, cache)
        {
            _apiKey = mapQuestDemoApiKey;
        }

        public override double GetElevationFromWebService(double lat, double lng)
        {
            try
            {
                string url = $"https://open.mapquestapi.com/elevation/v1/profile?key={_apiKey}&callback=handleHelloWorldResponse&shapeFormat=raw&latLngCollection={lat},{lng}";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Credentials = CredentialCache.DefaultCredentials;
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
                request.ContentType = "application/json";
                request.Referer = "https://open.mapquestapi.com/elevation/";
                request.ReadWriteTimeout = 2000;
                string responseFromServer = "";

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        responseFromServer = reader.ReadToEnd();
                        responseFromServer = responseFromServer.Replace("handleHelloWorldResponse(", "");
                        responseFromServer = responseFromServer.Replace("]}});", "]}}");
                        MapQuestResponse mapQuestResponse = JsonConvert.DeserializeObject<MapQuestResponse>(responseFromServer);
                        if (mapQuestResponse.elevationProfile != null && 0 < mapQuestResponse.elevationProfile.Count)
                        {
                            return mapQuestResponse.elevationProfile[0].height;
                        }
                    }
                }
            }
            catch(Exception)
            {
                // If we get here for any reason, then just drop down and return 0.
            }

            return 0;
        }
    }
}
