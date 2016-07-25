#region using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using PoGo.NecroBot.Logic;
using PoGo.NecroBot.Logic.Logging;
using PokemonGo.RocketAPI;
using PokemonGo.RocketAPI.Enums;
using POGOProtos.Enums;
using POGOProtos.Inventory.Item;

#endregion

namespace PoGo.NecroBot.CLI
{
    public class ClientSettings : ISettings
    {
        public AuthType AuthType => (AuthType) Enum.Parse(typeof(AuthType), UserSettings.Default.AuthType, true);
        public string PtcUsername => UserSettings.Default.PtcUsername;
        public string PtcPassword => UserSettings.Default.PtcPassword;
        public double DefaultLatitude => UserSettings.Default.DefaultLatitude;
        public double DefaultLongitude => UserSettings.Default.DefaultLongitude;
        public double DefaultAltitude => UserSettings.Default.DefaultAltitude;

        public string GoogleRefreshToken { get; set; }
    }

    public class LogicSettings : ILogicSettings
    {
        private ICollection<KeyValuePair<ItemId, int>> _itemRecycleFilter;
        private ICollection<PokemonId> _pokemonsNotToCatch;

        private ICollection<PokemonId> _pokemonsNotToTransfer;
        private ICollection<PokemonId> _pokemonsToEvolve;

        //public AuthType AuthType => (AuthType)Enum.Parse(typeof(AuthType), UserSettings.Default.AuthType, true);
        //public string PtcUsername => UserSettings.Default.PtcUsername;
        //public string PtcPassword => UserSettings.Default.PtcPassword;
        //public double DefaultLatitude => UserSettings.Default.DefaultLatitude;
        //public double DefaultLongitude => UserSettings.Default.DefaultLongitude;
        //public double DefaultAltitude => UserSettings.Default.DefaultAltitude;
        public float KeepMinIvPercentage => UserSettings.Default.KeepMinIVPercentage;
        public int KeepMinCp => UserSettings.Default.KeepMinCP;
        public double WalkingSpeedInKilometerPerHour => UserSettings.Default.WalkingSpeedInKilometerPerHour;
        public bool EvolveAllPokemonWithEnoughCandy => UserSettings.Default.EvolveAllPokemonWithEnoughCandy;
        public bool TransferDuplicatePokemon => UserSettings.Default.TransferDuplicatePokemon;
        public int DelayBetweenPokemonCatch => UserSettings.Default.DelayBetweenPokemonCatch;
        public bool UsePokemonToNotCatchFilter => UserSettings.Default.UsePokemonToNotCatchFilter;
        public int KeepMinDuplicatePokemon => UserSettings.Default.KeepMinDuplicatePokemon;
        public bool PrioritizeIvOverCp => UserSettings.Default.PrioritizeIVOverCP;
        public int MaxTravelDistanceInMeters => UserSettings.Default.MaxTravelDistanceInMeters;
        public string GpxFile => UserSettings.Default.GPXFile;
        public bool UseGpxPathing => UserSettings.Default.UseGPXPathing;
        public bool UseLuckyEggsWhileEvolving => UserSettings.Default.useLuckyEggsWhileEvolving;
        public bool EvolveAllPokemonAboveIv => UserSettings.Default.EvolveAllPokemonAboveIV;
        public float EvolveAboveIvValue => UserSettings.Default.EvolveAboveIVValue;

        //Type and amount to keep
        public ICollection<KeyValuePair<ItemId, int>> ItemRecycleFilter
        {
            get
            {
                //Type of pokemons to evolve
                var defaultItems = new List<KeyValuePair<ItemId, int>>
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
                _itemRecycleFilter = _itemRecycleFilter ?? LoadItemList("Configs\\ConfigItemList.ini", defaultItems);
                return _itemRecycleFilter;
            }
        }


