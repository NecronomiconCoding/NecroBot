namespace PoGo.NecroBot.Logic.Model.Settings
{
    public class SnipeConfig
    {
        public bool UseSnipeLocationServer;
        public string SnipeLocationServer = "localhost";
        public int SnipeLocationServerPort = 16969;
        public bool GetSniperInfoFromPokezz;
        public bool GetOnlyVerifiedSniperInfoFromPokezz = true;
        public bool GetSniperInfoFromPokeSnipers = true;
        public bool GetSniperInfoFromPokeWatchers = true;
        public bool GetSniperInfoFromSkiplagged = true;
        public int MinPokeballsToSnipe = 20;
        public int MinPokeballsWhileSnipe = 0;
        public int MinDelayBetweenSnipes = 60000;
        public double SnipingScanOffset = 0.005;
        public bool SnipeAtPokestops;
        public bool SnipeIgnoreUnknownIv;
        public bool UseTransferIvForSnipe;
        public bool SnipePokemonNotInPokedex;
        /*SnipeLimit*/
        public bool UseSnipeLimit = true;
        public int SnipeRestSeconds = 10 * 60;
        public int SnipeCountLimit = 39;
    }
}