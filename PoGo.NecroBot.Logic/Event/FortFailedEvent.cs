namespace PoGo.NecroBot.Logic.Event
{
    public class FortFailedEvent : IEvent
    {
        public int Max;
        public string Name;
        public int Try;
    }
}