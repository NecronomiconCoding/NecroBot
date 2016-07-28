#region using directives

using System.Linq;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using POGOProtos.Map.Pokemon;
using POGOProtos.Networking.Responses;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public static class CatchNearbyPokemonsTask
    {
        public static async Task Execute(ISession session)
        {
            Logger.Write(session.Translation.GetTranslation(Common.TranslationString.LookingForPokemon), LogLevel.Debug);

            var pokemons = await GetNearbyPokemons(session);
            foreach (var pokemon in pokemons)
            {
                if (session.LogicSettings.UsePokemonToNotCatchFilter &&
                    session.LogicSettings.PokemonsNotToCatch.Contains(pokemon.PokemonId))
                {
                    Logger.Write(session.Translation.GetTranslation(Common.TranslationString.PokemonSkipped, pokemon.PokemonId));
                    continue;
                }

                var distance = LocationUtils.CalculateDistanceInMeters(session.Client.CurrentLatitude,
                    session.Client.CurrentLongitude, pokemon.Latitude, pokemon.Longitude);
                await Task.Delay(distance > 100 ? 3000 : 500);

                var encounter = await session.Client.Encounter.EncounterPokemon(pokemon.EncounterId, pokemon.SpawnPointId);

                if (encounter.Status == EncounterResponse.Types.Status.EncounterSuccess)
                {
                    await CatchPokemonTask.Execute(session, encounter, pokemon);
                }
                else if (encounter.Status == EncounterResponse.Types.Status.PokemonInventoryFull)
                {
                    if (session.LogicSettings.TransferDuplicatePokemon)
                    {
                        session.EventDispatcher.Send(new WarnEvent {Message = session.Translation.GetTranslation(Common.TranslationString.InvFullTransferring)});
                        await TransferDuplicatePokemonTask.Execute(session);
                    }
                    else
                        session.EventDispatcher.Send(new WarnEvent
                        {
                            Message = session.Translation.GetTranslation(Common.TranslationString.InvFullTransferManually)
                        });
                }
                else
                {
                    session.EventDispatcher.Send(new WarnEvent {Message = session.Translation.GetTranslation(Common.TranslationString.EncounterProblem, encounter.Status)});
                }

                // If pokemon is not last pokemon in list, create delay between catches, else keep moving.
                if (!Equals(pokemons.ElementAtOrDefault(pokemons.Count() - 1), pokemon))
                {
                    await Task.Delay(session.LogicSettings.DelayBetweenPokemonCatch);
                }
            }
        }

        private static async Task<IOrderedEnumerable<MapPokemon>> GetNearbyPokemons(ISession session)
        {
            var mapObjects = await session.Client.Map.GetMapObjects();

            var pokemons = mapObjects.MapCells.SelectMany(i => i.CatchablePokemons)
                .OrderBy(
                    i =>
                        LocationUtils.CalculateDistanceInMeters(session.Client.CurrentLatitude, session.Client.CurrentLongitude,
                            i.Latitude, i.Longitude));

            return pokemons;
        }
    }
}