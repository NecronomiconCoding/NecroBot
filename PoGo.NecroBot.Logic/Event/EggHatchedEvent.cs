using POGOProtos.Enums;

namespace PoGo.NecroBot.Logic.Event
{
    public class EggHatchedEvent : IEvent
    {
        public ulong Id;
        public PokemonId PokemonId;
    }
}