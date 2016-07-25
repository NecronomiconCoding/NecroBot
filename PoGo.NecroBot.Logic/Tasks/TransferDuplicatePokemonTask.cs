#region using directives

using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;

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

                machine.Fire(new TransferPokemonEvent
                {
                    Id = duplicatePokemon.PokemonId,
                    Perfection = PokemonInfo.CalculatePokemonPerfection(duplicatePokemon),
                    Cp = duplicatePokemon.Cp,
                    BestCp = bestPokemonOfType.Cp,
                    BestPerfection = PokemonInfo.CalculatePokemonPerfection(bestPokemonOfType)
                });
            }
        }
    }
}