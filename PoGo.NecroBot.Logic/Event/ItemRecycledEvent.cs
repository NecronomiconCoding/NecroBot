using POGOProtos.Inventory.Item;

namespace PoGo.NecroBot.Logic.Event
{
    public class ItemRecycledEvent : IEvent
    {
        public ItemId Id;
        public int Count;
    }
}
