using PoGo.NecroBot.Logic.Tasks;

namespace PoGo.NecroBot.Logic.Event
{
    public class SnipePokemonFoundEvent : IEvent
    {
        public SniperInfo PokemonFound { get; set; }
        public override string ToString()
        {
            return PokemonFound.Id.ToString();
        }
    }
}