        public ICollection<PokemonId> PokemonsToEvolve
        {
            get
            {
                //Type of pokemons to evolve
                var defaultPokemon = new List<PokemonId>
                {
                    PokemonId.Zubat,
                    PokemonId.Pidgey,
                    PokemonId.Rattata
                };
                _pokemonsToEvolve = _pokemonsToEvolve ??
                                    LoadPokemonList("Configs\\ConfigPokemonsToEvolve.ini", defaultPokemon);
                return _pokemonsToEvolve;
            }
        }

        public ICollection<PokemonId> PokemonsNotToTransfer
        {
            get
            {
                //Type of pokemons not to transfer
                var defaultPokemon = new List<PokemonId>
                {
                    PokemonId.Dragonite,
                    PokemonId.Charizard,
                    PokemonId.Zapdos,
                    PokemonId.Snorlax,
                    PokemonId.Alakazam,
                    PokemonId.Mew,
                    PokemonId.Mewtwo
                };
                _pokemonsNotToTransfer = _pokemonsNotToTransfer ??
                                         LoadPokemonList("Configs\\ConfigPokemonsToKeep.ini", defaultPokemon);
                return _pokemonsNotToTransfer;
            }
        }

        //Do not catch those
        public ICollection<PokemonId> PokemonsNotToCatch
        {
            get
            {
                //Type of pokemons not to catch
                var defaultPokemon = new List<PokemonId>
                {
                    PokemonId.Zubat,
                    PokemonId.Pidgey,
                    PokemonId.Rattata
                };
                _pokemonsNotToCatch = _pokemonsNotToCatch ??
                                      LoadPokemonList("Configs\\ConfigPokemonsNotToCatch.ini", defaultPokemon);
                return _pokemonsNotToCatch;
            }
        }

        public ICollection<KeyValuePair<ItemId, int>> LoadItemList(string filename,
            List<KeyValuePair<ItemId, int>> defaultItems)
        {
            ICollection<KeyValuePair<ItemId, int>> result = new List<KeyValuePair<ItemId, int>>();

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

                var itemInfo = tr.ReadLine();
                while (itemInfo != null)
                {
                    var itemInfoArray = itemInfo.Split(' ');
                    var itemName = itemInfoArray.Length > 1 ? itemInfoArray[0] : "";
                    int itemAmount;
                    if (!int.TryParse(itemInfoArray.Length > 1 ? itemInfoArray[1] : "100", out itemAmount))
                        itemAmount = 100;

                    ItemId item;
                    if (Enum.TryParse(itemName, out item))
                    {
                        result.Add(new KeyValuePair<ItemId, int>(item, itemAmount));
                    }
                    itemInfo = tr.ReadLine();
                }
            }
            else
            {
                Logger.Write($"File: {filename} not found, creating new...", LogLevel.Warning);
                using (var w = File.AppendText(Directory.GetCurrentDirectory() + "\\" + filename))
                {
                    defaultItems.ForEach(itemInfo => w.WriteLine($"{itemInfo.Key} {itemInfo.Value}"));
                    defaultItems.ForEach(itemInfo => result.Add(itemInfo));
                    w.Close();
                }
            }
            return result;
        }

        private static ICollection<PokemonId> LoadPokemonList(string filename, List<PokemonId> defaultPokemon)
        {
            ICollection<PokemonId> result = new List<PokemonId>();

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
                    PokemonId pokemon;
                    if (Enum.TryParse(pokemonName, out pokemon))
                    {
                        result.Add(pokemon);
                    }
                    pokemonName = tr.ReadLine();
                }
            }
            else
            {
                Logger.Write($"File: {filename} not found, creating new...", LogLevel.Warning);
                using (var w = File.AppendText(Directory.GetCurrentDirectory() + "\\" + filename))
                {
                    defaultPokemon.ForEach(pokemon => w.WriteLine(pokemon.ToString()));
                    defaultPokemon.ForEach(pokemon => result.Add(pokemon));
                    w.Close();
                }
            }
            return result;
        }
    }
}