namespace PoGo.NecroBot.Logic.Event
{
    public class WarnEvent : IEvent
    {
        public bool CanBeDelayed;
        public string Message = "";

        public override string ToString()
        {
            return Message;
        }
    }
}