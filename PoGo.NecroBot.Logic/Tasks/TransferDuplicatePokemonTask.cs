#region using directives

using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public class TransferDuplicatePokemonTask
    {
        public static async Task Execute(ISession session)
        {
            var duplicatePokemons =
                await
                    session.Inventory.GetDuplicatePokemonToTransfer(session.LogicSettings.KeepPokemonsThatCanEvolve,
                        session.LogicSettings.PrioritizeIvOverCp,
                        session.LogicSettings.PokemonsNotToTransfer);

            var pokemonSettings = await session.Inventory.GetPokemonSettings();
            var pokemonFamilies = await session.Inventory.GetPokemonFamilies();

            var random = new Random();

            foreach (var duplicatePokemon in duplicatePokemons)
            {
                if (duplicatePokemon.Cp >= session.Inventory.GetPokemonTransferFilter(duplicatePokemon.PokemonId).KeepMinCp ||
                    PokemonInfo.CalculatePokemonPerfection(duplicatePokemon) >
                    session.Inventory.GetPokemonTransferFilter(duplicatePokemon.PokemonId).KeepMinIvPercentage)
                {
                    continue;
                }

                await session.Client.Inventory.TransferPokemon(duplicatePokemon.Id);
                await session.Inventory.DeletePokemonFromInvById(duplicatePokemon.Id);

                await Randomizer.Sleep(2500, 0.3);

                var bestPokemonOfType = (session.LogicSettings.PrioritizeIvOverCp
                    ? await session.Inventory.GetHighestPokemonOfTypeByIv(duplicatePokemon)
                    : await session.Inventory.GetHighestPokemonOfTypeByCp(duplicatePokemon)) ?? duplicatePokemon;

                var setting = pokemonSettings.Single(q => q.PokemonId == duplicatePokemon.PokemonId);
                var family = pokemonFamilies.First(q => q.FamilyId == setting.FamilyId);

                family.Candy++;

                session.EventDispatcher.Send(new TransferPokemonEvent
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