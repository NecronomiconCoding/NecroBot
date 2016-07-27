#region using directives

using System.Linq;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    /// <summary>
    /// This method will transfer all the pokemon no matter IV or CP given a ConfigPokemonsToTransfer.ini
    /// </summary>
    /// <returns></returns>
    public class ToTransferPokemonTask
    {
        public static async Task Execute(Context ctx, StateMachine machine)
        {
            var toTransferPokemons = await ctx.Inventory.GetPokemonToTransfer(ctx.LogicSettings.PokemonsToTransfer);

            var pokemonSettings = await ctx.Inventory.GetPokemonSettings();
            var pokemonFamilies = await ctx.Inventory.GetPokemonFamilies();

            foreach (var toTransferPokemon in toTransferPokemons)
            {
                await ctx.Client.Inventory.TransferPokemon(toTransferPokemon.Id);
                await ctx.Inventory.DeletePokemonFromInvById(toTransferPokemon.Id);

                var setting = pokemonSettings.Single(q => q.PokemonId == toTransferPokemon.PokemonId);
                var family = pokemonFamilies.First(q => q.FamilyId == setting.FamilyId);

                var bestPokemonOfType = (ctx.LogicSettings.PrioritizeIvOverCp
                    ? await ctx.Inventory.GetHighestPokemonOfTypeByIv(toTransferPokemon)
                    : await ctx.Inventory.GetHighestPokemonOfTypeByCp(toTransferPokemon)) ?? toTransferPokemon;
                
                family.Candy++;

                machine.Fire(new TransferPokemonEvent
                {
                    Id = toTransferPokemon.PokemonId,
                    Perfection = PokemonInfo.CalculatePokemonPerfection(toTransferPokemon),
                    Cp = toTransferPokemon.Cp,
                    BestCp = bestPokemonOfType.Cp,
                    BestPerfection = PokemonInfo.CalculatePokemonPerfection(bestPokemonOfType),
                    FamilyCandies = family.Candy
                });
            }
        }
    }
}
