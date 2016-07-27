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
                await
                    ctx.Inventory.GetDuplicatePokemonToTransfer(ctx.LogicSettings.KeepPokemonsThatCanEvolve,
                        ctx.LogicSettings.PrioritizeIvOverCp,
                        ctx.LogicSettings.PokemonsNotToTransfer);

            var pokemonSettings = await ctx.Inventory.GetPokemonSettings();
            var pokemonFamilies = await ctx.Inventory.GetPokemonFamilies();

            foreach (var duplicatePokemon in duplicatePokemons)
            {
                if (duplicatePokemon.Cp >= ctx.Inventory.GetPokemonTransferFilter(duplicatePokemon.PokemonId).KeepMinCp ||
                    PokemonInfo.CalculatePokemonPerfection(duplicatePokemon) >
                    ctx.Inventory.GetPokemonTransferFilter(duplicatePokemon.PokemonId).KeepMinIvPercentage)
                {
                    continue;
                }

                await ctx.Client.Inventory.TransferPokemon(duplicatePokemon.Id);
                await ctx.Inventory.DeletePokemonFromInvById(duplicatePokemon.Id);

                var bestPokemonOfType = (ctx.LogicSettings.PrioritizeIvOverCp
                    ? await ctx.Inventory.GetHighestPokemonOfTypeByIv(duplicatePokemon)
                    : await ctx.Inventory.GetHighestPokemonOfTypeByCp(duplicatePokemon)) ?? duplicatePokemon;

                var setting = pokemonSettings.Single(q => q.PokemonId == duplicatePokemon.PokemonId);
                var family = pokemonFamilies.First(q => q.FamilyId == setting.FamilyId);

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