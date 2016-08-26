namespace PoGo.NecroBot.CLI.WebSocketHandler.GetCommands.Events
{
    class SnipeListResponce : IWebSocketResponce
    {
        public SnipeListResponce(dynamic data,string requestID)
        {
            Command = "PokemonSnipeList";
            Data = data;
            RequestID = requestID;
        }
        public string RequestID { get; private set; }
        public string Command { get; private set; }
        public dynamic Data { get; private set; }
    }
}
