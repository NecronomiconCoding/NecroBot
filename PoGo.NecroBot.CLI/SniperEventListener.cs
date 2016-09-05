#region using directives

using System;
using System.Diagnostics.CodeAnalysis;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Tasks;

#endregion

namespace PoGo.NecroBot.CLI
{
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    internal class SniperEventListener
    {
        private static void HandleEvent(PokemonCaptureEvent pokemonCaptureEvent, ISession session)
        {
            //remove pokemon from list
            HumanWalkSnipeTask.UpdateCatchPokemon(pokemonCaptureEvent.Latitude, pokemonCaptureEvent.Longitude,
                pokemonCaptureEvent.Id);
        }

        public static void HandleEvent(SnipePokemonFoundEvent ev, ISession session)
        {
            Logic.Tasks.HumanWalkSnipeTask.AddSnipePokemon("Local Feeder", ev.PokemonFound.Id,
                ev.PokemonFound.Latitude,
                ev.PokemonFound.Longitude,
                ev.PokemonFound.ExpirationTimestamp,
                ev.PokemonFound.IV, 
                session
                );
        }
        internal void Listen(IEvent evt, ISession session)
        {
            dynamic eve = evt;

            try
            {
                HandleEvent(eve, session);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}