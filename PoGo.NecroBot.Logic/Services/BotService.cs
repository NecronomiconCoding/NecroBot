using PoGo.NecroBot.Logic.Player;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Services
{
    public class BotService
    {
        private ISession _session;
        private StateMachine _machine;

        public BotService(ISession session)
        {
            _session = session;
            _machine = new StateMachine();
        }

        public void Run()
        {
        }
    }
}
