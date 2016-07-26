#region using directives

using System.Linq;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public class TransferDuplicatePokemonTask
    {
        public static async Task Execute(Context ctx, StateMachine machine)
        {
            var duplicatePokemons =
<<<<<<< HEAD
                ctx.Inventory.GetDuplicatePokemonToTransfer(false, ctx.LogicSettings.PokemonSelector,
                    ctx.LogicSettings.PokemonsNotToTransfer).Result;
=======
                await ctx.Inventory.GetDuplicatePokemonToTransfer(ctx.LogicSettings.KeepPokemonsThatCanEvolve, ctx.LogicSettings.PrioritizeIvOverCp,
                    ctx.LogicSettings.PokemonsNotToTransfer);
>>>>>>> refs/remotes/NecronomiconCoding/master

            var pokemonSettings = await ctx.Inventory.GetPokemonSettings();
            var pokemonFamilies = await ctx.Inventory.GetPokemonFamilies();

            foreach (var duplicatePokemon in duplicatePokemons)
            {
                if (duplicatePokemon.Cp >= ctx.LogicSettings.KeepMinCp ||
                    PokemonInfo.CalculatePokemonPerfection(duplicatePokemon) > ctx.LogicSettings.KeepMinIvPercentage)
                {
                    continue;
                }

                await ctx.Client.Inventory.TransferPokemon(duplicatePokemon.Id);
                await ctx.Inventory.DeletePokemonFromInvById(duplicatePokemon.Id);

<<<<<<< HEAD
                var bestPokemonOfType = ctx.Inventory.GetHighestPokemonOfTypeBySelector(duplicatePokemon, ctx.LogicSettings.PokemonSelector).Result;
                 
=======
                var bestPokemonOfType = ctx.LogicSettings.PrioritizeIvOverCp
                    ? await ctx.Inventory.GetHighestPokemonOfTypeByIv(duplicatePokemon)
                    : await ctx.Inventory.GetHighestPokemonOfTypeByCp(duplicatePokemon);

>>>>>>> refs/remotes/NecronomiconCoding/master
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
            }
        }
    }
}