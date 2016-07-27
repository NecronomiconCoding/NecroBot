using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using PoGo.NecroBot.Logic;

using POGOProtos.Enums;
using POGOProtos.Inventory.Item;

using PokemonGo.RocketAPI;
using PokemonGo.RocketAPI.Enums;




namespace PoGo.NecroBot.UI.Config {
    public class LogicSettings : ILogicSettings {
        private GlobalSettings _settings;

        public LogicSettings(GlobalSettings settings) {
            _settings = settings;
        }

        public float KeepMinIvPercentage => _settings.KeepMinIvPercentage;
        public int KeepMinCp => _settings.KeepMinCp;
        public double WalkingSpeedInKilometerPerHour => _settings.WalkingSpeedInKilometerPerHour;
        public bool EvolveAllPokemonWithEnoughCandy => _settings.EvolveAllPokemonWithEnoughCandy;
        public bool KeepPokemonsThatCanEvolve => _settings.KeepPokemonsThatCanEvolve;
        public bool TransferDuplicatePokemon => _settings.TransferDuplicatePokemon;
        public int DelayBetweenPokemonCatch => _settings.DelayBetweenPokemonCatch;
        public bool UsePokemonToNotCatchFilter => _settings.UsePokemonToNotCatchFilter;
        public int KeepMinDuplicatePokemon => _settings.KeepMinDuplicatePokemon;
        public bool PrioritizeIvOverCp => _settings.PrioritizeIvOverCp;
        public int MaxTravelDistanceInMeters => _settings.MaxTravelDistanceInMeters;
        public string GpxFile => _settings.GpxFile;
        public bool UseGpxPathing => _settings.UseGpxPathing;
        public bool UseLuckyEggsWhileEvolving => _settings.UseLuckyEggsWhileEvolving;
        public bool EvolveAllPokemonAboveIv => _settings.EvolveAllPokemonAboveIv;
        public float EvolveAboveIvValue => _settings.EvolveAboveIvValue;
        public bool RenameAboveIv => _settings.RenameAboveIv;
        public int AmountOfPokemonToDisplayOnStart => _settings.AmountOfPokemonToDisplayOnStart;
        public ICollection<KeyValuePair<ItemId, int>> ItemRecycleFilter => _settings.ItemRecycleFilter;
        public ICollection<PokemonId> PokemonsToEvolve => _settings.PokemonsToEvolve;
        public ICollection<PokemonId> PokemonsNotToTransfer => _settings.PokemonsNotToTransfer;
        public ICollection<PokemonId> PokemonsNotToCatch => _settings.PokemonsToIgnore;
    }
}
