using System.Collections.Generic;
using Newtonsoft.Json;
using POGOProtos.Enums;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Title = "LevelUp Config", Description = "Set your levelup settings.", ItemRequired = Required.DisallowNull)]
    public class LevelUpConfig
    {
        internal static List<PokemonId> PokemonsToLevelUpDefault()
        {
            return new List<PokemonId>
            {
                //criteria: from SS Tier to A Tier + Regional Exclusive
                PokemonId.Venusaur,
                PokemonId.Charizard,
                PokemonId.Blastoise,
                //PokemonId.Nidoqueen,
                //PokemonId.Nidoking,
                PokemonId.Clefable,
                //PokemonId.Vileplume,
                //PokemonId.Golduck,
                //PokemonId.Arcanine,
                //PokemonId.Poliwrath,
                //PokemonId.Machamp,
                //PokemonId.Victreebel,
                //PokemonId.Golem,
                //PokemonId.Slowbro,
                //PokemonId.Farfetchd,
                PokemonId.Muk,
                //PokemonId.Exeggutor,
                //PokemonId.Lickitung,
                PokemonId.Chansey,
                //PokemonId.Kangaskhan,
                //PokemonId.MrMime,
                //PokemonId.Tauros,
                PokemonId.Gyarados,
                //PokemonId.Lapras,
                PokemonId.Ditto,
                //PokemonId.Vaporeon,
                //PokemonId.Jolteon,
                //PokemonId.Flareon,
                //PokemonId.Porygon,
                PokemonId.Snorlax,
                PokemonId.Articuno,
                PokemonId.Zapdos,
                PokemonId.Moltres,
                PokemonId.Dragonite,
                PokemonId.Mewtwo,
                PokemonId.Mew
            };
        }
    }
}
