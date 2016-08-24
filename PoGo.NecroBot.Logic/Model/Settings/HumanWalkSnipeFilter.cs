using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POGOProtos.Enums;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    public class HumanWalkSnipeFilter
    {
        public double MaxDistance { get; set; }
        public int Priority { get; set; }

        public double MaxWalkTimes { get; set; }

        public bool CatchPokemonWhileWalking { get; set; }

        public bool SpinPokestopWhileWalking { get; set; }

        public HumanWalkSnipeFilter(double maxDistance, double maxWalkTimes, int priority, bool catchPokemon, bool spinPokestop)
        {
            this.MaxDistance = maxDistance;
            this.MaxWalkTimes = maxWalkTimes;
            this.Priority = priority;
            this.CatchPokemonWhileWalking = catchPokemon;
            this.SpinPokestopWhileWalking = spinPokestop;
        }

        internal static Dictionary<PokemonId, HumanWalkSnipeFilter> Default()
        {
            return new Dictionary<PokemonId, HumanWalkSnipeFilter>
            {
                { PokemonId.Magikarp, new HumanWalkSnipeFilter(300, 100, 10, true, true)},
                { PokemonId.Electabuzz, new HumanWalkSnipeFilter(1500, 600, 2, true, true)},
                { PokemonId.Dragonite, new HumanWalkSnipeFilter(3000, 900, 1, false, false)},
                { PokemonId.Charizard, new HumanWalkSnipeFilter(3000, 900, 1, false, false)},
                { PokemonId.Snorlax, new HumanWalkSnipeFilter(3000, 900, 1, false, false)},
                { PokemonId.Lapras, new HumanWalkSnipeFilter(3000, 900, 1, false, false)} ,
                { PokemonId.Exeggutor, new HumanWalkSnipeFilter(1500, 600, 1, false, true)} ,
                { PokemonId.Vaporeon, new HumanWalkSnipeFilter(1800, 800, 2, false, false)}   ,
                { PokemonId.Flareon, new HumanWalkSnipeFilter(1800, 800, 2, false, false)},
                { PokemonId.Scyther, new HumanWalkSnipeFilter(1000, 800, 3, false, false)},
                { PokemonId.Beedrill, new HumanWalkSnipeFilter(1000, 800, 3, false, false)},
                 { PokemonId.Chansey, new HumanWalkSnipeFilter(1000, 800, 2, false, false)}  ,
                 { PokemonId.Clefable, new HumanWalkSnipeFilter(1000, 800, 3, false, false)},
                  { PokemonId.Clefable, new HumanWalkSnipeFilter(500, 800, 6, true, true)}
            };
        }
    }
}
