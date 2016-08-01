using PoGo.NecroBot.CLI.WebSocketHandler.BasicGetCommands.Events;
using PoGo.NecroBot.CLI.WebSocketHandler.BasicGetCommands.Helpers;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Inventory.Item;
using SuperSocket.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.CLI.WebSocketHandler.BasicGetCommands.Tasks
{
    class EvolvePokemonTask
    {
        public static async Task Execute(ISession session, WebSocketSession webSocketSession, ulong pokemonId, string requestID)
        {
            await Logic.Tasks.EvolveSpecificPokemonTask.Execute(session, pokemonId);
        }
    }
}
