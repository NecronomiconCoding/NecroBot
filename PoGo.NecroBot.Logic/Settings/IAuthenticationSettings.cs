using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonGo.RocketAPI.Enums;

namespace PoGo.NecroBot.Logic.Settings {
    public interface IAuthenticationSettings {
        AuthType AuthenticationType { get; set; }
        string PtcUsername { get; set; }
        string PtcPassword { get; set; }
        string GoogleRefreshToken { get; set; }
    }
}
