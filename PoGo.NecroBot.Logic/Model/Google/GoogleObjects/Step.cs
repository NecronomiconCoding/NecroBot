namespace PoGo.NecroBot.Logic.Model.Google.GoogleObjects
{
    public class Step
    {
        public ValueText distance { get; set; }
        public ValueText duration { get; set; }
        public Geo end_location { get; set; }
        public string html_instructions { get; set; }
        public Polyline polyline { get; set; }
        public Geo start_location { get; set; }
        public string travel_mode { get; set; }
        public string maneuver { get; set; }
    }
}