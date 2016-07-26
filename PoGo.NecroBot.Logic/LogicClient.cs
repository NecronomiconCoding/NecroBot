namespace PoGo.NecroBot.Logic
{
    public class LogicClient
    {
        public ILogicSettings Settings;

        public LogicClient(ILogicSettings settings)
        {
            Settings = settings;
        }
    }
}