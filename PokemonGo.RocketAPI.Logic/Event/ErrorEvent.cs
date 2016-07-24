using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Logic.Event
{
    public class ErrorEvent : IEvent
    {
        public string Message = "";
        public override string ToString()
        {
            return Message;
        }
    }
}
