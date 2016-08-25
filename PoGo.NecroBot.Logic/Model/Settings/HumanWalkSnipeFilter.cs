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

        public double MaxSpeedUpSpeed { get; set; }
        public bool AllowSpeedUp { get; set; }
        public HumanWalkSnipeFilter(double maxDistance, double maxWalkTimes, int priority, bool catchPokemon, bool spinPokestop, bool allowSpeedUp, double speedUpSpeed)
        {
            this.MaxDistance = maxDistance;
            this.MaxWalkTimes = maxWalkTimes;
            this.Priority = priority;
            this.CatchPokemonWhileWalking = catchPokemon;
            this.SpinPokestopWhileWalking = spinPokestop;
            this.AllowSpeedUp = allowSpeedUp;
            this.MaxSpeedUpSpeed = speedUpSpeed;
        }
        internal static Dictionary<PokemonId, HumanWalkSnipeFilter> Default()
        {
            return new Dictionary<PokemonId, HumanWalkSnipeFilter>
            {
                { PokemonId.Magikarp, new HumanWalkSnipeFilter(300, 200, 10, true, true,false,60.0)},
                { PokemonId.Eevee, new HumanWalkSnipeFilter(500, 200, 10, true, true,false,60.0)},
                { PokemonId.Electabuzz, new HumanWalkSnipeFilter(1500, 600, 2, true, true,false,60.0)},
                { PokemonId.Dragonite, new HumanWalkSnipeFilter(3000, 900, 1, false, false,false,60.0)},
                { PokemonId.Dragonair, new HumanWalkSnipeFilter(3000, 900, 1, false, false,false,60.0)},
                { PokemonId.Dratini, new HumanWalkSnipeFilter(2000, 900, 1, false, false,false,60.0)},
                { PokemonId.Charizard, new HumanWalkSnipeFilter(3000, 900, 1, false, false,false,60.0)},
                { PokemonId.Snorlax, new HumanWalkSnipeFilter(3000, 900, 1, false, false,false,60.0)},
                { PokemonId.Lapras, new HumanWalkSnipeFilter(3000, 900, 1, false, false,false,60.0)} ,
                { PokemonId.Exeggutor, new HumanWalkSnipeFilter(1500, 600, 1, false, true,false,60.0)} ,
                { PokemonId.Vaporeon, new HumanWalkSnipeFilter(1800, 800, 2, false, false,false,60.0)}   ,
                { PokemonId.Flareon, new HumanWalkSnipeFilter(1800, 800, 2, false, false,false,60.0)},
                { PokemonId.Scyther, new HumanWalkSnipeFilter(1000, 800, 3, false, false,false,60.0)},
                { PokemonId.Beedrill, new HumanWalkSnipeFilter(1000, 800, 3, false, false,false,60.0)},
                { PokemonId.Chansey, new HumanWalkSnipeFilter(1000, 800, 2, false, false,false,60.0)}  ,
                { PokemonId.Clefable, new HumanWalkSnipeFilter(1000, 800, 3, false, false,false,60.0)},
                { PokemonId.Golbat, new HumanWalkSnipeFilter(200, 800, 6, true, true,false,60.0)}  ,
                { PokemonId.Jynx, new HumanWalkSnipeFilter(1200, 800, 4, true, true,false,60.0)}  ,
                { PokemonId.Kangaskhan, new HumanWalkSnipeFilter(800, 800, 4, true, true,false,60.0)}  ,
                { PokemonId.Gyarados, new HumanWalkSnipeFilter(1800, 800, 2, false, false,false,60.0)}  ,
            };
        }
    }
}
