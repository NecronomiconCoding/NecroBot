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
            var highestsPokemonCpForUpgrade = await session.Inventory.GetHighestsCp(50);
            var highestsPokemonIvForUpgrade = await session.Inventory.GetHighestsPerfect(50);
            var pokemonPairedWithStatsCp =
                highestsPokemonCp.Select(
                    pokemon =>
                        Tuple.Create(pokemon, PokemonInfo.CalculateMaxCp(pokemon),
                            PokemonInfo.CalculatePokemonPerfection(pokemon), PokemonInfo.GetLevel(pokemon),
                            PokemonInfo.GetPokemonMove1(pokemon), PokemonInfo.GetPokemonMove2(pokemon),
                            PokemonInfo.GetCandy(pokemon, myPokemonFamilies, myPokeSettings))).ToList();
            var pokemonPairedWithStatsCpForUpgrade =
                highestsPokemonCpForUpgrade.Select(
                    pokemon =>
                        Tuple.Create(pokemon, PokemonInfo.CalculateMaxCp(pokemon),
                            PokemonInfo.CalculatePokemonPerfection(pokemon), PokemonInfo.GetLevel(pokemon),
                            PokemonInfo.GetPokemonMove1(pokemon), PokemonInfo.GetPokemonMove2(pokemon))).ToList();
            var highestsPokemonPerfect =
                await session.Inventory.GetHighestsPerfect(session.LogicSettings.AmountOfPokemonToDisplayOnStart);

            var pokemonPairedWithStatsIv =
                highestsPokemonPerfect.Select(
                    pokemon =>
                        Tuple.Create(pokemon, PokemonInfo.CalculateMaxCp(pokemon),
                            PokemonInfo.CalculatePokemonPerfection(pokemon), PokemonInfo.GetLevel(pokemon),
                            PokemonInfo.GetPokemonMove1(pokemon), PokemonInfo.GetPokemonMove2(pokemon),
                            PokemonInfo.GetCandy(pokemon, myPokemonFamilies, myPokeSettings))).ToList();
            var pokemonPairedWithStatsIvForUpgrade =
                highestsPokemonIvForUpgrade.Select(
                    pokemon =>
                        Tuple.Create(pokemon, PokemonInfo.CalculateMaxCp(pokemon),
                            PokemonInfo.CalculatePokemonPerfection(pokemon), PokemonInfo.GetLevel(pokemon),
                            PokemonInfo.GetPokemonMove1(pokemon), PokemonInfo.GetPokemonMove2(pokemon))).ToList();

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

            foreach (var pokemon in pokemonPairedWithStatsIvForUpgrade)
            {
                var dgdfs = pokemon.ToString();

                var tokens = dgdfs.Split(new[] {"id"}, StringSplitOptions.None);
                var splitone = tokens[1].Split('"');
                var iv = session.Inventory.GetPerfect(pokemon.Item1);
                if (iv >= session.LogicSettings.UpgradePokemonIvMinimum)
                {
                    PokemonId.Add(ulong.Parse(splitone[2]));
                }
            }
            foreach (var t in pokemonPairedWithStatsCpForUpgrade)
            {
                var dgdfs = t.ToString();
                var tokens = dgdfs.Split(new[] {"id"}, StringSplitOptions.None);
                var splitone = tokens[1].Split('"');
                var tokensSplit = tokens[1].Split(new[] {"cp"}, StringSplitOptions.None);
                var tokenSplitAgain = tokensSplit[1].Split(' ');
                var tokenSplitAgain2 = tokenSplitAgain[1].Split(',');
                if (float.Parse(tokenSplitAgain2[0]) >= session.LogicSettings.UpgradePokemonCpMinimum)
                {
                    PokemonIdcp.Add(ulong.Parse(splitone[2]));
                }
            }
            var allPokemonInBag = session.LogicSettings.PrioritizeIvOverCp
                ? await session.Inventory.GetHighestsPerfect(1000)
                : await session.Inventory.GetHighestsCp(1000);
            if (session.LogicSettings.DumpPokemonStats)
            {
                const string dumpFileName = "PokeBagStats";
                Dumper.ClearDumpFile(session, dumpFileName);
                Dumper.Dump(session, "pokemonid,pokemonlevel,cp,perfection,stamina,staminamax,move1,move2,candy,ownername,origin,heightm,weightkg,individualattack,individualdefense,individualstamina,cpmultiplier,battlesattacked,battlesdefended,creationtimems,numupgrades,additionalcpmultiplier,favorite,nickname", dumpFileName);
                foreach (var pokemon in allPokemonInBag)
                {
                    Dumper.Dump(session,
                        $"{pokemon.PokemonId},{PokemonInfo.GetLevel(pokemon)},{pokemon.Cp},{PokemonInfo.CalculatePokemonPerfection(pokemon)},{pokemon.Stamina},{pokemon.StaminaMax},{pokemon.Move1},{pokemon.Move2},{PokemonInfo.GetCandy(pokemon, myPokemonFamilies, myPokeSettings)},{pokemon.OwnerName},{pokemon.Origin},{pokemon.HeightM},{pokemon.WeightKg},{pokemon.IndividualAttack},{pokemon.IndividualDefense},{pokemon.IndividualStamina},{pokemon.CpMultiplier},{pokemon.BattlesAttacked},{pokemon.BattlesDefended},{pokemon.CreationTimeMs},{pokemon.NumUpgrades},{pokemon.AdditionalCpMultiplier},{pokemon.Favorite},{pokemon.Nickname}",
                        dumpFileName);
                }
            }
            await Task.Delay(500);
        }
    }
}