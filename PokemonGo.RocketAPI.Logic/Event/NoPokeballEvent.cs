using POGOProtos.Enums;

namespace PoGo.NecroBot.Logic.Event
{
    public class NoPokeballEvent : IEvent
    {
        public PokemonId Id;
        public int Cp;
    }
}
