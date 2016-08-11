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
                string dumpFileName = "PokemonStats-" + session.Profile.PlayerData.Username;
                try
                {
                    Dumper.ClearDumpFile(session, dumpFileName);
                    string[] dumpColumns = new string[] { "Pokemon ID", "Pokemon Level", "CP", "Perfection", "Stamina", "Stamina Max.", "Move 1", "Move 2", "Candy", "Owner Name", "Origin", "Height (m)", "Weight (kg)", "Individual Attack", "Individual Defense", "Individual Stamina", "CP Multiplier", "Battles Attacked", "Battles Defended", "Creation Time (ms)", "Upgrade Count", "Additional CP Multiplier", "Favorited", "Nickname"};
                    Dumper.Dump(session, string.Join(session.LogicSettings.DumpSeparator, dumpColumns), dumpFileName);
                    foreach (var pokemon in allPokemonInBag)
                    {
                        string[] dumpRow = new string[] { session.Translation.GetPokemonTranslation(pokemon.PokemonId), PokemonInfo.GetLevel(pokemon).ToString(), pokemon.Cp.ToString(), PokemonInfo.CalculatePokemonPerfection(pokemon).ToString().Replace(",","."), pokemon.Stamina.ToString(), pokemon.StaminaMax.ToString(), pokemon.Move1.ToString(), pokemon.Move2.ToString(), PokemonInfo.GetCandy(pokemon, myPokemonFamilies, myPokeSettings).ToString(), pokemon.OwnerName.ToString(), pokemon.Origin.ToString(), pokemon.HeightM.ToString().Replace(",", "."), pokemon.WeightKg.ToString().Replace(",", "."), pokemon.IndividualAttack.ToString(), pokemon.IndividualDefense.ToString(), pokemon.IndividualStamina.ToString(), pokemon.CpMultiplier.ToString().Replace(",", "."), pokemon.BattlesAttacked.ToString(), pokemon.BattlesDefended.ToString(), pokemon.CreationTimeMs.ToString(), pokemon.NumUpgrades.ToString(), pokemon.AdditionalCpMultiplier.ToString(), Convert.ToBoolean(pokemon.Favorite).ToString(), pokemon.Nickname.ToString() == "" ? "-" : pokemon.Nickname.ToString() };
                        Dumper.Dump(session,
                            $"{string.Join(session.LogicSettings.DumpSeparator, dumpRow)}",
                            dumpFileName);
                    }
                }
                catch (System.IO.IOException)
                {
                    session.EventDispatcher.Send(new ErrorEvent { Message = $"Could not write {dumpFileName} dump file." });
                }
            }
        }
    }
}