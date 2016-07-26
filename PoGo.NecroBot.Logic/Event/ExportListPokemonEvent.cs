using POGOProtos.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Event
{
    public class ExportListPokemonEvent : IEvent
    {
        public String SortedBy;
        //PokemonData | CP | IV | Level
        public bool ExportSuccessful;
        public int PokemonCount;
        public String Path;
        public String Message;
    }
}
