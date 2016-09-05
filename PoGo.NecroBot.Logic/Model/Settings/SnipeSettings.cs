using System.Collections.Generic;
using Newtonsoft.Json;
using POGOProtos.Enums;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Title = "Snipe Settings", Description = "", ItemRequired = Required.DisallowNull)]
    public class SnipeSettings
    {
        public SnipeSettings()
        {
        }

        public SnipeSettings(List<Location> locations, List<PokemonId> pokemon)
        {
            Locations = locations;
            Pokemon = pokemon;
        }

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore, Order = 1)]
        public List<Location> Locations = new List<Location>();

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore, Order = 2)]
        public List<PokemonId> Pokemon = new List<PokemonId>();

        internal static SnipeSettings Default()
        {
            return new SnipeSettings
            {
                Locations = new List<Location>
                {
                    new Location(38.55680748646112, -121.2383794784546), //Dratini Spot
                    new Location(-33.85901900, 151.21309800), //Magikarp Spot
                    new Location(47.5014969, -122.0959568), //Eevee Spot
                    new Location(51.5025343, -0.2055027) //Charmender Spot
                },
                Pokemon = new List<PokemonId>
                {
                    PokemonId.Venusaur,
                    PokemonId.Charizard,
                    PokemonId.Blastoise,
                    PokemonId.Beedrill,
                    PokemonId.Raichu,
                    PokemonId.Sandslash,
                    PokemonId.Nidoking,
                    PokemonId.Nidoqueen,
                    PokemonId.Clefable,
                    PokemonId.Ninetales,
                    PokemonId.Golbat,
                    PokemonId.Vileplume,
                    PokemonId.Golduck,
                    PokemonId.Primeape,
                    PokemonId.Arcanine,
                    PokemonId.Poliwrath,
                    PokemonId.Alakazam,
                    PokemonId.Machamp,
                    PokemonId.Golem,
                    PokemonId.Rapidash,
                    PokemonId.Slowbro,
                    //PokemonId.Farfetchd,
                    PokemonId.Muk,
                    PokemonId.Cloyster,
                    PokemonId.Gengar,
                    PokemonId.Exeggutor,
                    PokemonId.Marowak,
                    PokemonId.Hitmonchan,
                    PokemonId.Lickitung,
                    PokemonId.Rhydon,
                    PokemonId.Chansey,
                    //PokemonId.Kangaskhan,
                    PokemonId.Starmie,
                    //PokemonId.MrMime,
                    PokemonId.Scyther,
                    PokemonId.Magmar,
                    PokemonId.Electabuzz,
                    PokemonId.Jynx,
                    PokemonId.Gyarados,
                    PokemonId.Lapras,
                    PokemonId.Ditto,
                    PokemonId.Vaporeon,
                    PokemonId.Jolteon,
                    PokemonId.Flareon,
                    PokemonId.Porygon,
                    PokemonId.Kabutops,
                    PokemonId.Aerodactyl,
                    PokemonId.Snorlax,
                    PokemonId.Articuno,
                    PokemonId.Zapdos,
                    PokemonId.Moltres,
                    PokemonId.Dragonite,
                    PokemonId.Mewtwo,
                    PokemonId.Mew
                }
            };
        }
    }
}