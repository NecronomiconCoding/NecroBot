namespace PoGo.NecroBot.Logic.Event
{
    public class EggIncubatorStatusEvent : IEvent
    {
        public string IncubatorId;
        public bool WasAddedNow;
        public ulong PokemonId;
        public double KmToWalk;
        public double KmRemaining;
        public double KmWalked => KmToWalk - KmRemaining;
    }
}