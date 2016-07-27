#region using directives

using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Map.Fort;
using POGOProtos.Networking.Responses;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public static class CatchLurePokemonsTask
    {
        public static async Task Execute(Context ctx, StateMachine machine, FortData currentFortData)
        {
            Logger.Write("Looking for lure pokemon..", LogLevel.Debug);

            var fortId = currentFortData.Id;

            var pokemonId = currentFortData.LureInfo.ActivePokemonId;

            if (ctx.LogicSettings.UsePokemonToNotCatchFilter &&
                ctx.LogicSettings.PokemonsNotToCatch.Contains(pokemonId))
            {
                machine.Fire(new NoticeEvent {Message = $"Skipped {pokemonId}"});
            }
            else
            {
                var encounterId = currentFortData.LureInfo.EncounterId;
                var encounter = await ctx.Client.Encounter.EncounterLurePokemon(encounterId, fortId);

                if (encounter.Result == DiskEncounterResponse.Types.Result.Success)
                {
                    await CatchPokemonTask.Execute(ctx, machine, encounter, null, currentFortData, encounterId);
                }
                else if (encounter.Result == DiskEncounterResponse.Types.Result.PokemonInventoryFull)
                {
                    if (ctx.LogicClient.Settings.TransferDuplicatePokemon)
                    {
                        machine.Fire(new WarnEvent {Message = ctx.Translations.GetTranslation(Common.TranslationString.InvFullTransferring) });
                        await TransferDuplicatePokemonTask.Execute(ctx, machine);
                    }
                    else
                        machine.Fire(new WarnEvent
                        {
                            Message = ctx.Translations.GetTranslation(Common.TranslationString.InvFullTransferManually)
                        });
                }
                else
                {
                    machine.Fire(new WarnEvent {Message = $"Encounter problem: Lure Pokemon {encounter.Result}"});
                }
            }
        }
    }
}