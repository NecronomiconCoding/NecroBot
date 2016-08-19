namespace PoGo.NecroBot.Logic.Model.Google.GoogleObjects
{
    public class Leg
    {
        public ValueText distance { get; set; }
        public ValueText duration { get; set; }
        public string end_address { get; set; }
        public Geo end_location { get; set; }
        public string start_address { get; set; }
        public Geo start_location { get; set; }
        public Step[] steps { get; set; }
        public object[] via_waypoint { get; set; }
    }
}