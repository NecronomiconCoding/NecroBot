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
				.WithRequestID() //RPC ID?
				.WithAuthenticationMessage(authType, authToken); 

			//Now we add the requests to the envelope
			foreach (Request r in requestMessages)
				envelope.WithMessage(r);

			return envelope;
        }

        public static RequestEnvelope GetRequestEnvelope(AuthTicket authTicket, double lat, double lng, double altitude)
        {
			RequestEnvelope envelope = new RequestEnvelope();

			//These requests are sent with our issued AuthTicket
			envelope.WithAltitude(altitude)
				.WithLatitude(lat)
				.WithLongitude(lng)
				.WithRequestID() //RPC ID?
				.WithAuthTicket(authTicket);

			return envelope;
        }
    }
}