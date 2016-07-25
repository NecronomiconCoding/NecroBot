using POGOProtos.Enums;
using POGOProtos.Networking.Responses;

namespace PoGo.NecroBot.Logic.Event
{
    public class PokemonEvolveEvent : IEvent
    {
        public PokemonId Id;
        public int Exp;
        public EvolvePokemonResponse.Types.Result Result;
    }
}
