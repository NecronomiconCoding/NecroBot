using System.Collections.Generic;
using GeoCoordinatePortable;
using PoGo.NecroBot.Logic.Service;
using PoGo.NecroBot.Logic.State;

namespace PoGo.NecroBot.Logic.Model.Google
{
    public static class Teste
    {
        public static void Testar(ISession session)
        {
            var googleDirectionsService = new DirectionsService(session);

            var googleResult = googleDirectionsService.GetDirections(new GeoCoordinate(40.780599, -73.968862), new List<GeoCoordinate>(), new GeoCoordinate(40.781939, -73.965123));
            var googleWalk = GoogleWalk.Get(googleResult);

            var proximo = googleWalk.NextStep(new GeoCoordinate(40.780573, -73.968842));
        }
    }
}
