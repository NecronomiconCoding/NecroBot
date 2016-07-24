#region

using System;
using System.Collections.Generic;
using System.IO;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.GeneratedCode;
using System.Text.RegularExpressions;

#endregion

namespace PokemonGo.RocketAPI.Console
{
    public class Settings : ISettings
    {

        public Settings()
        {
            BuildItemRecycleFilter();
        }

        private void BuildItemRecycleFilter()
        {
            _ItemRecycleFilter = new Dictionary<ItemId, int>();
            ItemRecycleFilter.Add(ItemId.ItemUnknown, 0);
            ItemRecycleFilter.Add(ItemId.ItemPokeBall, 20);
            ItemRecycleFilter.Add(ItemId.ItemGreatBall, KeepMoreItems ? 28 : 20);
            ItemRecycleFilter.Add(ItemId.ItemUltraBall, KeepMoreItems ? 83 : 50);
            ItemRecycleFilter.Add(ItemId.ItemMasterBall, 100);
            ItemRecycleFilter.Add(ItemId.ItemPotion, 0);
            ItemRecycleFilter.Add(ItemId.ItemSuperPotion, 0);
            ItemRecycleFilter.Add(ItemId.ItemHyperPotion, KeepMoreItems ? 73 : 20);
            ItemRecycleFilter.Add(ItemId.ItemMaxPotion, 50);
            ItemRecycleFilter.Add(ItemId.ItemRevive, KeepMoreItems ? 23 : 10);
            ItemRecycleFilter.Add(ItemId.ItemMaxRevive, 50);
            ItemRecycleFilter.Add(ItemId.ItemLuckyEgg, 200);
            ItemRecycleFilter.Add(ItemId.ItemIncenseOrdinary, 100);
            ItemRecycleFilter.Add(ItemId.ItemIncenseSpicy, 100);
            ItemRecycleFilter.Add(ItemId.ItemIncenseCool, 100);
            ItemRecycleFilter.Add(ItemId.ItemIncenseFloral, 100);
            ItemRecycleFilter.Add(ItemId.ItemTroyDisk, 100);
            ItemRecycleFilter.Add(ItemId.ItemXAttack, 100);
            ItemRecycleFilter.Add(ItemId.ItemXDefense, 100);
            ItemRecycleFilter.Add(ItemId.ItemXMiracle, 100);
            ItemRecycleFilter.Add(ItemId.ItemRazzBerry, KeepMoreItems ? 200 : 50);
            ItemRecycleFilter.Add(ItemId.ItemBlukBerry, 10);
            ItemRecycleFilter.Add(ItemId.ItemNanabBerry, 10);
            ItemRecycleFilter.Add(ItemId.ItemWeparBerry, 30);
            ItemRecycleFilter.Add(ItemId.ItemPinapBerry, 30);
            ItemRecycleFilter.Add(ItemId.ItemSpecialCamera, 100);
            ItemRecycleFilter.Add(ItemId.ItemIncubatorBasicUnlimited, 100);
            ItemRecycleFilter.Add(ItemId.ItemIncubatorBasic, 100);
            ItemRecycleFilter.Add(ItemId.ItemPokemonStorageUpgrade, 100);
            ItemRecycleFilter.Add(ItemId.ItemItemStorageUpgrade, 100);
        }

        private ICollection<PokemonId> _pokemonsNotToTransfer;
        private ICollection<PokemonId> _pokemonsToEvolve;
        private ICollection<PokemonId> _pokemonsNotToCatch;
        private Dictionary<ItemId, int> _ItemRecycleFilter;

        public AuthType AuthType => (AuthType)Enum.Parse(typeof(AuthType), UserSettings.Default.AuthType, true);
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
        public bool KeepMoreItems => UserSettings.Default.KeepMoreItems;
        public bool PrioritizeIVOverCP => UserSettings.Default.PrioritizeIVOverCP;
        public int MaxTravelDistanceInMeters => UserSettings.Default.MaxTravelDistanceInMeters;
         
        //Type and amount to keep
        public Dictionary<ItemId, int> ItemRecycleFilter
        {
            get
            {
                return _ItemRecycleFilter;
            }
        }

        public ICollection<PokemonId> PokemonsToEvolve
        {
            get
            {
                //Type of pokemons to evolve
                var defaultText = new string[] { "Zubat", "Pidgey", "Ratata" };
                _pokemonsToEvolve = _pokemonsToEvolve ?? LoadPokemonList("Configs\\ConfigPokemonsToEvolve.txt", defaultText);
                return _pokemonsToEvolve;
            }
        }

        public ICollection<PokemonId> PokemonsNotToTransfer
        {
            get
            {
                //Type of pokemons not to transfer
                var defaultText = new string[] { "Dragonite", "Charizard", "Zapdos", "Snorlax", "Alakhazam", "Mew", "Mewtwo" };
                _pokemonsNotToTransfer = _pokemonsNotToTransfer ?? LoadPokemonList("Configs\\ConfigPokemonsToKeep.txt", defaultText);
                return _pokemonsNotToTransfer;
            }
        }

        //Do not catch those
        public ICollection<PokemonId> PokemonsNotToCatch
        {
            get
            {
                //Type of pokemons not to catch
                var defaultText = new string[] { "Zubat", "Pidgey", "Ratata" };
                _pokemonsNotToCatch = _pokemonsNotToCatch ?? LoadPokemonList("Configs\\ConfigPokemonsNotToCatch.txt", defaultText);
                return _pokemonsNotToCatch;
            }
        }

        private static ICollection<PokemonId> LoadPokemonList(string filename, string[] defaultContent)
        {
            ICollection<PokemonId> result = new List<PokemonId>();
            Func<string, ICollection<PokemonId>> addPokemonToResult = delegate (string pokemonName) {
                PokemonId pokemon;
                if (Enum.TryParse<PokemonId>(pokemonName, out pokemon))
                {
                    result.Add((PokemonId)pokemon);
                }
                return result;
            };

            DirectoryInfo di = Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Configs");

            if (File.Exists(Directory.GetCurrentDirectory() + "\\" + filename))
            {
                Logger.Write($"Loading File: {filename}");

                var content = string.Empty;
                using (StreamReader reader = new StreamReader(filename))
                {
                    content = reader.ReadToEnd();
                    reader.Close();
                }

                content = Regex.Replace(content, @"\\/\*(.|\n)*?\*\/", ""); //todo: supposed to remove comment blocks


                StringReader tr = new StringReader(content);

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
