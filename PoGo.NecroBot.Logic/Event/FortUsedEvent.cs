namespace PoGo.NecroBot.Logic.Event
{
    public class FortUsedEvent : IEvent
    {
        public string Name;
        public int Exp;
        public int Gems;
        public string Items;
        public double Latitude;
        public double Longitude;
    }
}