#region

using System.Net.Http;
using System.Threading.Tasks;
using Google.Protobuf;
using PokemonGo.RocketAPI.Exceptions;
using PokemonGo.RocketAPI.GeneratedCode;
using System.Linq;
using System.Collections.Generic;

#endregion

namespace PokemonGo.RocketAPI.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<Response> PostProto<TRequest>(this HttpClient client, string url, TRequest request)
            where TRequest : IMessage<TRequest>
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

        public static async Task<TResponsePayload> PostProtoPayload<TRequest, TResponsePayload>(this HttpClient client,
            string url, TRequest request) where TRequest : IMessage<TRequest>
            where TResponsePayload : IMessage<TResponsePayload>, new()
        {
            Logger.Write($"Requesting {typeof(TResponsePayload).Name}", LogLevel.Debug);
            var response = await PostProto(client, url, request);

            if (response.Payload.Count == 0)
                throw new InvalidResponseException();

            //Decode payload
            var payload = response.Payload[0];
            var parsedPayload = new TResponsePayload();
            parsedPayload.MergeFrom(payload);

            return parsedPayload;
        }

        public static async Task<IEnumerable<TResponsePayload>> PostProtoPayloadExpectMultiple<TRequest, TResponsePayload>(this HttpClient client,
            string url, TRequest request) where TRequest : IMessage<TRequest>
            where TResponsePayload : IMessage<TResponsePayload>, new()
        {
            Logger.Write($"Requesting multiple {typeof(TResponsePayload).Name}", LogLevel.Debug);
            var response = await PostProto(client, url, request);

            if (response.Payload.Count == 0)
                throw new InvalidResponseException();

            //Decode payload
            return response.Payload.Select(sb =>
            {
                TResponsePayload responsePayload = new TResponsePayload(); //this is slowish. Compiler emits IL for Activator.CreateNew on generic new() constraint. Consider compiled lambda in the future.
                responsePayload.MergeFrom(sb);

                return responsePayload;
            });
        }
    }
}