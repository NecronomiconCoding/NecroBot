#region

using System.Collections.Generic;
using PokemonGoDesktop.API.Proto;
using PokemonGoDesktop.API.Common;

#endregion

namespace PokemonGo.RocketAPI
{
    public interface ISettings
    {
        AuthType AuthType { get; }
        double DefaultLatitude { get; }
        double DefaultLongitude { get; }
        double DefaultAltitude { get; }
        string PtcPassword { get; }
        string PtcUsername { get; }
        float KeepMinIVPercentage { get; }
        int KeepMinCP { get; }
        double WalkingSpeedInKilometerPerHour { get; }
        bool EvolveAllPokemonWithEnoughCandy { get; }
        bool TransferDuplicatePokemon { get; }
        int DelayBetweenPokemonCatch { get; }
        bool UsePokemonToNotCatchFilter { get; }
        int KeepMinDuplicatePokemon { get; }
        bool PrioritizeIVOverCP {get; }
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
