using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Tasks.custom
{
    public class FarmControl
    {
        public static bool stopping = true;
        public static bool stoped = true;
        public static bool flying = false;
        public static double flyLatitude = 0;
        public static double flyLongitude = 0;
        public static string flyCatchName = "";
        public static bool isFirstStart = true;
        public static bool transfering = false;
        public static bool snipping = false;
        public static bool autoFarm = true;
    }
}
