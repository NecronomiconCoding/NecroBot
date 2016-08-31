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
        public double MaxEstimateTime = 900.0;
        public int CatchEmAllMinBalls = 50;
        public bool TryCatchEmAll = true;
        public bool CatchPokemonWhileWalking = true;
        public bool SpinWhileWalking = true;
        public bool AlwaysWalkback = false;
        public double SnipingScanOffset = 0.025;
        public bool Enable = true;
        public double WalkbackDistanceLimit = 300.0;
        public bool IncludeDefaultLocation = true;
        public bool UsePokeRadar = true;
        public bool UseSkiplagged = true;
        public bool UseSnipePokemonList = true;
        public double MaxSpeedUpSpeed = 60.0;
        public bool AllowSpeedUp = true;
        public int DelayTimeAtDestination = 10000;//  10 sec
    }
}
