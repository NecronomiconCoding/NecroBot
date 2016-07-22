#region

using System;
using System.IO;
using System.Collections.Generic;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.GeneratedCode;

#endregion

namespace PokemonGo.RocketAPI.Console
{
    public class Settings : ISettings
    {
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

        private ICollection<PokemonId> _pokemonsToEvolve;
        private ICollection<PokemonId> _pokemonsNotToTransfer;

        public string GoogleRefreshToken
        {
            get { return UserSettings.Default.GoogleRefreshToken; }
            set
            {
                UserSettings.Default.GoogleRefreshToken = value;
                UserSettings.Default.Save();
            }
        }

        public ICollection<KeyValuePair<ItemId, int>> ItemRecycleFilter
        {
            get
            {
                //Type and amount to keep
                return new[]
                {
                    new KeyValuePair<ItemId, int>(ItemId.ItemUnknown, 0),
                    new KeyValuePair<ItemId, int>(ItemId.ItemPokeBall, 20),
                    new KeyValuePair<ItemId, int>(ItemId.ItemGreatBall, 20),
                    new KeyValuePair<ItemId, int>(ItemId.ItemUltraBall, 50),
                    new KeyValuePair<ItemId, int>(ItemId.ItemMasterBall, 100),
                    new KeyValuePair<ItemId, int>(ItemId.ItemPotion, 0),
                    new KeyValuePair<ItemId, int>(ItemId.ItemSuperPotion, 0),
                    new KeyValuePair<ItemId, int>(ItemId.ItemHyperPotion, 20),
                    new KeyValuePair<ItemId, int>(ItemId.ItemMaxPotion, 50),
                    new KeyValuePair<ItemId, int>(ItemId.ItemRevive, 10),
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
                    new KeyValuePair<ItemId, int>(ItemId.ItemRazzBerry, 200),
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
            }
        }
        private ICollection<PokemonId> LoadPokemonList(string filename)
        {
            ICollection<PokemonId> result = new List<PokemonId>();

            if (File.Exists(Directory.GetCurrentDirectory() + "\\" + filename))
            {
                Logger.Write($"Loading File: {filename}", LogLevel.Info);
                TextReader tr;
                tr = File.OpenText(filename);

                var pokemonName = tr.ReadLine();
                while (pokemonName != null)
                {

                    var pokemon = Enum.Parse(typeof(PokemonId), pokemonName, true);
                    if (pokemon != null) result.Add((PokemonId)pokemon);
                    pokemonName = tr.ReadLine();
                }

            }
            else
            {
                Logger.Write($"File: {filename} not found, creating new...", LogLevel.Error);
                using (StreamWriter w = File.AppendText(Directory.GetCurrentDirectory() + "\\" + filename)) { w.WriteLine(PokemonId.Mewtwo.ToString()); }
            }


            return result;

        }
        public ICollection<PokemonId> PokemonsToEvolve
        {
            get
            {
                //Type of pokemons to evolve
                _pokemonsToEvolve = _pokemonsToEvolve != null ? _pokemonsToEvolve : LoadPokemonList("Configs\\ConfigPokemonsToEvolve.txt");
                return _pokemonsToEvolve;
            }
        }
        
        public ICollection<PokemonId> PokemonsNotToTransfer
        {
            get
            {
                //Type of pokemons not to transfer
                _pokemonsNotToTransfer = _pokemonsNotToTransfer != null ? _pokemonsNotToTransfer : LoadPokemonList("Configs\\ConfigPokemonsToKeep.txt"); 
                return _pokemonsNotToTransfer;
            }
        }
    }
}