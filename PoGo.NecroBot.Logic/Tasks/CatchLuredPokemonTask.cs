using System.Threading;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Networking.Responses;


namespace PoGo.NecroBot.Logic.Tasks
{
    class CatchLuredPokemonTask
    { 
        public static void Execute(Context ctx, StateMachine machine, POGOProtos.Map.Fort.FortData pokeStop)
        {
            if (pokeStop.LureInfo != null) // TODO: Currently no way to distinguish between pokemon already encountered/not.
            {
                Logger.Write($"Found a lured pokemon : {pokeStop.LureInfo.ActivePokemonId}", LogLevel.Debug);

                ulong EncounterId = EncounterId = pokeStop.LureInfo.EncounterId;
                string FortId = pokeStop.LureInfo.FortId;

                var encounter = ctx.Client.Encounter.EncounterLurePokemon(EncounterId, FortId).Result;

                if (encounter.Result == DiskEncounterResponse.Types.Result.Success)
                {
                    CatchPokemonTask.Execute(ctx, machine, encounter, EncounterId, FortId);
                }
                else
                {
                    machine.Fire(new WarnEvent { Message = $"Lure encounter problem: {encounter.Result}" });
                }

                Thread.Sleep(ctx.LogicSettings.DelayBetweenPokemonCatch);
            }
        }
    }
}
