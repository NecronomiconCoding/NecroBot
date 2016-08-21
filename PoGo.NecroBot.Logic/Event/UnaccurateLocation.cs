namespace PoGo.NecroBot.Logic.Event
{
    public class UnaccurateLocation : IEvent
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}