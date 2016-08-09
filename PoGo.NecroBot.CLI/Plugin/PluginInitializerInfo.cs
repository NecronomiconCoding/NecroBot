using PoGo.NecroBot.Logic;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.CLI.Plugin
{
    public class PluginInitializerInfo
    {
        public Session Session { get; set; }
        public GlobalSettings Settings { get; set; }
        public ConsoleLogger Logger { get; set; }
        public Statistics Statistics { get; set; }
    }
}
