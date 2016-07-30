#region using directives

using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Tasks;
using PoGo.NecroBot.Logic.Event;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public class FarmState : IState
    {
        public static int gpxLoopCounter = 0;

        public async Task<IState> Execute(ISession session, CancellationToken cancellationToken)
        {
            if (session.LogicSettings.EvolveAllPokemonAboveIv || session.LogicSettings.EvolveAllPokemonWithEnoughCandy)
            {
                await EvolvePokemonTask.Execute(session, cancellationToken);
            }

            if (session.LogicSettings.TransferDuplicatePokemon)
            {
                await TransferDuplicatePokemonTask.Execute(session, cancellationToken);
            }

            if (session.LogicSettings.RenameAboveIv)
            {
                await RenamePokemonTask.Execute(session, cancellationToken);
            }

            await RecycleItemsTask.Execute(session, cancellationToken);

            if (session.LogicSettings.UseEggIncubators)
            {
                await UseIncubatorsTask.Execute(session, cancellationToken);
            }

            if (session.LogicSettings.UseGpxPathing)
            {
                //checks if output loop data is true and if so outputs the start of the current loop
                if (!session.LogicSettings.GpxSettings.InfiniteLoopOver && session.LogicSettings.GpxSettings.OutputLoopData)
                {                    
                    session.EventDispatcher.Send(new NoticeEvent
                    {                        
                        Message = session.Translation.GetTranslation(Common.TranslationString.GpxStartLoop, (gpxLoopCounter + 1))
                    });
                }

                await FarmPokestopsGpxTask.Execute(session, cancellationToken);

                //Checks if the config settings tell us to loop again
                if(!session.LogicSettings.GpxSettings.InfiniteLoopOver)
                {
                    //Increments the counter for gpx loops
                    gpxLoopCounter++;

                    //checks if output loop data is true and if so outputs the completion of the current loop
                    if (session.LogicSettings.GpxSettings.OutputLoopData)
                    {
                        session.EventDispatcher.Send(new NoticeEvent
                        {
                            Message = session.Translation.GetTranslation(Common.TranslationString.GpxEndLoop, gpxLoopCounter)
                        });
                    }

                    //Checks if the counter is above or equal to the config setting amount
                    if (gpxLoopCounter >= session.LogicSettings.GpxSettings.LoopNumber)
                    {
                        //outputs a message telling the user we are done
                        session.EventDispatcher.Send(new NoticeEvent
                        {
                            Message = session.Translation.GetTranslation(Common.TranslationString.GpxCompleted)
                        });

                        //return null so we exit the application
                        return null;
                    }
                }
            }
            else
            {
                await FarmPokestopsTask.Execute(session, cancellationToken);
            }

            return this;
        }
    }
}