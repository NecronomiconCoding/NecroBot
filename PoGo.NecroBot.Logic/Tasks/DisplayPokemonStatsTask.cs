using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;
using System;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Tasks
{
    public class DisplayPokemonStatsTask
    {
        public static async Task Execute(Context ctx, StateMachine machine)
        {
            var strHeader = ctx.Translations.GetTranslation(TranslationString.DisplayHighestsCPHeader);
            var strPerfect = ctx.Translations.GetTranslation(TranslationString.CommonWordPerfect);
            var strName = ctx.Translations.GetTranslation(TranslationString.CommonWordName).ToUpper();

            Logger.Write($"====== {strHeader} ======", LogLevel.Info, ConsoleColor.Yellow);
            var highestsPokemonCp = await ctx.Inventory.GetHighestsCp(20);
            foreach (var pokemon in highestsPokemonCp)
                Logger.Write(
                    $"# CP {pokemon.Cp.ToString().PadLeft(4, ' ')}/{PokemonInfo.CalculateMaxCp(pokemon).ToString().PadLeft(4, ' ')} | ({PokemonInfo.CalculatePokemonPerfection(pokemon).ToString("0.00")}% {strPerfect})\t| Lvl {PokemonInfo.GetLevel(pokemon).ToString("00")}\t {strName}: '{pokemon.PokemonId}'",
                    LogLevel.Info, ConsoleColor.Yellow);
            strHeader = ctx.Translations.GetTranslation(TranslationString.DisplayHighestsPerfectHeader);
            Logger.Write($"====== {strHeader} ======", LogLevel.Info, ConsoleColor.Yellow);
            var highestsPokemonPerfect = await ctx.Inventory.GetHighestsPerfect(20);
            foreach (var pokemon in highestsPokemonPerfect)
            {
                Logger.Write(
                    $"# CP {pokemon.Cp.ToString().PadLeft(4, ' ')}/{PokemonInfo.CalculateMaxCp(pokemon).ToString().PadLeft(4, ' ')} | ({PokemonInfo.CalculatePokemonPerfection(pokemon).ToString("0.00")}% {strPerfect})\t| Lvl {PokemonInfo.GetLevel(pokemon).ToString("00")}\t {strName}: '{pokemon.PokemonId}'",
                    LogLevel.Info, ConsoleColor.Yellow);
            }
        }
    }
}
