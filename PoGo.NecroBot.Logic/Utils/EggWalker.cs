#region using directives

using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Tasks;

#endregion

namespace PoGo.NecroBot.Logic.Utils
{
    internal class EggWalker
    {
        private readonly double _checkInterval;
        private readonly ISession _session;

        private double _distanceTraveled;

        public EggWalker(double checkIncubatorsIntervalMeters, ISession session)
        {
            _checkInterval = checkIncubatorsIntervalMeters;
            _session = session;
        }

        public async Task ApplyDistance(double distanceTraveled, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!_session.LogicSettings.UseEggIncubators)
                return;

            _distanceTraveled += distanceTraveled;
            if (_distanceTraveled > _checkInterval)
            {
                await UseIncubatorsTask.Execute(_session, cancellationToken);
                _distanceTraveled = 0;
            }
        }
    }
}