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
        public static async Task Execute(Session ctx, StateMachine machine, FortData currentFortData)
        {
            Logger.Write(ctx.Translations.GetTranslation(Common.TranslationString.LookingForLurePokemon), LogLevel.Debug);

            var fortId = currentFortData.Id;

            var pokemonId = currentFortData.LureInfo.ActivePokemonId;

            if (ctx.LogicSettings.UsePokemonToNotCatchFilter &&
                ctx.LogicSettings.PokemonsNotToCatch.Contains(pokemonId))
            {
                machine.Fire(new NoticeEvent {Message = ctx.Translations.GetTranslation(Common.TranslationString.PokemonSkipped, pokemonId)});
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
                    if (ctx.LogicSettings.TransferDuplicatePokemon)
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
                    if (encounter.Result.ToString().Contains("NotAvailable")) return;
                    machine.Fire(new WarnEvent {Message = ctx.Translations.GetTranslation(Common.TranslationString.EncounterProblemLurePokemon, encounter.Result)});
                }
            }
        }
    }
}