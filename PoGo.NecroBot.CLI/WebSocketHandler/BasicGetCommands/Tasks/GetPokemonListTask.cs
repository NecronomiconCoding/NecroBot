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
    class PokemonExtendedListTask
    {
    
        public static async Task Execute(ISession session, WebSocketSession webSocketSession)
        {
            var allPokemonInBag = await session.Inventory.GetHighestsCp(1000);
            var list = new List<PokemonListWeb>();
            allPokemonInBag.ToList().ForEach(o => list.Add(new PokemonListWeb(o)));
            webSocketSession.Send(EncodingHelper.Serialize(new PokemonListExtendedResponce(list)));
            
            await Task.Delay(500);
        }

    }
}
