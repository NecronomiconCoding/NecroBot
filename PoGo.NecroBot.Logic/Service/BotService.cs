using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Service
{
    public class BotService
    {
        public ISession _session;
        public ILogin _loginTask;

        public void Run()
        {
            _loginTask.DoLogin();

        }
    }
}
