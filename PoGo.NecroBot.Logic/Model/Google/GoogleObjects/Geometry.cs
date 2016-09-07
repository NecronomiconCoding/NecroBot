namespace PoGo.NecroBot.Logic.Model.Google.GoogleObjects
{
    public class Geometry
    {
        public Geo location { get; set; }
        public string location_type { get; set; }
        public Bounds viewport { get; set; }
    }
}