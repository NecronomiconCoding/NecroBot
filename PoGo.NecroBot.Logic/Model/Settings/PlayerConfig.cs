namespace PoGo.NecroBot.Logic.Model.Settings
{
    public class PlayerConfig
    {
        public int DelayBetweenPlayerActions = 10000;
        public int EvolveActionDelay = 30000;
        public int TransferActionDelay = 60000;
        public int RecycleActionDelay = 60000;
        public int RenamePokemonActionDelay = 180000;
        public bool UseNearActionRandom = true;
        public bool AutoCompleteTutorial = false;
        public string DesiredNickname;
        public string DesiredGender = "Male";
        public string DesiredStarter = "Squirtle";
    }
}