using Caching;
using GeoCoordinatePortable;
using Newtonsoft.Json;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace PoGo.NecroBot.Logic.Service
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

    public class MapQuestElevationService
    {
        ISession _session;
        LRUCache<string, double> cache = new LRUCache<string, double>(capacity: 500);
        private string apiKey = $"Kmjtd|luua2qu7n9,7a=o5-lzbgq";

        public MapQuestElevationService(ISession session)
        {
            _session = session;
        }

        private string GetRawLatLngCollection(IEnumerable<GeoCoordinate> positions)
        {
            string rawLatLng = "";
            foreach (GeoCoordinate position in positions)
            {
                rawLatLng += $"{position.Latitude},{position.Longitude}";
            }
            return rawLatLng;
        }
        
        private string GetCacheKey(double lat, double lng)
        {
            return Math.Round(lat, 3) + "," + Math.Round(lng, 3);
        }

        private string GetCacheKey(GeoCoordinate position)
        {
            return GetCacheKey(position.Latitude, position.Longitude);
        }

        public double GetAltitude(double lat, double lng)
        {
            string cacheKey = GetCacheKey(lat, lng);
            double altitude;
            bool success = cache.TryGetValue(cacheKey, out altitude);
            if (!success)
            {
                altitude = GetAltitudeFromMapQuest(lat, lng);
                if (altitude == 0)
                {
                    // Error getting altitude so just return 0.
                    return 0;
                }
                else
                {
                    cache.Add(cacheKey, altitude);
                }
            }

            // Always return a slightly random altitude.
            return GetRandomAltitude(altitude);
        }

        public void UpdateAltitude(ref GeoCoordinate position)
        {
            double altitude = GetAltitude(position.Latitude, position.Longitude);
            // Only update the position altitude if we got a non-zero altitude.
            if (altitude != 0)
            {
                position.Altitude = altitude;
            }
        }
        
        private double GetRandomAltitude(double altitude)
        {
            return altitude + (new Random().NextDouble() * 5);
        }
                
        private double GetAltitudeFromMapQuest(double lat, double lng)
        {
            try
            {
                string url = $"https://open.mapquestapi.com/elevation/v1/profile?key={apiKey}&callback=handleHelloWorldResponse&shapeFormat=raw&latLngCollection={lat},{lng}";
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
