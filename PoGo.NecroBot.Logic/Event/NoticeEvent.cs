namespace PoGo.NecroBot.Logic.Event
{
    public class NoticeEvent : IEvent
    {
        public string Message = "";

        public override string ToString() => Message;
    }
}