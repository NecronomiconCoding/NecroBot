using PoGo.NecroBot.CLI.WebSocketHandler.BasicGetCommands.Events;
using PoGo.NecroBot.Logic.State;
using SuperSocket.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.CLI.WebSocketHandler.BasicGetCommands.Tasks
{
    class GetTrainerProfileTask
    {
        public static async Task Execute(ISession session, WebSocketSession webSocketSession, string requestID)
        {
            webSocketSession.Send(EncodingHelper.Serialize(new TrainerProfileResponce(session.Profile, requestID)));
            await Task.Delay(500);
        }
    }
}
