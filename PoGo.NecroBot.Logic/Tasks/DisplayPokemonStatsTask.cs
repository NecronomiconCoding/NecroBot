#region using directives

using System;
using System.Linq;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.DataDumper;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;
using System.Collections.Generic;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public class DisplayPokemonStatsTask
    {
        public static List<ulong> PokemonID = new List<ulong>();
 

        public static List<ulong> PokemonIDCP = new List<ulong>();
        public static async Task Execute(ISession session)
        {
            var highestsPokemonCp = await session.Inventory.GetHighestsCp(session.LogicSettings.AmountOfPokemonToDisplayOnStart);
            var highestsPokemonCpForUpgrade = await session.Inventory.GetHighestsCp(50);
            var highestsPokemonIVForUpgrade = await session.Inventory.GetHighestsPerfect(50);
            var pokemonPairedWithStatsCp = highestsPokemonCp.Select(pokemon => Tuple.Create(pokemon, PokemonInfo.CalculateMaxCp(pokemon), PokemonInfo.CalculatePokemonPerfection(pokemon), PokemonInfo.GetLevel(pokemon), PokemonInfo.GetPokemonMove1(pokemon), PokemonInfo.GetPokemonMove2(pokemon))).ToList();
            var pokemonPairedWithStatsCpForUpgrade = highestsPokemonCpForUpgrade.Select(pokemon => Tuple.Create(pokemon, PokemonInfo.CalculateMaxCp(pokemon), PokemonInfo.CalculatePokemonPerfection(pokemon), PokemonInfo.GetLevel(pokemon), PokemonInfo.GetPokemonMove1(pokemon), PokemonInfo.GetPokemonMove2(pokemon))).ToList();
            var highestsPokemonPerfect =
                await session.Inventory.GetHighestsPerfect(session.LogicSettings.AmountOfPokemonToDisplayOnStart);
          
            var pokemonPairedWithStatsIv = highestsPokemonPerfect.Select(pokemon => Tuple.Create(pokemon, PokemonInfo.CalculateMaxCp(pokemon), PokemonInfo.CalculatePokemonPerfection(pokemon), PokemonInfo.GetLevel(pokemon), PokemonInfo.GetPokemonMove1(pokemon), PokemonInfo.GetPokemonMove2(pokemon))).ToList();
            var pokemonPairedWithStatsIvForUpgrade = highestsPokemonIVForUpgrade.Select(pokemon => Tuple.Create(pokemon, PokemonInfo.CalculateMaxCp(pokemon), PokemonInfo.CalculatePokemonPerfection(pokemon), PokemonInfo.GetLevel(pokemon), PokemonInfo.GetPokemonMove1(pokemon), PokemonInfo.GetPokemonMove2(pokemon))).ToList();

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

            for (int i = 0; i < pokemonPairedWithStatsIvForUpgrade.Count; i++)
            {
                var dgdfs = pokemonPairedWithStatsIvForUpgrade[i].ToString();
                
                string[] tokens = dgdfs.Split(new[] {"id"}, StringSplitOptions.None);
                string[] splitone = tokens[1].Split('"');
                var IV = session.Inventory.GetPerfect(pokemonPairedWithStatsIvForUpgrade[i].Item1);
                if (IV.Result > session.LogicSettings.UpgradePokemonIVMinimum)
                {
                    
                    PokemonID.Add(ulong.Parse(splitone[2]));
                }
               
            }
            for (int i = 0; i < pokemonPairedWithStatsCpForUpgrade.Count; i++)
            {
                var dgdfs = pokemonPairedWithStatsCpForUpgrade[i].ToString();
                string[] tokens = dgdfs.Split(new[] {"id"}, StringSplitOptions.None);
                string[] splitone = tokens[1].Split('"');
                string[] tokensSplit = tokens[1].Split(new[] { "cp" }, StringSplitOptions.None);
                string[] TokenSplitAgain = tokensSplit[1].Split((' '));
                string[] TokenSplitAgain2 = TokenSplitAgain[1].Split((','));
                if (float.Parse(TokenSplitAgain2[0]) > session.LogicSettings.UpgradePokemonCPMinimum)
                {
                    PokemonIDCP.Add(ulong.Parse(splitone[2]));
                }
              
                
            }
            var allPokemonInBag = session.LogicSettings.PrioritizeIvOverCp ? await session.Inventory.GetHighestsPerfect(1000) : await session.Inventory.GetHighestsCp(1000);
            if (session.LogicSettings.DumpPokemonStats)
            {
                const string dumpFileName = "PokeBagStats";
                Dumper.ClearDumpFile(session, dumpFileName);
                foreach (var pokemon in allPokemonInBag)
                {
                    Dumper.Dump(session, $"NAME: {pokemon.PokemonId.ToString().PadRight(16, ' ')}Lvl: { PokemonInfo.GetLevel(pokemon).ToString("00")}\t\tCP: { pokemon.Cp.ToString().PadRight(8, ' ')}\t\t IV: { PokemonInfo.CalculatePokemonPerfection(pokemon).ToString("0.00")}%\t\t\tMOVE1: { pokemon.Move1.ToString()}\t\t\tMOVE2: { pokemon.Move2.ToString()}", dumpFileName);
                }

            }
            await Task.Delay(500);
        }
    }
}
