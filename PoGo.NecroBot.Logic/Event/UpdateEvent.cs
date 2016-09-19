namespace PoGo.NecroBot.Logic.Event
{
    public class UpdateEvent : IEvent
    {
        public string Message = "";

        public override string ToString()
        {
            return Message;
        }
    }
}