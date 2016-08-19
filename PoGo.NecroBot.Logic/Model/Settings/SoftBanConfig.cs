namespace PoGo.NecroBot.Logic.Model.Settings
{
    public class SoftBanConfig
    {
        public bool FastSoftBanBypass;
        public bool UseKillSwitchCatch = true;
        public int CatchErrorPerHours = 40;
        public int CatchEscapePerHours = 40;
        public int CatchFleePerHours = 40;
        public int CatchMissedPerHours = 40;
        public int CatchSuccessPerHours = 40;
        public bool UseKillSwitchPokestops = true;
        public int AmountPokestops = 80;
    }
}