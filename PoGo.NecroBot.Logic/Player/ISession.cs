using PoGo.NecroBot.Logic.Event;
using PokemonGo.RocketAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Player
{
    public delegate void EventDelegate(IEvent evt);
    public interface ISession
    {
        ILogicSettings LogicSettings { get; }

        ISettings ApiSettings { get; }

        IEventHandler EventHandler { get; }

        Client Client { get; }

    }
}
