using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Tasks
{
    public class DisplayPokemonStatsTask
    {
        public static void Execute(Context ctx, StateMachine machine)
        {
            Logger.Write("====== DisplayHighestsCP ======", LogLevel.Info, ConsoleColor.Yellow);
            var highestsPokemonCp = ctx.Inventory.GetHighestsCp(20).Result;
            foreach (var pokemon in highestsPokemonCp)
                Logger.Write(
                    $"# CP {pokemon.Cp.ToString().PadLeft(4, ' ')}/{PokemonInfo.CalculateMaxCp(pokemon).ToString().PadLeft(4, ' ')} | ({PokemonInfo.CalculatePokemonPerfection(pokemon).ToString("0.00")}% perfect)\t| Lvl {PokemonInfo.GetLevel(pokemon).ToString("00")}\t NAME: '{pokemon.PokemonId}'",
                    LogLevel.Info, ConsoleColor.Yellow);
            Logger.Write("====== DisplayHighestsPerfect ======", LogLevel.Info, ConsoleColor.Yellow);
            var highestsPokemonPerfect = ctx.Inventory.GetHighestsPerfect(20).Result;
            foreach (var pokemon in highestsPokemonPerfect)
            {
                Logger.Write(
                    $"# CP {pokemon.Cp.ToString().PadLeft(4, ' ')}/{PokemonInfo.CalculateMaxCp(pokemon).ToString().PadLeft(4, ' ')} | ({PokemonInfo.CalculatePokemonPerfection(pokemon).ToString("0.00")}% perfect)\t| Lvl {PokemonInfo.GetLevel(pokemon).ToString("00")}\t NAME: '{pokemon.PokemonId}'",
                    LogLevel.Info, ConsoleColor.Yellow);
            }
        }
    }
}
