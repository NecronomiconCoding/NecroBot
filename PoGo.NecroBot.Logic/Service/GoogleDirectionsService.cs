using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using GeoCoordinatePortable;
using Newtonsoft.Json;
using PoGo.NecroBot.Logic.Model.Google;
using PoGo.NecroBot.Logic.Model.Google.GoogleObjects;
using PoGo.NecroBot.Logic.State;

namespace PoGo.NecroBot.Logic.Service
{
    public class GoogleDirectionsService
    {
        private readonly ISession _session;
        private readonly bool _cache;
        public List<GoogleResult> OldResults { get; set; }
        public GoogleDirectionsService(ISession session)
        {
            _session = session;
            _cache = _session.LogicSettings.UseGoogleWalkCache;
            OldResults = new List<GoogleResult>();
        }

        public GoogleResult GetDirections(GeoCoordinate origin, List<GeoCoordinate> waypoints, GeoCoordinate destino)
        {
            if (_cache)
            {
                var item = OldResults.FirstOrDefault(pesquisa => IsSameAdress(origin, waypoints, destino, pesquisa));
                if (item != null)
                {
                    item.FromCache = true;
                    return item;
                }
            }
            var url = GetUrl(origin, waypoints, destino);

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage responseMessage = client.GetAsync(url).Result;
                    var resposta = responseMessage.Content.ReadAsStringAsync();
                    var google = JsonConvert.DeserializeObject<DirectionsResponse>(resposta.Result);
                    if (google.status.Equals("OVER_QUERY_LIMIT"))
                    {
                        // If we get an error, don't cache empty GoogleResult.  Just return null.
                        return null;
                    }

                    var resultadoPesquisa = new GoogleResult
                    {
                        Directions = google,
                        RequestDate = DateTime.Now,
                        Origin = origin,
                        Waypoints = waypoints,
                        Destiny = destino,
                        FromCache = false,
                    };

                    if (_cache)
                        SaveResult(resultadoPesquisa);

                    return resultadoPesquisa;
                }
                catch(Exception)
                {
                    return null;
                }
            }
        }

        private static bool IsSameAdress(GeoCoordinate origem, List<GeoCoordinate> waypoints, GeoCoordinate destino, GoogleResult googleSearch)
        {
            var sameAdress = origem.GetDistanceTo(googleSearch.Origin) < 10 && destino.GetDistanceTo(googleSearch.Destiny) < 10;

            var sameQuantityWaypoint = waypoints.Count == googleSearch.Waypoints.Count;

            if (!sameQuantityWaypoint) return sameAdress;
            if (!sameAdress) return sameAdress;

            for (int i = 0; i < waypoints.Count; i++)
            {
                sameAdress = googleSearch.Waypoints[i].Equals(waypoints[i]);

                if (!sameAdress)
                    break;
            }
            return sameAdress;
        }

        private string GetUrl(GeoCoordinate origem, List<GeoCoordinate> pontosDeParada, GeoCoordinate destino)
        {
            var url = $"directions/json?origin={origem.Latitude.ToString("R").Replace(",", ".")},{origem.Longitude.ToString("R").Replace(",", ".")}&destination={destino.Latitude.ToString("R").Replace(",", ".")},{destino.Longitude.ToString("R").Replace(",", ".")}&sensor=false";
            var waypoint = "&waypoints=optimize:false|";
            var possuiWayPoint = pontosDeParada.Any();
            for (var i = 0; i < pontosDeParada.Count; i++)
            {
                waypoint += $"{pontosDeParada[i].Latitude.ToString("R").Replace(",", ".")},{pontosDeParada[i].Longitude.ToString("R").Replace(",", ".")}|";
            }
            if (possuiWayPoint)
                url += waypoint.Substring(0, waypoint.Length - 1);

            if (!string.IsNullOrEmpty(_session.LogicSettings.GoogleApiKey))
                url += $"&key={_session.LogicSettings.GoogleApiKey}";

            if (!string.IsNullOrEmpty(_session.LogicSettings.GoogleHeuristic))
                url += $"&mode={_session.LogicSettings.GoogleHeuristic}";

            return url;
        }

        private void SaveResult(GoogleResult googleResult)
        {
            var item = OldResults.FirstOrDefault(pesquisa => IsSameAdress(googleResult.Origin, googleResult.Waypoints, googleResult.Destiny, pesquisa));
            if (item != null)
            {
                lock (OldResults)
                    OldResults.Remove(item);
            }

            OldResults.Add(googleResult);

        }

    }






}
