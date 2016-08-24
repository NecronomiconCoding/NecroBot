using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    public class HumanWalkSnipeConfig
    {
        public bool HumanWalkingSnipeDisplayList = true;
        public double HumanWalkingSnipeMaxDistance = 1000.0;
        public double HumanWalkingSnipeMaxEstimateTime = 300.0;
        public int HumanWalkingSnipeCatchEmAllMinBalls = 100;
        public bool HumanWalkingSnipeTryCatchEmAll = true;
        public bool HumanWalkingSnipeCatchPokemonWhileWalking = true;
        public bool HumanWalkingSnipeSpinWhileWalking = true;
        public bool HumanWalkingSnipeAlwaysWalkBack = false;
        public double HumanWalkingSnipeSnipingScanOffset = 0.015;
        public bool EnableHumanWalkingSnipe = true;

    }
}
