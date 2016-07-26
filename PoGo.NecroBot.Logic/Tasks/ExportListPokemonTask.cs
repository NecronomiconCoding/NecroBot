using Microsoft.CSharp.RuntimeBinder;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Tasks
{
    public static class ExportListPokemonTask
    {
        public static void Execute(Context ctx, StateMachine machine)
        {

            var player = ctx.Profile.PlayerData;
          

            var allPokemon = ctx.Inventory.GetPokemons().Result;
            allPokemon = allPokemon.OrderByDescending(PokemonInfo.CalculateCp).ThenBy(n => n.StaminaMax);

           
            var stats = ctx.Inventory.GetPlayerStats().Result;
            var stat = stats.FirstOrDefault();

            if (stat == null || player == null)
            {
                machine.Fire(
                  new ExportListPokemonEvent
                  {
                      SortedBy = "Cp",
                      ExportSuccessful = false,
                      PokemonCount = 100,
                      Path = "",
                      Message = "Cannot get player info"
                  });
                Thread.Sleep(500);
                return;
            }

            int trainerLevel = stat.Level;
            int[] exp_req = new[] { 0, 1000, 3000, 6000, 10000, 15000, 21000, 28000, 36000, 45000, 55000, 65000, 75000, 85000, 100000, 120000, 140000, 160000, 185000, 210000, 260000, 335000, 435000, 560000, 710000, 900000, 1100000, 1350000, 1650000, 2000000, 2500000, 3000000, 3750000, 4750000, 6000000, 7500000, 9500000, 12000000, 15000000, 20000000 };
            int exp_req_at_level = exp_req[stat.Level - 1];

            var folderPath = Directory.GetCurrentDirectory() + "\\" + $"Profile_{player.Username}";
            var filePath = folderPath + "\\" + $"Pokemon-{DateTime.Today.ToString("yyyy-MM-dd")}-{DateTime.Now.ToString("HH_mm")}.csv";
            DirectoryInfo diProfile = Directory.CreateDirectory(folderPath);
            var IVWeight = ctx.LogicSettings.PokemonBrPrioritizationIVWeightPercentage;
            File.WriteAllText(filePath, $"Pokemon data for {player.Username} @ trainer Level: {trainerLevel}....Battle Rating based on {IVWeight.ToString("00.0")}% IV and {(100-IVWeight).ToString("00.0")}% Cp");

            try
            {
                using (var w = File.AppendText(filePath))
                {
                    w.WriteLine("");
                    w.WriteLine($"PokemonID,NickName,CP,Max CP,CP @ max lvl,Lvl,Battle Rating,Perfection,Move 1, Move 2,HP,Attk,Def,Stamina,Candies, previewLink ----");

                    var myPokemonSettings = ctx.Inventory.GetPokemonSettings().Result;
                    var pokemonSettings = myPokemonSettings.ToList();

                    var myPokemonFamilies = ctx.Inventory.GetPokemonFamilies().Result;
                    var pokemonFamilies = myPokemonFamilies.ToArray();

                    foreach (var pokemon in allPokemon)
                    {
                        var settings = pokemonSettings.Single(x => x.PokemonId == pokemon.PokemonId);
                        var familyCandy = pokemonFamilies.Single(x => settings.FamilyId == x.FamilyId);


                        string toEncode = $"{(int)pokemon.PokemonId}" + "," + trainerLevel + "," + PokemonInfo.GetLevel(pokemon) + "," + pokemon.Cp + "," + pokemon.Stamina;

                        //Generate base64 code to make it viewable here https://jackhumbert.github.io/poke-rater/#MTUwLDIzLDE3LDE5MDIsMTE4
                        var encoded = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(toEncode));

                        w.WriteLine($"{pokemon.PokemonId},{pokemon.Nickname},{pokemon.Cp},{PokemonInfo.CalculateMaxCp(pokemon)},{(int)PokemonInfo.GetMaxCpAtTrainerLevel(pokemon,trainerLevel)},{PokemonInfo.GetLevel(pokemon)},{Math.Round(PokemonInfo.CalculatePokemonBattleRating(pokemon, ctx.LogicSettings.PokemonBrPrioritizationIVWeightPercentage, trainerLevel) * 10) / 10},{Math.Round(PokemonInfo.CalculatePokemonPerfection(pokemon) * 10) / 10},{pokemon.Move1},{pokemon.Move2},{pokemon.Stamina},{pokemon.IndividualAttack},{pokemon.IndividualDefense},{pokemon.IndividualStamina},{familyCandy.Candy},https://jackhumbert.github.io/poke-rater/#{encoded}");
                    }
                    w.Close();



                    machine.Fire(
                            new ExportListPokemonEvent
                            {
                                SortedBy = "Cp",
                                ExportSuccessful = true,
                                PokemonCount = allPokemon.Count(),
                                Path = filePath,
                                  Message = $"Exported List of {allPokemon.Count()} Pokemon to {filePath}"
                            });

                    Thread.Sleep(500);
                }
            }
            catch (IOException ex)
            {

                machine.Fire(
                        new ExportListPokemonEvent
                        {
                            SortedBy = "Cp",
                            ExportSuccessful = true,
                            PokemonCount = allPokemon.Count(),
                            Path = filePath,
                            Message = ex.Message
                        });

                Thread.Sleep(500);
            }
            Thread.Sleep(500);

        }
    }
}
