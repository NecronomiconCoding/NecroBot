using POGOProtos.Enums;
using POGOProtos.Inventory.Item;
using POGOProtos.Networking.Responses;

namespace PoGo.NecroBot.Logic.Event
{
    public class PokemonCaptureEvent : IEvent
    {
        public int Exp;
        public int Stardust;
        public CatchPokemonResponse.Types.CatchStatus Status;
        public double Level;
        public PokemonId Id;
        public int Cp;
        public int MaxCp;
        public double Perfection;
        public double Probability;
        public double Distance;
        public ItemId Pokeball;
        public int Attempt;
    }
}
