#region using directives

#region using directives

using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using POGOProtos.Networking.Responses;
using PoGo.NecroBot.Logic.PoGoUtils;
#endregion

// ReSharper disable CyclomaticComplexity

#endregion

namespace PoGo.NecroBot.Logic.Utils
{
    public delegate void StatisticsDirtyDelegate();

    public class Statistics
    {
        private readonly DateTime _initSessionDateTime = DateTime.Now;

        private string _currentLevelInfos;
        private string _playerName;
        public int TotalExperience;
        public int TotalItemsRemoved;
        public int TotalPokemons;
        public int TotalPokemonsTransfered;
        public int TotalStardust;

        public void Dirty(Inventory inventory)
        {
            _currentLevelInfos = GetCurrentInfo(inventory);
            DirtyEvent?.Invoke();
            ExportPokemonToCSV(inventory, $"{_playerName}.csv", ToString());
        }

        public event StatisticsDirtyDelegate DirtyEvent;

        private string FormatRuntime()
        {
            return (DateTime.Now - _initSessionDateTime).ToString(@"dd\.hh\:mm\:ss");
        }

        public string GetCurrentInfo(Inventory inventory)
        {
            var stats = inventory.GetPlayerStats().Result;
            var output = string.Empty;
            var stat = stats.FirstOrDefault();
            if (stat != null)
            {
                var ep = stat.NextLevelXp - stat.PrevLevelXp - (stat.Experience - stat.PrevLevelXp);
                var time = Math.Round(ep/(TotalExperience/GetRuntime()), 2);
                var hours = 0.00;
                var minutes = 0.00;
                if (double.IsInfinity(time) == false && time > 0)
                {
                    time = Convert.ToDouble(TimeSpan.FromHours(time).ToString("h\\.mm"), CultureInfo.InvariantCulture);
                    hours = Math.Truncate(time);
                    minutes = Math.Round((time - hours)*100);
                }

                output =
                    $"{stat.Level} (next level in {hours}h {minutes}m | {stat.Experience - stat.PrevLevelXp - GetXpDiff(stat.Level)}/{stat.NextLevelXp - stat.PrevLevelXp - GetXpDiff(stat.Level)} XP)";
            }
            return output;
        }

        public double GetRuntime()
        {
            return (DateTime.Now - _initSessionDateTime).TotalSeconds/3600;
        }

        public static int GetXpDiff(int level)
        {
            if (level > 0 && level <= 40)
            {
                int[] xpTable =
                {
                    0, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000,
                    10000, 10000, 10000, 10000, 15000, 20000, 20000, 20000, 25000, 25000,
                    50000, 75000, 100000, 125000, 150000, 190000, 200000, 250000, 300000, 350000,
                    500000, 500000, 750000, 1000000, 1250000, 1500000, 2000000, 2500000, 1000000, 1000000
                };
                return xpTable[level - 1];
            }
            return 0;
        }

        public void SetUsername(GetPlayerResponse profile)
        {
            _playerName = profile.PlayerData.Username ?? "";
        }

        public override string ToString()
        {
            return
                $"{_playerName} - Runtime {FormatRuntime()} - Lvl: {_currentLevelInfos} | EXP/H: {TotalExperience / GetRuntime():0} | P/H: {TotalPokemons / GetRuntime():0} | Stardust: {TotalStardust:0} | Transfered: {TotalPokemonsTransfered:0} | Items Recycled: {TotalItemsRemoved:0}";

        }
        public async void ExportPokemonToCSV(Inventory inventory, string filename, string playerstats = "")
        {
            string export_path = Path.Combine("Exports");
            if (!Directory.Exists(export_path))
                Directory.CreateDirectory(export_path);
            if (Directory.Exists(export_path))
            {
                try
                {
                    string pokelist_file = Path.Combine(export_path, filename);
                    if (File.Exists(pokelist_file))
                        File.Delete(pokelist_file);
                    string ls = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;
                    File.WriteAllText(pokelist_file, $"{playerstats.Replace(",", $"{ls}")}");
                    string header = "PokemonID,Name,NickName,CP,MaxCP,Perfection,Attack 1,Attack 2,HP,Attk,Def,Stamina,Familie Candies";
                    using (var w = File.AppendText(pokelist_file))
                    {
                        w.WriteLine("");
                        w.WriteLine($"{ $"{header.Replace(",", $"{ls}")}"}");

                    }

                    var AllPokemonInInv = await inventory.GetPokemons();
                    AllPokemonInInv = AllPokemonInInv.Where(p => p.DeployedFortId == string.Empty).OrderByDescending(p => p.PokemonId);
                    var myPokemonSettings = await inventory.GetPokemonSettings();
                    var pokemonSettings = myPokemonSettings.ToList();
                    var myPokemonFamilies = await inventory.GetPokemonFamilies();
                    var pokemonFamilies = myPokemonFamilies.ToArray();

                    using (var w = File.AppendText(pokelist_file))
                    {
                        w.WriteLine("");
                        foreach (var pokemon in AllPokemonInInv)
                        {
                            var settings = pokemonSettings.Single(x => x.PokemonId == pokemon.PokemonId);
                            var familiecandies = pokemonFamilies.Single(x => settings.FamilyId == x.FamilyId).Candy;
                            string perfection = $"{ PokemonInfo.CalculatePokemonPerfection(pokemon).ToString("0.00") }%";
                            perfection = perfection.Replace(",", ls == "," ? "." : ",");
                            string content_part1 = $"{(int)pokemon.PokemonId},{pokemon.PokemonId},{pokemon.Nickname},{pokemon.Cp},{PokemonInfo.CalculateMaxCp(pokemon).ToString().PadLeft(4, ' ')},";
                            string content_part2 = $",{pokemon.Move1},{pokemon.Move2},{pokemon.Stamina},{pokemon.IndividualAttack},{pokemon.IndividualDefense},{pokemon.IndividualStamina},{familiecandies}";
                            string content = $"{content_part1.Replace(",", $"{ls}")}{perfection}{content_part2.Replace(",", $"{ls}")}";
                            w.WriteLine($"{content}");

                        }
                        w.WriteLine("");
                    }
                    using (var w = File.AppendText(pokelist_file))
                    {
                            List<int> allnums = new List<int>();
                        for (int i = 1; i <= 151; i++)
                            allnums.Add(i);
                        foreach (var pokemon in AllPokemonInInv)
                        {
                            if (allnums.Contains((int)pokemon.PokemonId))
                            {
                                allnums.Remove((int)pokemon.PokemonId);
                            }
                        }
                        w.WriteLine($"Catched: {151 - allnums.Count}");
                        w.WriteLine("Missing:");
                        foreach (int num in allnums)
                            w.WriteLine($"{num}");
                        w.Close();
                    }
                }
                catch
                {

                }
            }
        }
    }
}