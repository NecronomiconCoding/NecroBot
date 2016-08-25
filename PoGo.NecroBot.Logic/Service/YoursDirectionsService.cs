using GeoCoordinatePortable;
using PoGo.NecroBot.Logic.State;
using System;
using System.IO;
using System.Net;

namespace PoGo.NecroBot.Logic.Service
{
    class YoursDirectionsService
    {
        private readonly ISession _session;
        public YoursDirectionsService(ISession session)
        {
            _session = session;
        }

        public string GetDirections(GeoCoordinate sourceLocation, GeoCoordinate destLocation)
        {
            WebRequest request = WebRequest.Create(GetUrl(sourceLocation, destLocation));
            request.Credentials = CredentialCache.DefaultCredentials;

            string responseFromServer = "";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        responseFromServer = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {
                responseFromServer = "";
            }

            return responseFromServer;
        }

        private string GetUrl(GeoCoordinate sourceLocation, GeoCoordinate destLocation)
        {
            string url = $"http://www.yournavigation.org/api/dev/route.php?format=geojson&flat={sourceLocation.Latitude}&flon={sourceLocation.Longitude}&tlat={destLocation.Latitude}&tlon={destLocation.Longitude}&fast=1&layer=mapnik";
            
            if (!string.IsNullOrEmpty(_session.LogicSettings.YoursWalkHeuristic))
                url += $"&v={_session.LogicSettings.YoursWalkHeuristic}";
            else
                url += $"&v=bicycle";

            return url;
        }
    }
}
