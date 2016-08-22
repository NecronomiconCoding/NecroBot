namespace PoGo.NecroBot.Logic.Model.Settings
{
    public class CustomCatchConfig
    {
        public bool EnableHumanizedThrows = true;
        public bool EnableMissedThrows = true;
        public int ThrowMissPercentage = 25;
        public int NiceThrowChance = 40;
        public int GreatThrowChance = 30;
        public int ExcellentThrowChance = 10;
        public int CurveThrowChance = 90;
        public double ForceGreatThrowOverIv = 90.00;
        public double ForceExcellentThrowOverIv = 95.00;
        public int ForceGreatThrowOverCp = 1000;
        public int ForceExcellentThrowOverCp = 1500;
    }
}