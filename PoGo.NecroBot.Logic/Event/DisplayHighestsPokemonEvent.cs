using POGOProtos.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Event
{
    public class DisplayHighestsPokemonEvent : IEvent
    {
        public String SortedBy;
        //PokemonData | CP | IV | Level
        public List<Tuple<PokemonData,int,double,double,double>> PokemonList;

    }
}
