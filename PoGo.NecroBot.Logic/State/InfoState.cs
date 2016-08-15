#region using directives

using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Tasks;
using PokemonGo.RocketAPI.Exceptions;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public class InfoState : IState
    {
        public async Task<IState> Execute(ISession session, CancellationToken cancellationToken)
        {
            //var inventory = await session.Inventory.RefreshCachedInventory();
            //if (!inventory.Success)
            //    throw new PermaBannedException();

            cancellationToken.ThrowIfCancellationRequested();
            await DisplayPokemonStatsTask.Execute(session);
            return new FarmState();
        }
    }
}
