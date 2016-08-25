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
    public class GoogleResponse
    {
        public List<GoogleElevationResults> results { get; set; }
    }

    public class GoogleElevationResults
    {
        public double elevation { get; set; }
        public double resolution { get; set; }
        public GoogleLocation location { get; set; }
    }

    public class GoogleLocation
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class GoogleElevationService : BaseElevationService
    {
        public GoogleElevationService(ISession session, LRUCache<string, double> cache) : base(session, cache)
        {
            _apiKey = session.LogicSettings.GoogleApiKey;
        }
                        
        public override double GetElevationFromWebService(double lat, double lng)
        {
            try
            {
                string url = $"https://maps.googleapis.com/maps/api/elevation/json?key={_apiKey}&locations={lat},{lng}";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Credentials = CredentialCache.DefaultCredentials;
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
                request.ContentType = "application/json";
                request.ReadWriteTimeout = 2000;
                string responseFromServer = "";

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        responseFromServer = reader.ReadToEnd();
                        GoogleResponse googleResponse = JsonConvert.DeserializeObject<GoogleResponse>(responseFromServer);
                        if (googleResponse.results != null && 0 < googleResponse.results.Count)
                        {
                            return googleResponse.results[0].elevation;
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
