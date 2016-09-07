using POGOProtos.Inventory.Item;

namespace PoGo.NecroBot.Logic.Event
{
    public class UseBerryEvent : IEvent
    {
        public ItemId BerryType;
        public int Count;
    }
}