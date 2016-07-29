#region using directives

using System.Linq;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;

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
                        session.LogicSettings.PokemonsNotToTransfer,
                        session.LogicSettings.TransferDuplicateEvolvedPokemon);

            var pokemonSettings = await session.Inventory.GetPokemonSettings();
            var pokemonFamilies = await session.Inventory.GetPokemonFamilies();
            var pokemonsIE = await session.Inventory.GetPokemons();
            var pokemons = pokemonsIE.ToList();

            foreach (var duplicatePokemon in duplicatePokemons)
            {
                bool lessEvolved = session.LogicSettings.TransferDuplicateEvolvedPokemon && duplicatePokemons.Where(p => p.PokemonId == duplicatePokemon.PokemonId).Count() <= (pokemons.Where(ip => ip.PokemonId == duplicatePokemon.PokemonId).Count() - session.LogicSettings.KeepEvolvedDuplicates);
                bool lowerEvolvedCp = session.LogicSettings.TransferDuplicateEvolvedPokemon && pokemons.Where(p => p.PokemonId == duplicatePokemon.PokemonId).OrderByDescending(p => p.Cp).ElementAt(session.LogicSettings.KeepEvolvedDuplicates - 1).Cp > duplicatePokemon.Cp;
                if ((duplicatePokemon.Cp >= session.Inventory.GetPokemonTransferFilter(duplicatePokemon.PokemonId).KeepMinCp ||
                    PokemonInfo.CalculatePokemonPerfection(duplicatePokemon) >
                    session.Inventory.GetPokemonTransferFilter(duplicatePokemon.PokemonId).KeepMinIvPercentage) &&
                    (!session.LogicSettings.TransferDuplicateEvolvedPokemon ||
                        (!lessEvolved &&
                        !lowerEvolvedCp)))
                {
                    continue;
                }

                await session.Client.Inventory.TransferPokemon(duplicatePokemon.Id);
                await session.Inventory.DeletePokemonFromInvById(duplicatePokemon.Id);

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

                DelayingUtils.Delay(session.LogicSettings.DelayBetweenPlayerActions, 0);
            }
        }
    }
}