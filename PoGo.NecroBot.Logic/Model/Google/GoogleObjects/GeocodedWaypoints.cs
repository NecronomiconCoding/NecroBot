namespace PoGo.NecroBot.Logic.Model.Google.GoogleObjects
{
    public class GeocodedWaypoints
    {
        public string geocoder_status { get; set; }
        public string place_id { get; set; }
        public string[] types { get; set; }
    }
}