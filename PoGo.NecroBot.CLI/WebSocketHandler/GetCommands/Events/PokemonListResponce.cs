#region using directives

using PoGo.NecroBot.Logic.Event;

#endregion

namespace PoGo.NecroBot.CLI.WebSocketHandler.GetCommands.Events
{
    public class PokemonListResponce : IWebSocketResponce, IEvent
    {
        public PokemonListResponce(dynamic data, string requestID)
        {
            Command = "PokemonListWeb";
            Data = data;
            RequestID = requestID;
        }

        public string RequestID { get; }
        public string Command { get; }
        public dynamic Data { get; }
    }
}