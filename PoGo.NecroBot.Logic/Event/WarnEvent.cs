namespace PoGo.NecroBot.Logic.Event
{
    public class WarnEvent : IEvent
    {
        public string Message = "";

        /// <summary>
        ///     This event requires handler to perform input
        /// </summary>
        public bool RequireInput;

        public override string ToString()
        {
            return Message;
        }
    }
}