#region using directives

using System;
using System.Collections.Generic;
using POGOProtos.Data;

#endregion

namespace PoGo.NecroBot.Logic.Event
{
    public class DisplayHighestsPokemonEvent : IEvent
    {
        //PokemonData | CP | IV | Level
        public List<Tuple<PokemonData, int, double, double>> PokemonList;
        public string SortedBy;
    }
}