namespace PoGo.NecroBot.Logic.Event
{
    public class FortFailedEvent : IEvent
    {
        public string Name;
        public int Try;
        public int Max;
    }
}