using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Settings {
    public interface IProfileSettings {
        IAuthenticationSettings Account { get; }
        IConfigurationSettings  Bot     { get; }
    }
}
