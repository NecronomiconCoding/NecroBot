#region using directives

using System.Linq;
using System.Threading;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public class TransferDuplicatePokemonTask
    {
        public static void Execute(Context ctx, StateMachine machine)
        {
            var duplicatePokemons =
                ctx.Inventory.GetDuplicatePokemonToTransfer(ctx.LogicSettings.KeepPokemonsThatCanEvolve, ctx.LogicSettings.PrioritizeIvOverCp,
                    ctx.LogicSettings.PokemonsNotToTransfer).Result;

            var pokemonSettings = ctx.Inventory.GetPokemonSettings().Result;
            var pokemonFamilies = ctx.Inventory.GetPokemonFamilies().Result;

            foreach (var duplicatePokemon in duplicatePokemons)
            {
                if (duplicatePokemon.Cp >= ctx.LogicSettings.KeepMinCp ||
                    PokemonInfo.CalculatePokemonPerfection(duplicatePokemon) > ctx.LogicSettings.KeepMinIvPercentage)
                {
                    continue;
                }

                ctx.Client.Inventory.TransferPokemon(duplicatePokemon.Id).Wait();
                ctx.Inventory.DeletePokemonFromInvById(duplicatePokemon.Id);

                var bestPokemonOfType = ctx.LogicSettings.PrioritizeIvOverCp
                    ? ctx.Inventory.GetHighestPokemonOfTypeByIv(duplicatePokemon).Result
                    : ctx.Inventory.GetHighestPokemonOfTypeByCp(duplicatePokemon).Result;

                if (bestPokemonOfType == null)
                    bestPokemonOfType = duplicatePokemon;

                var setting = pokemonSettings.Single(q => q.PokemonId == duplicatePokemon.PokemonId);
                var family = pokemonFamilies.Single(q => q.FamilyId == setting.FamilyId);

                family.Candy++;

                machine.Fire(new TransferPokemonEvent
                {
                    Id = duplicatePokemon.PokemonId,
                    Perfection = PokemonInfo.CalculatePokemonPerfection(duplicatePokemon),
                    Cp = duplicatePokemon.Cp,
                    BestCp = bestPokemonOfType.Cp,
                    BestPerfection = PokemonInfo.CalculatePokemonPerfection(bestPokemonOfType),
                    FamilyCandies = family.Candy
                });

                DelayingUtils.Delay(ctx.LogicSettings.DelayBetweenPlayerActions, 0);
            }
        }
    }
}