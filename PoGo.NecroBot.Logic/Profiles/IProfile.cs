using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Tasks;
using PoGo.NecroBot.Logic.Settings;


namespace PoGo.NecroBot.Logic.Profiles {
    public interface IProfile {
        string Name { get; }
        string FilePath { get; }
        IProfileSettings Settings { get; }
    }
}
