#region using directives

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Enums;
using POGOProtos.Inventory.Item;
using POGOProtos.Networking.Responses;
#endregion

namespace PoGo.NecroBot.CLI
{
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    internal class SniperEventListener
    {
        private static void HandleEvent(PokemonCaptureEvent pokemonCaptureEvent, ISession session)
        {
            //remove pokemon from list
            Logic.Tasks.HumanWalkSnipeTask.UpdateCatchPokemon(pokemonCaptureEvent.Latitude, pokemonCaptureEvent.Longitude, pokemonCaptureEvent.Id);
        }

        public static void HandleEvent(SnipePokemonFoundEvent ev, ISession session)
        {
            Logic.Tasks.HumanWalkSnipeTask.AddSnipePokemon("Local Feeder",
                ev.PokemonFound.Id,
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
            { HandleEvent(eve, session); }
            catch
            { }
        }
    }
}
