#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.DataDumper;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public class DisplayPokemonStatsTask
    {
        public static List<ulong> PokemonId = new List<ulong>();

        public static List<ulong> PokemonIdcp = new List<ulong>();

        public static async Task Execute(ISession session)
        {
            var myPokemonFamilies = await session.Inventory.GetPokemonFamilies();
            var myPokeSettings = await session.Inventory.GetPokemonSettings();

            var highestsPokemonCp =
                await session.Inventory.GetHighestsCp(session.LogicSettings.AmountOfPokemonToDisplayOnStart);

            var pokemonPairedWithStatsCp =
                highestsPokemonCp.Select(
                    pokemon =>
                        Tuple.Create(pokemon, PokemonInfo.CalculateMaxCp(pokemon),
                            PokemonInfo.CalculatePokemonPerfection(pokemon), PokemonInfo.GetLevel(pokemon),
                            PokemonInfo.GetPokemonMove1(pokemon), PokemonInfo.GetPokemonMove2(pokemon),
                            PokemonInfo.GetCandy(pokemon, myPokemonFamilies, myPokeSettings))).ToList();

            var highestsPokemonPerfect =
                await session.Inventory.GetHighestsPerfect(session.LogicSettings.AmountOfPokemonToDisplayOnStart);

            var pokemonPairedWithStatsIv =
                highestsPokemonPerfect.Select(
                    pokemon =>
                        Tuple.Create(pokemon, PokemonInfo.CalculateMaxCp(pokemon),
                            PokemonInfo.CalculatePokemonPerfection(pokemon), PokemonInfo.GetLevel(pokemon),
                            PokemonInfo.GetPokemonMove1(pokemon), PokemonInfo.GetPokemonMove2(pokemon),
                            PokemonInfo.GetCandy(pokemon, myPokemonFamilies, myPokeSettings))).ToList();

            session.EventDispatcher.Send(
                new DisplayHighestsPokemonEvent
                {
                    SortedBy = "CP",
                    PokemonList = pokemonPairedWithStatsCp
                });

            await Task.Delay(500);

            session.EventDispatcher.Send(
                new DisplayHighestsPokemonEvent
                {
                    SortedBy = "IV",
                    PokemonList = pokemonPairedWithStatsIv
                });

            var allPokemonInBag = session.LogicSettings.PrioritizeIvOverCp
                ? await session.Inventory.GetHighestsPerfect(1000)
                : await session.Inventory.GetHighestsCp(1000);
            if (session.LogicSettings.DumpPokemonStats)
            {
                const string dumpFileName = "PokeBagStats";
                try
                {
                    Dumper.ClearDumpFile(session, dumpFileName);
                    Dumper.Dump(session, "pokemonid,pokemonlevel,cp,perfection,stamina,staminamax,move1,move2,candy,ownername,origin,heightm,weightkg,individualattack,individualdefense,individualstamina,cpmultiplier,battlesattacked,battlesdefended,creationtimems,numupgrades,additionalcpmultiplier,favorite,nickname", dumpFileName);
                    foreach (var pokemon in allPokemonInBag)
                    {
                        Dumper.Dump(session,
                            $"{session.Translation.GetPokemonTranslation(pokemon.PokemonId)},{PokemonInfo.GetLevel(pokemon)},{pokemon.Cp},{PokemonInfo.CalculatePokemonPerfection(pokemon)},{pokemon.Stamina},{pokemon.StaminaMax},{pokemon.Move1},{pokemon.Move2},{PokemonInfo.GetCandy(pokemon, myPokemonFamilies, myPokeSettings)},{pokemon.OwnerName},{pokemon.Origin},{pokemon.HeightM},{pokemon.WeightKg},{pokemon.IndividualAttack},{pokemon.IndividualDefense},{pokemon.IndividualStamina},{pokemon.CpMultiplier},{pokemon.BattlesAttacked},{pokemon.BattlesDefended},{pokemon.CreationTimeMs},{pokemon.NumUpgrades},{pokemon.AdditionalCpMultiplier},{pokemon.Favorite},{pokemon.Nickname}",
                            dumpFileName);
                    }
                }
                catch (System.IO.IOException)
                {
                    session.EventDispatcher.Send(new ErrorEvent { Message = $"Could not write {dumpFileName} dump file." });
                }
            }
            await Task.Delay(500);
        }
    }
}