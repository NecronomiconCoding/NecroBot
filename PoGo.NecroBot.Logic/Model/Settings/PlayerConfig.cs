namespace PoGo.NecroBot.Logic.Model.Settings
{
    public class PlayerConfig
    {
        public int DelayBetweenPlayerActions = 5000;
        public int MinEvolveActionDelay = 22500;
        public int MaxEvolveActionDelay = 25500;
        public bool UseNearActionRandom = true;
        public bool AutoCompleteTutorial = false;
        public string DesiredNickname;
        public string DesiredGender = "Male";
        public string DesiredStarter = "Squirtle";
    }
}