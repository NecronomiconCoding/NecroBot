using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Localization;
using PokemonGo.RocketAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Service
{
    public interface ISession
    {
        Client Client { get; }
        IEventDispatcher EventDispatcher {get;}
        ILocalizer Localizer { get; }
    }
}
