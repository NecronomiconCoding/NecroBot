namespace PoGo.NecroBot.CLI.WebSocketHandler.GetCommands.Events
{
    internal class TrainerProfileResponce : IWebSocketResponce
    {
        public TrainerProfileResponce(dynamic data, string requestID)
        {
            Command = "TrainerProfile";
            Data = data;
            RequestID = requestID;
        }

        public string RequestID { get; private set; }
        public string Command { get; private set; }
        public dynamic Data { get; private set; }
    }
}