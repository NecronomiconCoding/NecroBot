using POGOProtos.Enums;

namespace PoGo.NecroBot.Logic.Event
{
    public class TransferPokemonEvent : IEvent
    {
        public PokemonId Id;
        public double Perfection;
        public int Cp;
        public int BestCp;
        public double BestPerfection;
    }
}
