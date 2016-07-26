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
        public static async Task Execute(Context ctx, StateMachine machine)
        {
            Logger.Write("====== DisplayHighestsCP ======", LogLevel.Info, ConsoleColor.Yellow);
            var highestsPokemonCp = await ctx.Inventory.GetHighestsCp(20);
            foreach (var pokemon in highestsPokemonCp)
                Logger.Write(
                    $"# CP {pokemon.Cp.ToString().PadLeft(4, ' ')}/{PokemonInfo.CalculateMaxCp(pokemon).ToString().PadLeft(4, ' ')} | ({PokemonInfo.CalculatePokemonPerfection(pokemon).ToString("0.00")}% perfect)\t| Lvl {PokemonInfo.GetLevel(pokemon).ToString("00")}\t NAME: '{pokemon.PokemonId}'",
                    LogLevel.Info, ConsoleColor.Yellow);
            Logger.Write("====== DisplayHighestsPerfect ======", LogLevel.Info, ConsoleColor.Yellow);
            var highestsPokemonPerfect = await ctx.Inventory.GetHighestsPerfect(20);
            foreach (var pokemon in highestsPokemonPerfect)
            {
                Logger.Write(
                    $"# CP {pokemon.Cp.ToString().PadLeft(4, ' ')}/{PokemonInfo.CalculateMaxCp(pokemon).ToString().PadLeft(4, ' ')} | ({PokemonInfo.CalculatePokemonPerfection(pokemon).ToString("0.00")}% perfect)\t| Lvl {PokemonInfo.GetLevel(pokemon).ToString("00")}\t NAME: '{pokemon.PokemonId}'",
                    LogLevel.Info, ConsoleColor.Yellow);
            }

            Logger.Write($"WTF IS GOING ON {ctx.LogicSettings.DumpPokemonStats}", LogLevel.Info);
            if (ctx.LogicSettings.DumpPokemonStats)
            {
                // Maximum pokebag is limited to 1000
                var allPokemonInBag = ctx.LogicSettings.PrioritizeIvOverCp ? await ctx.Inventory.GetHighestsPerfect(1000) : await ctx.Inventory.GetHighestsCp(1000);
                string dumpFileName = "PokeBagStats";
                // Converts the list of pokemon into a string array to be dumped ordered by IV/CP depending on setting
                foreach (var pokemon in allPokemonInBag)
                {
                    Logger.Dump($"NAME: {pokemon.PokemonId}\tLvl: { PokemonInfo.GetLevel(pokemon).ToString("00")}\tCP: { pokemon.Cp.ToString()}\tIV: { PokemonInfo.CalculatePokemonPerfection(pokemon).ToString("0.00")}", dumpFileName);
                }
            }
        }
    }
}
