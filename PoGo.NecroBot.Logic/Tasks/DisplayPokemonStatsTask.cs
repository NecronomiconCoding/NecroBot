#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.DataDumper;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;
using System.Globalization;
using System.Threading;

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

                    string[] data = {
                        "pokemonid",
                        "pokemonlevel",
                        "cp",
                        "perfection",
                        "stamina",
                        "staminamax",
                        "move1",
                        "move2",
                        "candy",
                        "ownername",
                        "origin",
                        "heightm",
                        "weightkg",
                        "individualattack",
                        "individualdefense",
                        "individualstamina",
                        "cpmultiplier",
                        "battlesattacked",
                        "battlesdefended",
                        "creationtimems",
                        "numupgrades",
                        "additionalcpmultiplier",
                        "favorite",
                        "nickname"
                    };
                    Dumper.Dump(session, data, dumpFileName);

                    // set culture to OS default
                    CultureInfo prevCulture = Thread.CurrentThread.CurrentCulture;
                    CultureInfo culture = CultureInfo.CurrentUICulture;
                    Thread.CurrentThread.CurrentCulture = culture;

                    foreach (var pokemon in allPokemonInBag)
                    {
                        string[] pokemonData = {
                            session.Translation.GetPokemonTranslation(pokemon.PokemonId),
                            PokemonInfo.GetLevel(pokemon).ToString(),
                            pokemon.Cp.ToString(),
                            PokemonInfo.CalculatePokemonPerfection(pokemon).ToString(),
                            pokemon.Stamina.ToString(),
                            pokemon.StaminaMax.ToString(),
                            pokemon.Move1.ToString(),
                            pokemon.Move2.ToString(),
                            PokemonInfo.GetCandy(pokemon, myPokemonFamilies, myPokeSettings).ToString(),
                            pokemon.OwnerName,
                            pokemon.Origin.ToString(),
                            pokemon.HeightM.ToString(),
                            pokemon.WeightKg.ToString(),
                            pokemon.IndividualAttack.ToString(),
                            pokemon.IndividualDefense.ToString(),
                            pokemon.IndividualStamina.ToString(),
                            pokemon.CpMultiplier.ToString(),
                            pokemon.BattlesAttacked.ToString(),
                            pokemon.BattlesDefended.ToString(),
                            pokemon.CreationTimeMs.ToString(),
                            pokemon.NumUpgrades.ToString(),
                            pokemon.AdditionalCpMultiplier.ToString(),
                            pokemon.Favorite.ToString(),
                            pokemon.Nickname
                        };
                        Dumper.Dump(session, pokemonData, dumpFileName);
                    }

                    // restore culture
                    Thread.CurrentThread.CurrentCulture = prevCulture;
                }
                catch (System.IO.IOException)
                {
                    session.EventDispatcher.Send(new ErrorEvent { Message = $"Could not write {dumpFileName} dump file." });
                }
            }
        }
    }
}