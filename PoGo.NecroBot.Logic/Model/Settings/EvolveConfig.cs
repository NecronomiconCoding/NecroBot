using System.Collections.Generic;
using Newtonsoft.Json;
using POGOProtos.Enums;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Title = "Evolve Config", Description = "Set your evolve settings.", ItemRequired = Required.DisallowNull)]
    public class EvolveConfig
    {
        internal static List<PokemonId> PokemonsToEvolveDefault()
        {
            return new List<PokemonId>
            {
                /*NOTE: keep all the end-of-line commas exept for the last one or an exception will be thrown!
               criteria: 12 candies*/
               PokemonId.Caterpie,
               PokemonId.Weedle,
               PokemonId.Pidgey,
               /*criteria: 25 candies*/
               //PokemonId.Bulbasaur,
               //PokemonId.Charmander,
               //PokemonId.Squirtle,
               PokemonId.Rattata
               //PokemonId.NidoranFemale,
               //PokemonId.NidoranMale,
               //PokemonId.Oddish,
               //PokemonId.Poliwag,
               //PokemonId.Abra,
               //PokemonId.Machop,
               //PokemonId.Bellsprout,
               //PokemonId.Geodude,
               //PokemonId.Gastly,
               //PokemonId.Eevee,
               //PokemonId.Dratini,
               /*criteria: 50 candies commons*/
               //PokemonId.Spearow,
               //PokemonId.Ekans,
               //PokemonId.Zubat,
               //PokemonId.Paras,
               //PokemonId.Venonat,
               //PokemonId.Psyduck,
               //PokemonId.Slowpoke,
               //PokemonId.Doduo,
               //PokemonId.Drowzee,
               //PokemonId.Krabby,
               //PokemonId.Horsea,
               //PokemonId.Goldeen,
               //PokemonId.Staryu
            };
        }
    }
}

