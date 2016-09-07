using PoGo.NecroBot.Logic.Model;
using POGOProtos.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Common
{
    public static class PokemonGradeHelper
    {
        public static PokemonGrades GetPokemonGrade(PokemonId id)
        {
            var first = pokemonByGrades.FirstOrDefault(p => p.Value.Contains(id));
            return first.Key;
        }
        private static Dictionary<PokemonGrades, List<PokemonId>> pokemonByGrades = new Dictionary<PokemonGrades, List<PokemonId>>()
        {
            {
                PokemonGrades.VeryCommon, new List<PokemonId>() {
                                       PokemonId.Caterpie,
                                       PokemonId.Weedle,
                                       PokemonId.Pidgey ,
                                       PokemonId.Rattata  ,
                                       PokemonId.Ekans    ,
                                       PokemonId.Sandshrew ,
                                       PokemonId.NidoranFemale,
                                       PokemonId.NidoranMale   ,
                                       PokemonId.Zubat          ,
                                       PokemonId.Oddish          ,
                                       PokemonId.Paras            ,
                                       PokemonId.Venonat              ,
                                       PokemonId.Mankey     ,
                                        PokemonId.Poliwag    ,
                                        PokemonId.Machop      ,
                                        PokemonId.Bellsprout   ,
                                        PokemonId.Geodude       ,
                                        PokemonId.Slowpoke,
                                         PokemonId.Magnemite    ,
                                        PokemonId.Gastly      ,
                                        PokemonId.Krabby,
                                        PokemonId.Voltorb       ,
                                        PokemonId.Goldeen,
                                        PokemonId.Eevee,
                                        PokemonId.Magikarp
                } }
                ,
                    {
                    PokemonGrades.Common, new List<PokemonId> {
                       PokemonId.Bulbasaur  ,
                       PokemonId.Charmander ,
                       PokemonId.Squirtle   ,
                       PokemonId.Kakuna     ,
                       PokemonId.Pidgeotto   ,
                       PokemonId.Raticate   ,
                       PokemonId.Spearow    ,
                       PokemonId.Arbok      ,
                       PokemonId.Pikachu    ,
                       PokemonId.Sandslash  ,
                       PokemonId.Clefable   ,
                       PokemonId.Jigglypuff ,
                       PokemonId.Golbat     ,
                       PokemonId.Diglett     ,
                       PokemonId.Persian    ,
                       PokemonId.Psyduck    ,
                       PokemonId.Growlithe  ,
                       PokemonId.Abra       ,
                       PokemonId.Machoke    ,
                       PokemonId.Graveler   ,
                       PokemonId.Ponyta     ,
                       PokemonId.Magneton   ,
                       PokemonId.Doduo      ,
                       PokemonId.Seel       ,
                       PokemonId.Grimer     ,
                       PokemonId.Shellder   ,
                       PokemonId.Haunter    ,
                       PokemonId.Electrode  ,
                       PokemonId.Exeggcute  ,
                       PokemonId.Cubone     ,
                       PokemonId.Hitmonlee  ,
                       PokemonId.Koffing    ,
                       PokemonId.Rhyhorn    ,
                       PokemonId.Horsea     ,
                       PokemonId.Staryu     ,
                       PokemonId.Jynx       ,

                    }

                    },
            {
             PokemonGrades.Popular, new List<PokemonId>()
             {

                 PokemonId.Dratini,
                 PokemonId.Butterfree,
                 PokemonId.Spearow,
                 PokemonId.Nidorina,
                 PokemonId.Nidorino,
                 PokemonId.Ninetales,
                 PokemonId.Wigglytuff,
                 PokemonId.Gloom,
                 PokemonId.Parasect,
                 PokemonId.Golduck     ,
              PokemonId.Primeape    ,
              PokemonId.Chansey     ,
              PokemonId.Poliwhirl   ,
              PokemonId.Kadabra     ,
              PokemonId.Machamp     ,
              PokemonId.Tentacruel  ,
              PokemonId.Golem       ,
              PokemonId.Kabuto      ,
              PokemonId.Dodrio      ,
              PokemonId.Cloyster    ,
              PokemonId.Scyther     ,
              PokemonId.Hypno,
              PokemonId.Seadra      ,
              PokemonId.Hitmonchan  ,
              PokemonId.Lickitung   ,
              PokemonId.Weezing     ,
                              PokemonId.Seaking,
                              PokemonId.Starmie

             }
            }   ,
            {
                PokemonGrades.Rare, new List<PokemonId>()
                {
                    PokemonId.Beedrill,
                    PokemonId.Pidgeot,
                   PokemonId. Pinsir    ,
                   PokemonId.Snorlax    ,
                   PokemonId.Slowbro    ,
                   PokemonId.MrMime   ,
                   PokemonId.Farfetchd ,
                   PokemonId.Onix       ,
                   PokemonId.Jolteon    ,
                   PokemonId.Flareon    ,
                   PokemonId.Magmar     ,
                   PokemonId.Kingler    ,
                   PokemonId.Rhydon     ,
                   PokemonId.Rapidash   ,
                   PokemonId.Arcanine   ,
                   PokemonId.Muk        ,
                   PokemonId.Exeggutor,
                   PokemonId.Tangela    ,
                }
            } ,
            {
                PokemonGrades.VeryRare, new List<PokemonId>()
                {
                   PokemonId.Gyarados         ,
                   PokemonId.Lapras           ,
                   PokemonId.Vaporeon         ,
                   PokemonId.Kabutops,
                   PokemonId.Dragonair        ,
                   PokemonId.Dragonite        ,
                   PokemonId.Raichu           ,
                   PokemonId.Nidoqueen        ,
                   PokemonId.Nidoking         ,
                   PokemonId.Vileplume        ,
                   PokemonId.Venomoth         ,
                   PokemonId.Poliwrath        ,
                   PokemonId.Alakazam         ,
                   PokemonId.Electabuzz       ,
                   PokemonId.Victreebel       ,
                   PokemonId.Kangaskhan       ,
                   PokemonId.Dewgong          ,
                   PokemonId.Marowak          ,
                   PokemonId.Gengar           ,
                }
            }  ,
            {
                PokemonGrades.Epic,  new List<PokemonId>()
                {
                 PokemonId.Venusaur          ,
                 PokemonId.Charmeleon       ,
                 PokemonId.Wartortle        ,
                 PokemonId.Porygon          ,
                 PokemonId.Omanyte          ,
                 PokemonId.Aerodactyl       ,

                }
            }       ,
            {
                PokemonGrades.Legendary, new List<PokemonId>()
                {
                    PokemonId.Ditto        ,
                    PokemonId.Articuno    ,
                    PokemonId.Zapdos      ,
                    PokemonId.Moltres     ,
                    PokemonId.Mewtwo      ,
                }
            }
        };
    }
}
