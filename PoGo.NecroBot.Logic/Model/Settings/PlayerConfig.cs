namespace PoGo.NecroBot.Logic.Model.Settings
{
    public class PlayerConfig
    {
        public int DelayBetweenPlayerActions = 10000;
        public int EvolveActionDelay = 30000;
        public int TransferActionDelay = 40000;
        public int RecycleActionDelay = 25000;
        public int RenamePokemonActionDelay = 60000;
        public bool UseNearActionRandom = true;
        public bool AutoCompleteTutorial = false;
        public string DesiredNickname;
        public string DesiredGender = "Male";
        public string DesiredStarter = "Squirtle";
    }
}