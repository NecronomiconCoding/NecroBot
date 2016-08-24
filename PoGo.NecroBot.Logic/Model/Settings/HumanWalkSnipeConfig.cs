using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    public class HumanWalkSnipeConfig
    {
        public bool DisplayPokemonList = true;
        public double MaxDistance = 1500.0;
        public double MaxEstimateTime = 600.0;
        public int CatchEmAllMinBalls = 50;
        public bool TryCatchEmAll = true;
        public bool CatchPokemonWhileWalking = true;
        public bool SpinWhileWalking = true;
        public bool AlwaysWalkback = false;
        public double SnipingScanOffset = 0.025;
        public bool Enable = true;
        public double WalkbackDistanceLimit = 300.0;


    }
}
