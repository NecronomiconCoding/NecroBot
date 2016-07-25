using POGOProtos.Inventory.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Logic.Event
{
    public class ItemRecycledEvent : IEvent
    {
        public ItemId Id;
        public int Count;
    }
}
