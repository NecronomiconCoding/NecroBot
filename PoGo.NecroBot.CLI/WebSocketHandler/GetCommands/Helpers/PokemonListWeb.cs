#region using directives

using PoGo.NecroBot.Logic.PoGoUtils;
using POGOProtos.Data;

#endregion

namespace PoGo.NecroBot.CLI.WebSocketHandler.GetCommands.Helpers
{
    public class PokemonListWeb
    {
        public PokemonData Base;

        public PokemonListWeb(PokemonData data)
        {
            Base = data;
        }

        public double IvPerfection => PokemonInfo.CalculatePokemonPerfection(Base);
    }
}