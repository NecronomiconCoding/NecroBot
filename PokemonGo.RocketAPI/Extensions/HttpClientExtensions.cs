#region

using System.Net.Http;
using System.Threading.Tasks;
using Google.Protobuf;
using PokemonGo.RocketAPI.Exceptions;
using System.Linq;
using System.Collections.Generic;
using PokemonGoDesktop.API.Proto;

#endregion

namespace PokemonGo.RocketAPI.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<ResponseEnvelope> PostProto(this HttpClient client, string url, IMessage request)
        {
            //Encode payload and put in envelop, then send
            var data = request.ToByteString();
            var result = await client.PostAsync(url, new ByteArrayContent(data.ToByteArray()));

            //Decode message
            var responseData = await result.Content.ReadAsByteArrayAsync();
            var codedStream = new CodedInputStream(responseData);
            var decodedResponse = new ResponseEnvelope();
            decodedResponse.MergeFrom(codedStream);

            return decodedResponse;
        }

        public static async Task<TResponsePayload> PostProtoPayload<TResponsePayload>(this HttpClient client, string url, IMessage request)
            where TResponsePayload : IResponseMessage, IMessage<TResponsePayload>, new()
        {
            Logger.Write($"Requesting {typeof(TResponsePayload).Name}", LogLevel.Debug);
            var response = await PostProto(client, url, request);

            if (response.Returns.Count == 0)
                throw new InvalidResponseException();

            //Decode payload
            var payload = response.Returns.First();
            var parsedPayload = new TResponsePayload();
            parsedPayload.MergeFrom(payload);

            return parsedPayload;
        }

        public static async Task<IEnumerable<TResponsePayload>> PostProtoPayloadExpectMultiple<TResponsePayload>(this HttpClient client, string url, IMessage request)
            where TResponsePayload : IResponseMessage, IMessage<TResponsePayload>, new()
        {
            Logger.Write($"Requesting multiple {typeof(TResponsePayload).Name}", LogLevel.Debug);
            var response = await PostProto(client, url, request);

            if (response.Returns.Count == 0)
                throw new InvalidResponseException();

            //Decode payload
            return response.Returns.Select(sb =>
            {
                TResponsePayload responsePayload = new TResponsePayload(); //this is slowish. Compiler emits IL for Activator.CreateNew on generic new() constraint. Consider compiled lambda in the future.
                responsePayload.MergeFrom(sb);

                return responsePayload;
            });
        }
    }
}