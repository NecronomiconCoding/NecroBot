#region using directives

using System.Collections.Generic;
using POGOProtos.Inventory;
using POGOProtos.Inventory.Item;

#endregion

namespace PoGo.NecroBot.Logic.Event
{
    public class InventoryListEvent : IEvent
    {
        public List<ItemData> Items;
    }
}