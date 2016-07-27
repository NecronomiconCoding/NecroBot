#region using directives

using POGOProtos.Enums;

#endregion

namespace PoGo.NecroBot.Logic.Event
{
    public class TransferPokemonEvent : IEvent
    {
        public int BestCp;
        public double BestPerfection;
        public int Cp;
        public int FamilyCandies;
        public PokemonId Id;
        public double Perfection;
    }
}