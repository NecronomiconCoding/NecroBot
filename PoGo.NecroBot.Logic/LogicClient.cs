namespace PoGo.NecroBot.Logic
{
    public class LogicClient
    {
        public ILogicSettings Settings;

        public LogicClient(ILogicSettings settings)
        {
            Settings = settings;
        }
        public static int variation(string a = null)
        {
            System.Random ran = new System.Random(System.Guid.NewGuid().GetHashCode());
            switch (a)
            {
                case "xsm":
                    return ran.Next(2, 10);
                case "sml":
                    return ran.Next(112, 625);
                case "med":
                    return ran.Next(1221, 2625);
                case "lrg":
                    return ran.Next(4112, 8125);
                default:
                    int min = int.Parse(a.Split(',')[0]);
                    int max = int.Parse(a.Split(',')[1]);
                    return ran.Next(min, max);
            }
        }
    }
}