namespace PoGo.NecroBot.Logic.Event
{
    public class FortTargetEvent : IEvent
    {
        public double Distance;
        public string Name;
        public double Latitude;
        public double Longitude;
    }
}