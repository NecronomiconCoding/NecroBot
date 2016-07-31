using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Tasks
{
    public class InventoryListTask
    {
        public static async Task Execute(ISession session)
        {
            var inventory = await session.Inventory.GetItems();

            session.EventDispatcher.Send(
                new InventoryListEvent
                {
                    Items = inventory.ToList()
                });
        }
    }
}
