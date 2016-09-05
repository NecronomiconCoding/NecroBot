namespace PoGo.NecroBot.Logic.Model.Google.GoogleObjects
{
    public class DistanceMatrixResponse
    {
        public string[] destination_addresses { get; set; }
        public string[] origin_addresses { get; set; }
        public Row[] rows { get; set; }
        public string status { get; set; }

        
    }
}