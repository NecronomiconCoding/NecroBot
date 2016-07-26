#region using directives

using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.PoGoUtils;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public static class DumpPokemonStatsTask
    {
        public static void Execute(Context ctx, StateMachine machine)
        {
            // Maximum pokebag is limited to 1000
            var PokeBag = ctx.LogicSettings.PrioritizeIvOverCp ? ctx.Inventory.GetHighestsPerfect(1000) : ctx.Inventory.GetHighestsCp(1000);

            var stringCount = 0;
            // Converts the list of pokemon into a string array to be dumped ordered by IV/CP depending on setting
            foreach (var pokemon in PokeBag.Result)
            {
                string PokemonStat = $"NAME: {pokemon.PokemonId} Lvl: { PokemonInfo.GetLevel(pokemon).ToString("00").PadLeft(4, ' ')} CP: { pokemon.Cp.ToString().PadLeft(4, ' ')} IV: { PokemonInfo.CalculatePokemonPerfection(pokemon).ToString("0.00")}";
                stringCount++;
                Logger.Dump(PokemonStat, "PokeBagStats", true);
            }
        }
    }
}