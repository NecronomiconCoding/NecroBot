using POGOProtos.Data;
using System;
using System.Collections.Generic;

namespace PoGo.NecroBot.Logic.Event
{
    public class DisplayHighestsPokemonEvent : IEvent
    {
        public string SortedBy;
        //PokemonData | CP | IV | Level
        public List<Tuple<PokemonData,int,double,double>> PokemonList;

    }
}
