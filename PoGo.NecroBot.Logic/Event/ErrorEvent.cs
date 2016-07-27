namespace PoGo.NecroBot.Logic.Event
{
    public class ErrorEvent : IEvent
    {
        public string Message = "";

        public override string ToString() => Message;
    }
}