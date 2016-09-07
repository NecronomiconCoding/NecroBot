using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using POGOProtos.Enums;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Description = "", ItemRequired = Required.DisallowNull)] //Dont set Title
    public class HumanWalkSnipeFilter
    {
        [DefaultValue(300.0)]
        [Range(0, 9999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 1)]
        public double MaxDistance { get; set; }

        [DefaultValue(1)]
        [Range(0, 9999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 2)]
        public int Priority { get; set; }

        [DefaultValue(200.0)]
        [Range(0, 9999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 3)]
        public double MaxWalkTimes { get; set; }

        [DefaultValue(true)]
        [Range(0, 9999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 4)]
        public bool CatchPokemonWhileWalking { get; set; }

        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 5)]
        public bool SpinPokestopWhileWalking { get; set; }

        [DefaultValue(60.0)]
        [Range(0, 999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 6)]
        public double MaxSpeedUpSpeed { get; set; }

        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 7)]
        public bool AllowSpeedUp { get; set; }

        [DefaultValue(10000)]
        [Range(0, 999999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 8)]
        public int DelayTimeAtDestination { get; set; }

        public HumanWalkSnipeFilter(double maxDistance, double maxWalkTimes, int priority, bool catchPokemon, bool spinPokestop, bool allowSpeedUp, double speedUpSpeed, int delay = 10)
        {
            this.MaxDistance = maxDistance;
            this.MaxWalkTimes = maxWalkTimes;
            this.Priority = priority;
            this.CatchPokemonWhileWalking = catchPokemon;
            this.SpinPokestopWhileWalking = spinPokestop;
            this.AllowSpeedUp = allowSpeedUp;
            this.MaxSpeedUpSpeed = speedUpSpeed;
            this.DelayTimeAtDestination = delay;
        }

        internal static Dictionary<PokemonId, HumanWalkSnipeFilter> Default()
        {
            return new Dictionary<PokemonId, HumanWalkSnipeFilter>
            {

                //http://fraghero.com/heres-the-full-pokemon-go-pokemon-list-most-common-to-the-rarest/

                { PokemonId.Magikarp, new HumanWalkSnipeFilter(300, 200, 10, true, true,false,60.0)},
                { PokemonId.Eevee, new HumanWalkSnipeFilter(500, 200, 10, true, true,false,60.0)},
                { PokemonId.Electabuzz, new HumanWalkSnipeFilter(1500, 700, 2, true, true,false,60.0)},
                { PokemonId.Dragonite, new HumanWalkSnipeFilter(3000, 900, 1, false, false,false,60.0)},
                { PokemonId.Dragonair, new HumanWalkSnipeFilter(3000, 900, 1, false, false,false,60.0)},
                { PokemonId.Dratini, new HumanWalkSnipeFilter(2000, 900, 1, false, false,false,60.0)},
                { PokemonId.Charizard, new HumanWalkSnipeFilter(3000, 900, 1, false, false,false,60.0)},
                { PokemonId.Snorlax, new HumanWalkSnipeFilter(3000, 900, 1, false, false,false,60.0)},
                { PokemonId.Lapras, new HumanWalkSnipeFilter(3000, 900, 1, false, false,false,60.0)} ,
                { PokemonId.Exeggutor, new HumanWalkSnipeFilter(1500, 600, 1, false, true,false,60.0)} ,
                { PokemonId.Vaporeon, new HumanWalkSnipeFilter(1800, 800, 2, false, false,false,60.0)}   ,
                { PokemonId.Lickitung, new HumanWalkSnipeFilter(1800, 800, 2, false, false,false,60.0)}   ,
                { PokemonId.Flareon, new HumanWalkSnipeFilter(1800, 800, 2, false, false,false,60.0)},
                { PokemonId.Scyther, new HumanWalkSnipeFilter(1000, 800, 3, false, false,false,60.0)},
                { PokemonId.Beedrill, new HumanWalkSnipeFilter(1000, 800, 3, false, false,false,60.0)},
                { PokemonId.Chansey, new HumanWalkSnipeFilter(1500, 800, 2, false, false,false,60.0)},
                { PokemonId.Hitmonlee, new HumanWalkSnipeFilter(1500, 800, 2, false, false,false,60.0)}  ,
                { PokemonId.Machamp, new HumanWalkSnipeFilter(1500, 800, 2, false, false,false,60.0)}  ,
                { PokemonId.Ninetales, new HumanWalkSnipeFilter(1500, 800, 2, false, false,false,60.0)}  ,
                { PokemonId.Jolteon, new HumanWalkSnipeFilter(1200, 800, 2, false, false,false,60.0)}  ,
                { PokemonId.Poliwhirl, new HumanWalkSnipeFilter(1200, 800, 2, false, false,false,60.0)}  ,
                { PokemonId.Rapidash, new HumanWalkSnipeFilter(1500, 800, 2, false, false,false,60.0)}  ,
                { PokemonId.Cloyster, new HumanWalkSnipeFilter(1200, 800, 2, false, false,false,60.0)}  ,
                { PokemonId.Dodrio, new HumanWalkSnipeFilter(1200, 800, 2, false, false,false,60.0)}  ,
                { PokemonId.Clefable, new HumanWalkSnipeFilter(1000, 800, 3, false, false,false,60.0)},
                { PokemonId.Golbat, new HumanWalkSnipeFilter(200, 800, 6, true, true,false,60.0)}  ,
                { PokemonId.Jynx, new HumanWalkSnipeFilter(1200, 800, 4, true, true,false,60.0)}  ,
                { PokemonId.Rhydon, new HumanWalkSnipeFilter(1200, 800, 4, true, true,false,60.0)}  ,
                { PokemonId.Kangaskhan, new HumanWalkSnipeFilter(800, 800, 4, true, true,false,60.0)}  ,
                { PokemonId.Wigglytuff, new HumanWalkSnipeFilter(1250, 800, 4, true, true,false,60.0)}  ,
                { PokemonId.Gyarados, new HumanWalkSnipeFilter(1800, 800, 2, false, false,false,60.0)}  ,
                { PokemonId.Dewgong, new HumanWalkSnipeFilter(1800, 800, 2, false, false,false,60.0)}  ,
                { PokemonId.Blastoise, new HumanWalkSnipeFilter(3000, 900, 1, false, false,true,60.0)}  ,
                { PokemonId.Venusaur, new HumanWalkSnipeFilter(3000, 900, 1, false, false,true,60.0)}  ,
                { PokemonId.Bulbasaur, new HumanWalkSnipeFilter(500, 300, 3, true, true,false,60.0)},
                { PokemonId.Charmander, new HumanWalkSnipeFilter(500, 300, 3, true, true,false,60.0)},
                { PokemonId.Squirtle, new HumanWalkSnipeFilter(500, 300, 3, true, true,false,60.0)},
                { PokemonId.Omanyte, new HumanWalkSnipeFilter(1500, 300, 3, true, true,false,60.0)},
                { PokemonId.Marowak, new HumanWalkSnipeFilter(1500, 300, 3, true, true,false,60.0)},
                { PokemonId.Venomoth, new HumanWalkSnipeFilter(1500, 300, 3, true, true,false,60.0)},
                { PokemonId.Vileplume, new HumanWalkSnipeFilter(1500, 300, 3, true, true,false,60.0)},
            };
        }
    }
}
