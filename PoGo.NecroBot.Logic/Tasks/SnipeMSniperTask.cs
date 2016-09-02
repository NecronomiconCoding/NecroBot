
using GeoCoordinatePortable;
using Newtonsoft.Json;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Model.Settings;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using POGOProtos.Enums;
using POGOProtos.Map.Pokemon;
using POGOProtos.Networking.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Tasks
{
    public class MSniperInfo
    {
        public PokemonId Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public static class SnipeMSniperTask
    {
        public static async Task CheckMSniperLocation(ISession session, CancellationToken cancellationToken)
        {
            string pth = Path.Combine(session.LogicSettings.ProfilePath, "SnipeMS.json");
            try
            {
                if (session.LogicSettings.CatchPokemon == true &&
                    session.LogicSettings.SnipeAtPokestops == false &&
                    session.LogicSettings.UseSnipeLocationServer == false /* &&
                    session.LogicSettings.EnableHumanWalkingSnipe == false */
                    )//extra security
                {

                    if (!File.Exists(pth))
                        return;

                    if (!await SnipePokemonTask.CheckPokeballsToSnipe(session.LogicSettings.MinPokeballsWhileSnipe + 1, session, cancellationToken))
                        return;

                    StreamReader sr = new StreamReader(pth, Encoding.UTF8);
                    string jsn = sr.ReadToEnd();
                    sr.Close();
                    List<MSniperInfo> MSniperLocation = JsonConvert.DeserializeObject<List<MSniperInfo>>(jsn);
                    File.Delete(pth);
                    foreach (var location in MSniperLocation)
                    {
                        session.EventDispatcher.Send(new SnipeScanEvent
                        {
                            Bounds = new Location(location.Latitude, location.Longitude),
                            PokemonId = location.Id,
                            Source = "MSniper"
                        });

                        await OwnSnipe(session, location.Id, location.Latitude, location.Longitude, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                File.Delete(pth);//fixing deserialize errors
                ErrorEvent ee = new ErrorEvent { Message = ex.Message };
                if (ex.InnerException != null) ee.Message = ex.InnerException.Message;
                session.EventDispatcher.Send(ee);
            }
        }

        public static async Task OwnSnipe(ISession session, PokemonId TargetPokemonId, double latitude,
           double longitude, CancellationToken cancellationToken, bool sessionAllowTransfer = true)
        {
            var CurrentLatitude = session.Client.CurrentLatitude;
            var CurrentLongitude = session.Client.CurrentLongitude;

            session.EventDispatcher.Send(new SnipeModeEvent { Active = true });

            List<MapPokemon> catchablePokemon;
            try
            {
                await LocationUtils.UpdatePlayerLocationWithAltitude(session, new GeoCoordinate(latitude, longitude, session.Client.CurrentAltitude));

                session.EventDispatcher.Send(new UpdatePositionEvent
                {
                    Longitude = longitude,
                    Latitude = latitude
                });

                var nearbyPokemons = await CatchNearbyPokemonsTask.GetNearbyPokemons(session);
                catchablePokemon = nearbyPokemons.Where(p => p.PokemonId == TargetPokemonId).ToList();

            }
            finally
            {
                await LocationUtils.UpdatePlayerLocationWithAltitude(session, new GeoCoordinate(CurrentLatitude, CurrentLongitude, session.Client.CurrentAltitude));
            }

            if (catchablePokemon.Count > 0)
            {
                foreach (var pokemon in catchablePokemon)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    EncounterResponse encounter;
                    try
                    {
                        await LocationUtils.UpdatePlayerLocationWithAltitude(session, new GeoCoordinate(latitude, longitude, session.Client.CurrentAltitude));

                        encounter = await session.Client.Encounter.EncounterPokemon(pokemon.EncounterId, pokemon.SpawnPointId);
                    }
                    finally
                    {
                        await LocationUtils.UpdatePlayerLocationWithAltitude(session, new GeoCoordinate(CurrentLatitude, CurrentLongitude, session.Client.CurrentAltitude));
                    }

                    if (encounter.Status == EncounterResponse.Types.Status.EncounterSuccess)
                    {
                        session.EventDispatcher.Send(new UpdatePositionEvent
                        {
                            Latitude = CurrentLatitude,
                            Longitude = CurrentLongitude
                        });

                        await CatchPokemonTask.Execute(session, cancellationToken, encounter, pokemon);
                    }
                    else if (encounter.Status == EncounterResponse.Types.Status.PokemonInventoryFull)
                    {
                        if (session.LogicSettings.TransferDuplicatePokemon || session.LogicSettings.TransferWeakPokemon)
                        {
                            session.EventDispatcher.Send(new WarnEvent
                            {
                                Message = session.Translation.GetTranslation(TranslationString.InvFullTransferring)
                            });
                            if (session.LogicSettings.TransferDuplicatePokemon)
                                await TransferDuplicatePokemonTask.Execute(session, cancellationToken);
                            if (session.LogicSettings.TransferWeakPokemon)
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

                    if (!Equals(catchablePokemon.ElementAtOrDefault(catchablePokemon.Count() - 1), pokemon))
                    {
                        await Task.Delay(2000, cancellationToken);
                    }
                }
            }
            else
            {
                session.EventDispatcher.Send(new SnipeEvent
                {
                    Message = session.Translation.GetTranslation(TranslationString.NoPokemonToSnipe)
                });
            }

            session.EventDispatcher.Send(new SnipeModeEvent { Active = false });

            await Task.Delay(5000, cancellationToken);
        }


    }
}
