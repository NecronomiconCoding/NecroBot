using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.CLI.Plugin
{
    public interface INecroPlugin
    {
        void Initialize(PluginInitializerInfo pii);
    }
}
