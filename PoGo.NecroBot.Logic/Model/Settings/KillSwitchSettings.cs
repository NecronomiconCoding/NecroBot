using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    public class KillSwitchSettings
    {
        public bool UseKillSwitchCatch = false;
        public int CatchErrorPerHours = 40;
        public int CatchEscapePerHours = 40;
        public int CatchFleePerHours = 40;
        public int CatchMissedPerHours = 40;
        public int CatchSuccessPerHours = 40;
        public bool UseKillSwitchPokestops = false;
        public int AmountPokestops = 80;
    }
}
