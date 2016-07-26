using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using PokemonGo.RocketAPI;
using PokemonGo.RocketAPI.Exceptions;
using PokemonGo.RocketAPI.GeneratedCode;

namespace PokemonGo.RocketAPI.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<TResponsePayload> PostProtoPayload<TRequest, TResponsePayload>(this HttpClient client, string url, TRequest request) where TRequest : IMessage<TRequest> where TResponsePayload : IMessage<TResponsePayload>, new()
        {
            Logger.Write($"Requesting {typeof(TResponsePayload).Name}", LogLevel.Debug);
            var response = await PostProto<TRequest>(client, url, request);

            if (response.Payload.Count == 0)
                throw new InvalidResponseException();

            //Decode payload
            //todo: multi-payload support
            var payload = response.Payload[0];
            var parsedPayload = new TResponsePayload();
            parsedPayload.MergeFrom(payload);
            
            return parsedPayload;
        }

        public static async Task<Response> PostProto<TRequest>(this HttpClient client, string url, TRequest request) where TRequest : IMessage<TRequest>
        {
            //Encode payload and put in envelop, then send
            var data = request.ToByteString();
            var result = await client.PostAsync(url, new ByteArrayContent(data.ToByteArray()));

            //Decode message
            var responseData = await result.Content.ReadAsByteArrayAsync();
            var codedStream = new CodedInputStream(responseData);
            var decodedResponse = new Response();
            decodedResponse.MergeFrom(codedStream);
            
            return decodedResponse;
        }
    }
}
