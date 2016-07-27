namespace PoGo.NecroBot.Logic.Event
{
    public class FortFailedEvent : IEvent
    {
        public int Retry;
        public int Max;
    }
}