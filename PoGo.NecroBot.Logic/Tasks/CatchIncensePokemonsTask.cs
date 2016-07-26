using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using POGOProtos.Map.Pokemon;

namespace PoGo.NecroBot.Logic.Tasks
{
    public static class CatchIncensePokemonsTask
    {
        public static void Execute(Context ctx, StateMachine machine)
        {
            Logger.Write("Looking for incense pokemon..", LogLevel.Debug);

            var encounter = ctx.Client.Encounter.EncounterIncensePokemon()

        }
    }
}
