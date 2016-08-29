namespace PoGo.NecroBot.CLI.WebSocketHandler.GetCommands.Events
{
    public class ConfigResponce : IWebSocketResponce
    {
        public ConfigResponce(dynamic data, string requestID)
        {
            Command = "ConfigWeb";
            Data = data;
            RequestID = requestID;
        }
        public string RequestID { get; private set; }
        public string Command { get; private set; }
        public dynamic Data { get; private set; }
        
    }
}
