#region using directives

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using POGOProtos.Inventory.Item;
using POGOProtos.Map.Pokemon;
using POGOProtos.Networking.Responses;
using POGOProtos.Enums;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public static class CatchNearbyPokemonsTask
    {
        public static async Task Execute(ISession session, CancellationToken cancellationToken, PokemonId priority = PokemonId.Missingno, bool sessionAllowTransfer= true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!session.LogicSettings.CatchPokemon) return;

            Logger.Write(session.Translation.GetTranslation(TranslationString.LookingForPokemon), LogLevel.Debug);

            var nearbyPokemons = await GetNearbyPokemons(session);
            var pokemons = nearbyPokemons.Where(p => p.PokemonId == priority).ToList();
            pokemons.AddRange(nearbyPokemons.Where(p => p.PokemonId != priority).ToList());

            foreach (var pokemon in pokemons)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var allitems = await session.Inventory.GetItems();
                var pokeBallsCount = allitems.FirstOrDefault(i => i.ItemId == ItemId.ItemPokeBall)?.Count;
                var greatBallsCount = allitems.FirstOrDefault(i => i.ItemId == ItemId.ItemGreatBall)?.Count;
                var ultraBallsCount = allitems.FirstOrDefault(i => i.ItemId == ItemId.ItemUltraBall)?.Count;
                var masterBallsCount = allitems.FirstOrDefault(i => i.ItemId == ItemId.ItemMasterBall)?.Count;

                /*
                var pokeBallsCount = await session.Inventory.GetItemAmountByType(ItemId.ItemPokeBall);
                var greatBallsCount = await session.Inventory.GetItemAmountByType(ItemId.ItemGreatBall);
                var ultraBallsCount = await session.Inventory.GetItemAmountByType(ItemId.ItemUltraBall);
                var masterBallsCount = await session.Inventory.GetItemAmountByType(ItemId.ItemMasterBall);
                */

                if (pokeBallsCount + greatBallsCount + ultraBallsCount + masterBallsCount == 0)
                {
                    Logger.Write(session.Translation.GetTranslation(TranslationString.ZeroPokeballInv));
                    return;
                }

                if( ( session.LogicSettings.UsePokemonSniperFilterOnly && !session.LogicSettings.PokemonToSnipe.Pokemon.Contains( pokemon.PokemonId ) ) ||
                    ( session.LogicSettings.UsePokemonToNotCatchFilter && session.LogicSettings.PokemonsNotToCatch.Contains( pokemon.PokemonId ) ) )
                {
                    Logger.Write(session.Translation.GetTranslation(TranslationString.PokemonSkipped, session.Translation.GetPokemonTranslation(pokemon.PokemonId)));
                    continue;
                }

                var distance = LocationUtils.CalculateDistanceInMeters(session.Client.CurrentLatitude,
                    session.Client.CurrentLongitude, pokemon.Latitude, pokemon.Longitude);
                await Task.Delay(distance > 100 ? 500 : 100, cancellationToken);

                var encounter =
                    await session.Client.Encounter.EncounterPokemon(pokemon.EncounterId, pokemon.SpawnPointId);

                if (encounter.Status == EncounterResponse.Types.Status.EncounterSuccess && session.LogicSettings.CatchPokemon)
                {
                    await CatchPokemonTask.Execute(session, cancellationToken, encounter, pokemon, sessionAllowTransfer:sessionAllowTransfer);
                }
                else if (encounter.Status == EncounterResponse.Types.Status.PokemonInventoryFull)
                {
					if (session.LogicSettings.TransferDuplicatePokemon || session.LogicSettings.TransferWeakPokemon)
					{
						session.EventDispatcher.Send(new WarnEvent
						{
							Message = session.Translation.GetTranslation(TranslationString.InvFullTransferring)
						});
						if(session.LogicSettings.TransferDuplicatePokemon)
							await TransferDuplicatePokemonTask.Execute(session, cancellationToken);
						if(session.LogicSettings.TransferWeakPokemon)
							await TransferWeakPokemonTask.Execute(session, cancellationToken);
					}
                    else
                        session.EventDispatcher.Send(new WarnEvent
                        {
                            Message = session.Translation.GetTranslation(TranslationString.InvFullTransferManually)
                        });
                }
                else
                {
                    session.EventDispatcher.Send(new WarnEvent
                    {
                        Message =
                            session.Translation.GetTranslation(TranslationString.EncounterProblem, encounter.Status)
                    });
                }

                // If pokemon is not last pokemon in list, create delay between catches, else keep moving.
                if (!Equals(pokemons.ElementAtOrDefault(pokemons.Count() - 1), pokemon))
                {
                    await Task.Delay(session.LogicSettings.DelayBetweenPokemonCatch, cancellationToken);
                }
            }
        }

        public static async Task<IOrderedEnumerable<MapPokemon>> GetNearbyPokemons(ISession session)
        {
            var mapObjects = await session.Client.Map.GetMapObjects();

            var pokemons = mapObjects.Item1.MapCells.SelectMany(i => i.CatchablePokemons)
                .OrderBy(
                    i =>
                        LocationUtils.CalculateDistanceInMeters(session.Client.CurrentLatitude,
                            session.Client.CurrentLongitude,
                            i.Latitude, i.Longitude));

            return pokemons;
        }
    }
}
