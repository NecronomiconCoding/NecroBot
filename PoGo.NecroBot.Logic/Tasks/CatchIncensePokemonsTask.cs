#region using directives

using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using POGOProtos.Enums;
using POGOProtos.Map.Pokemon;
using POGOProtos.Networking.Responses;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public static class CatchIncensePokemonsTask
    {
        public static async Task Execute(ISession session)
        {
            Logger.Write(session.Translation.GetTranslation(Common.TranslationString.LookingForIncensePokemon), LogLevel.Debug);


            var incensePokemon = await session.Client.Map.GetIncensePokemons();
            if (incensePokemon.Result == GetIncensePokemonResponse.Types.Result.IncenseEncounterAvailable)
            {
                var pokemon = new MapPokemon
                {
                    EncounterId = incensePokemon.EncounterId,
                    ExpirationTimestampMs = incensePokemon.DisappearTimestampMs,
                    Latitude = incensePokemon.Latitude,
                    Longitude = incensePokemon.Longitude,
                    PokemonId = (PokemonId) incensePokemon.PokemonTypeId,
                    SpawnPointId = incensePokemon.EncounterLocation
                };

                if (session.BotProfile.Settings.Bot.UsePokemonToNotCatchFilter &&
                    session.BotProfile.Settings.Bot.PokemonsNotToCatch.Contains(pokemon.PokemonId))
                {
                    Logger.Write(session.Translation.GetTranslation(Common.TranslationString.PokemonIgnoreFilter, pokemon.PokemonId));
                }
                else
                {
                    var distance = LocationUtils.CalculateDistanceInMeters(session.Client.CurrentLatitude,
                        session.Client.CurrentLongitude, pokemon.Latitude, pokemon.Longitude);
                    await Task.Delay(distance > 100 ? 3000 : 500);

                    var encounter =
                        await
                            session.Client.Encounter.EncounterIncensePokemon((long) pokemon.EncounterId,
                                pokemon.SpawnPointId);

                    if (encounter.Result == IncenseEncounterResponse.Types.Result.IncenseEncounterSuccess)
                    {
                        await CatchPokemonTask.Execute(session, encounter, pokemon);
                    }
                    else if (encounter.Result == IncenseEncounterResponse.Types.Result.PokemonInventoryFull)
                    {
                        if (session.BotProfile.Settings.Bot.TransferDuplicatePokemon)
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
                        session.EventDispatcher.Send(new WarnEvent {Message = session.Translation.GetTranslation(Common.TranslationString.EncounterProblem, encounter.Result)});
                    }
                }
            }
        }
    }
}