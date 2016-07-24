#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.GeneratedCode;

#endregion

namespace PokemonGo.RocketAPI.Console
{
    public class Settings : ISettings
    {
        private const string PathItemsToKeep = "Configs\\ItemsToKeep.txt";
        private const string PathPokemonToIgnore = "Configs\\PokemonToIgnore.txt";
        private const string PathPokemonToEvolve = "Configs\\PokemonToEvolve.txt";
        private const string PathPokemonToKeep = "Configs\\PokemonToKeep.txt";

        private static readonly string[] DefaultPokemon =
        {
            "Zubat",
            "Pidgey",
            "Ratata"
        };

        public AuthType AuthType => (AuthType) Enum.Parse(typeof(AuthType), UserSettings.Default.AuthType, true);
        public string PtcUsername => UserSettings.Default.PtcUsername;
        public string PtcPassword => UserSettings.Default.PtcPassword;
        public double DefaultLatitude => UserSettings.Default.DefaultLatitude;
        public double DefaultLongitude => UserSettings.Default.DefaultLongitude;
        public double DefaultAltitude => UserSettings.Default.DefaultAltitude;
        public float KeepMinIVPercentage => UserSettings.Default.KeepMinIVPercentage;
        public int KeepMinCP => UserSettings.Default.KeepMinCP;
        public double WalkingSpeedInKilometerPerHour => UserSettings.Default.WalkingSpeedInKilometerPerHour;
        public bool EvolveAllPokemonWithEnoughCandy => UserSettings.Default.EvolveAllPokemonWithEnoughCandy;
        public bool TransferDuplicatePokemon => UserSettings.Default.TransferDuplicatePokemon;
        public int DelayBetweenPokemonCatch => UserSettings.Default.DelayBetweenPokemonCatch;
        public bool UsePokemonToNotCatchFilter => UserSettings.Default.UsePokemonToNotCatchFilter;
        public int KeepMinDuplicatePokemon => UserSettings.Default.KeepMinDuplicatePokemon;
        public bool PrioritizeIVOverCP => UserSettings.Default.PrioritizeIVOverCP;
        public int MaxTravelDistanceInMeters => UserSettings.Default.MaxTravelDistanceInMeters;

        public ICollection<KeyValuePair<ItemId, int>> ItemsToKeep => this.LoadItemsList(PathItemsToKeep);

        public ICollection<PokemonId> PokemonToEvolve => LoadPokemonList(PathPokemonToEvolve, DefaultPokemon);
        public ICollection<PokemonId> PokemonToKeep => LoadPokemonList(PathPokemonToKeep, DefaultPokemon);
        public ICollection<PokemonId> PokemonToIgnore => LoadPokemonList(PathPokemonToIgnore, DefaultPokemon);

        private ICollection<KeyValuePair<ItemId, int>> LoadItemsList(string filename)
        {
            var collection = new Collection<KeyValuePair<ItemId, int>>();

            try
            {
                var lines = File.ReadAllLines(filename);
                var lineNum = 0;
                foreach(var line in lines)
                {
                    var split = line.Split(',');
                    ItemId itemID;
                    int itemAmount;

                    if(Enum.TryParse(split[0], out itemID) || !int.TryParse(split[1], out itemAmount))
                    {
                        collection.Add(new KeyValuePair<ItemId, int>());
                    }
                    else
                    {
                        Logger.Write($"There was an error while loading the items list. Invalid at line {lineNum}", LogLevel.Error);
                    }

                    lineNum++;
                }
                Logger.Write("Loaded the items-to-keep list!");
            }
            catch(Exception e)
            {
                Logger.Write(e.Message, LogLevel.Error);
            }

            return collection;
        }


        private static ICollection<PokemonId> LoadPokemonList(string filename, string[] defaultContent)
        {
            ICollection<PokemonId> result = new List<PokemonId>();
            Func<string, ICollection<PokemonId>> addPokemonToResult = delegate(string pokemonName)
            {
                PokemonId pokemon;
                if(Enum.TryParse(pokemonName, out pokemon))
                {
                    result.Add(pokemon);
                }
                return result;
            };

            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Configs");

            if(File.Exists(Directory.GetCurrentDirectory() + "\\" + filename))
            {
                Logger.Write($"Loading File: {filename}");

                var content = string.Empty;
                using(var reader = new StreamReader(filename))
                {
                    content = reader.ReadToEnd();
                    reader.Close();
                }

                content = Regex.Replace(content, @"\\/\*(.|\n)*?\*\/", ""); //todo: supposed to remove comment blocks

                var tr = new StringReader(content);

                var pokemonName = tr.ReadLine();
                while(pokemonName != null)
                {
                    addPokemonToResult(pokemonName);
                    pokemonName = tr.ReadLine();
                }
            }
            else
            {
                Logger.Write($"File: {filename} not found, creating new...", LogLevel.Warning);
                using(var w = File.AppendText(Directory.GetCurrentDirectory() + "\\" + filename))
                {
                    Array.ForEach(defaultContent, x => w.WriteLine(x));
                    Array.ForEach(defaultContent, x => addPokemonToResult(x));
                    w.Close();
                }
            }
            return result;
        }
    }
}