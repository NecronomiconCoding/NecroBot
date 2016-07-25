#region

using System.Linq;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.GeneratedCode;
using PokemonGoDesktop.API.Proto;
using PokemonGoDesktop.API.Proto.Services;
using PokemonGoDesktop.API.Common;

#endregion

namespace PokemonGo.RocketAPI.Helpers
{
    public static class RequestEnvelopeBuilder
    {
        public static RequestEnvelope GetInitialRequestEnvelope(string authToken, AuthType authType, double lat, double lng,
            double altitude, params Request[] requestMessages)
        {
			RequestEnvelope envelope = new RequestEnvelope();

			//Provide the auth type and the oAuth token issued
			envelope.WithAltitude(altitude)
				.WithLatitude(lat)
				.WithLongitude(lng)
				.WithAuthenticationMessage(authType, authToken); //RPC ID?

			//Now we add the requests to the envelope
			foreach (Request r in requestMessages)
				envelope.WithMessage(r);

			return envelope;
        }

        public static RequestEnvelope GetInitialRequestEnvelope(string authToken, AuthType authType, double lat, double lng,
            double altitude, params RequestType[] customRequestTypes)
        {
            var customRequests = customRequestTypes.ToList().Select(c => new Request.Types.Requests {Type = (int) c});
            return GetInitialRequest(authToken, authType, lat, lng, altitude, customRequests.ToArray());
        }

        public static RequestEnvelope GetRequestEnvelope(AuthTicket authTicket, double lat, double lng, double altitude)
        {
            return new Request
            {
                Altitude = Utils.FloatAsUlong(altitude),
                authTicket = authTicket,
                Latitude = Utils.FloatAsUlong(lat),
                Longitude = Utils.FloatAsUlong(lng),
                RpcId = 1469378659230941192,
                Unknown1 = 2,
                Unknown12 = 989, //Required otherwise we receive incompatible protocol
                Requests =
                {
                    customRequests
                }
            };
        }
    }
}