using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POGOProtos.Inventory.Item;
using POGOProtos.Map.Pokemon;
using POGOProtos.Enums;
namespace PokemonGo.RocketAPI.Logic
{
    public interface LogicSettings
    {
        float KeepMinIVPercentage { get; }
        int KeepMinCP { get; }
        double WalkingSpeedInKilometerPerHour { get; }
        bool EvolveAllPokemonWithEnoughCandy { get; }
        bool TransferDuplicatePokemon { get; }
        int DelayBetweenPokemonCatch { get; }
        bool UsePokemonToNotCatchFilter { get; }
        int KeepMinDuplicatePokemon { get; }
        bool PrioritizeIVOverCP { get; }
        int MaxTravelDistanceInMeters { get; }
        bool UseGPXPathing { get; }
        string GPXFile { get; }
        bool useLuckyEggsWhileEvolving { get; }
        bool EvolveAllPokemonAboveIV { get; }
        float EvolveAboveIVValue { get; }

        ICollection<KeyValuePair<ItemId, int>> ItemRecycleFilter { get; }

        ICollection<PokemonId> PokemonsToEvolve { get; }

        ICollection<PokemonId> PokemonsNotToTransfer { get; }

        ICollection<PokemonId> PokemonsNotToCatch { get; }
    }
}
