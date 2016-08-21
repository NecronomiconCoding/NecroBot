namespace PoGo.NecroBot.Logic.Model.Settings
{
    public class LocationConfig
    {
        public bool DisableHumanWalking;
        public bool StartFromLastPosition = true;
        public double DefaultLatitude = 40.785092;
        public double DefaultLongitude = -73.968286;
        public double WalkingSpeedInKilometerPerHour = 4.16;
        public bool UseWalkingSpeedVariant = true;
        public double WalkingSpeedVariant = 1.2;
        public bool ShowVariantWalking = false;
        public bool RandomlyPauseAtStops = true;
        public int MaxSpawnLocationOffset = 10;
        public int MaxTravelDistanceInMeters = 1000;
    }
}