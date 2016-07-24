#region using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.GeneratedCode;

#endregion

namespace PokemonGo.RocketAPI.Console
{
    public class Settings : ISettings
    {
        private ICollection<PokemonId> _pokemonsNotToCatch;
        private ICollection<PokemonId> _pokemonsNotToTransfer;
        private ICollection<PokemonId> _pokemonsToEvolve;

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
        public string GPXFile => UserSettings.Default.GPXFile;
        public bool UseGPXPathing => UserSettings.Default.UseGPXPathing;
        public bool useLuckyEggsWhileEvolving => UserSettings.Default.useLuckyEggsWhileEvolving;
        public bool EvolveAllPokemonAboveIV => UserSettings.Default.EvolveAllPokemonAboveIV;
        public float EvolveAboveIVValue => UserSettings.Default.EvolveAboveIVValue;

        //Type and amount to keep
        public ICollection<KeyValuePair<ItemId, int>> ItemRecycleFilter => new[]
        {
            new KeyValuePair<ItemId, int>(ItemId.ItemUnknown, 0),
            new KeyValuePair<ItemId, int>(ItemId.ItemPokeBall, 25),
            new KeyValuePair<ItemId, int>(ItemId.ItemGreatBall, 50),
            new KeyValuePair<ItemId, int>(ItemId.ItemUltraBall, 75),
            new KeyValuePair<ItemId, int>(ItemId.ItemMasterBall, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemPotion, 0),
            new KeyValuePair<ItemId, int>(ItemId.ItemSuperPotion, 25),
            new KeyValuePair<ItemId, int>(ItemId.ItemHyperPotion, 50),
            new KeyValuePair<ItemId, int>(ItemId.ItemMaxPotion, 75),
            new KeyValuePair<ItemId, int>(ItemId.ItemRevive, 25),
            new KeyValuePair<ItemId, int>(ItemId.ItemMaxRevive, 50),
            new KeyValuePair<ItemId, int>(ItemId.ItemLuckyEgg, 200),
            new KeyValuePair<ItemId, int>(ItemId.ItemIncenseOrdinary, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemIncenseSpicy, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemIncenseCool, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemIncenseFloral, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemTroyDisk, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemXAttack, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemXDefense, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemXMiracle, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemRazzBerry, 50),
            new KeyValuePair<ItemId, int>(ItemId.ItemBlukBerry, 10),
            new KeyValuePair<ItemId, int>(ItemId.ItemNanabBerry, 10),
            new KeyValuePair<ItemId, int>(ItemId.ItemWeparBerry, 30),
            new KeyValuePair<ItemId, int>(ItemId.ItemPinapBerry, 30),
            new KeyValuePair<ItemId, int>(ItemId.ItemSpecialCamera, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemIncubatorBasicUnlimited, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemIncubatorBasic, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemPokemonStorageUpgrade, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemItemStorageUpgrade, 100)
        };

        public ICollection<PokemonId> PokemonsToEvolve
        {
            get
            {
                //Type of pokemons to evolve
                var defaultText = new[] {"Zubat", "Pidgey", "Rattata"};
                _pokemonsToEvolve = _pokemonsToEvolve ??
                                    LoadPokemonList("Configs\\ConfigPokemonsToEvolve.txt", defaultText);
                return _pokemonsToEvolve;
            }
        }

        public ICollection<PokemonId> PokemonsNotToTransfer
        {
            get
            {
                //Type of pokemons not to transfer
                var defaultText = new[] {"Dragonite", "Charizard", "Zapdos", "Snorlax", "Alakhazam", "Mew", "Mewtwo"};
                _pokemonsNotToTransfer = _pokemonsNotToTransfer ??
                                         LoadPokemonList("Configs\\ConfigPokemonsToKeep.txt", defaultText);
                return _pokemonsNotToTransfer;
            }
        }

        //Do not catch those
        public ICollection<PokemonId> PokemonsNotToCatch
        {
            get
            {
                //Type of pokemons not to catch
                var defaultText = new[] {"Zubat", "Pidgey", "Rattata"};
                _pokemonsNotToCatch = _pokemonsNotToCatch ??
                                      LoadPokemonList("Configs\\ConfigPokemonsNotToCatch.txt", defaultText);
                return _pokemonsNotToCatch;
            }
        }

        private static ICollection<PokemonId> LoadPokemonList(string filename, string[] defaultContent)
        {
            ICollection<PokemonId> result = new List<PokemonId>();
            Func<string, ICollection<PokemonId>> addPokemonToResult = delegate(string pokemonName)
            {
                PokemonId pokemon;
                if (Enum.TryParse(pokemonName, out pokemon))
                {
                    result.Add(pokemon);
                }
                return result;
            };

            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Configs");

            if (File.Exists(Directory.GetCurrentDirectory() + "\\" + filename))
            {
                Logger.Write($"Loading File: {filename}");

                string content;
                using (var reader = new StreamReader(filename))
                {
                    content = reader.ReadToEnd();
                    reader.Close();
                }

                content = Regex.Replace(content, @"\\/\*(.|\n)*?\*\/", ""); //todo: supposed to remove comment blocks


                var tr = new StringReader(content);

                var pokemonName = tr.ReadLine();
                while (pokemonName != null)
                {
                    addPokemonToResult(pokemonName);
                    pokemonName = tr.ReadLine();
                }
            }
            else
            {
                Logger.Write($"File: {filename} not found, creating new...", LogLevel.Warning);
                using (var w = File.AppendText(Directory.GetCurrentDirectory() + "\\" + filename))
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