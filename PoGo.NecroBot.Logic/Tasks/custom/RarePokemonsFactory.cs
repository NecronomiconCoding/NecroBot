using POGOProtos.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Tasks.custom
{
    public class RarePokemonsFactory
    {
        public static List<PokemonId> createRarePokemonList()
        {
            List<PokemonId> rarePokemonIds = new List<PokemonId>()
            {
                PokemonId.Venusaur,
                PokemonId.Charizard,
                PokemonId.Blastoise,
                PokemonId.Raichu,
                PokemonId.Nidoqueen,
                PokemonId.Nidoking,
                PokemonId.Ninetales,
                PokemonId.Wigglytuff,
                PokemonId.Dugtrio,
                PokemonId.Arcanine,
                PokemonId.Alakazam,
                PokemonId.Rapidash,
                PokemonId.Slowbro,
                PokemonId.Magneton,
                PokemonId.Farfetchd,
                PokemonId.Dewgong,
                PokemonId.Grimer,
                PokemonId.Muk,
                PokemonId.Haunter,
                PokemonId.Gengar,
                PokemonId.Exeggutor,
                PokemonId.Marowak,
                PokemonId.Hitmonlee,
                PokemonId.Hitmonchan,
                PokemonId.Lickitung,
                PokemonId.Chansey,
                PokemonId.Kangaskhan,
                PokemonId.MrMime,
                PokemonId.Gyarados,
                PokemonId.Lapras,
                PokemonId.Ditto,
                PokemonId.Vaporeon,
                PokemonId.Jolteon,
                PokemonId.Flareon,
                PokemonId.Porygon,
                PokemonId.Snorlax,
                PokemonId.Articuno,
                PokemonId.Zapdos,
                PokemonId.Moltres,
                PokemonId.Dratini,
                PokemonId.Dragonair,
                PokemonId.Dragonite,
                PokemonId.Mewtwo,
                PokemonId.Mew
            };
            return rarePokemonIds;
        }
    }
}
