using System.Collections.Generic;
using Newtonsoft.Json;
using POGOProtos.Enums;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Title = "Catch Config", Description = "Set your catch settings.", ItemRequired = Required.DisallowNull)]
    public class CatchConfig
    {
        internal static List<PokemonId> PokemonsToIgnoreDefault()
        {
            return new List<PokemonId>
            {
                //criteria: most common
                PokemonId.Caterpie,
                PokemonId.Weedle,
                PokemonId.Pidgey,
                PokemonId.Rattata,
                PokemonId.Spearow,
                PokemonId.Zubat,
                PokemonId.Doduo
            };
        }

        internal static List<PokemonId> PokemonsToUseMasterballDefault()
        {
            return new List<PokemonId>
            {
                PokemonId.Articuno,
                PokemonId.Zapdos,
                PokemonId.Moltres,
                PokemonId.Mew,
                PokemonId.Mewtwo
            };
        }
    }
}