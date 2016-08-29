#region using directives

using System.Collections.Generic;
using PoGo.NecroBot.Logic.Model;
using PoGo.NecroBot.Logic.Model.Settings;
using PoGo.NecroBot.Logic.Tasks;
using POGOProtos.Enums;
using POGOProtos.Map.Fort;
using Location = PoGo.NecroBot.Logic.Model.Settings.Location;

#endregion

namespace PoGo.NecroBot.Logic.Event
{
    public enum HumanWalkSnipeEventTypes
    {
        StartWalking,
        DestinationReached,
        PokemonScanned,
        AddedSnipePokemon,
        PokestopUpdated,
        NotEnoughtPalls,
        TargetedPokemon,
        ClientRequestUpdate,
        EncounterSnipePokemon,
        QueueUpdated
    }
    public class HumanWalkSnipeEvent : IEvent
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public double Distance { get; set; }

        public double WalkTimes { get; set; }

        public PokemonId PokemonId { get; set; }
        public HumanWalkSnipeEventTypes Type { get; set; }
        public double Expires { get; internal set; }
        public int Estimate { get; internal set; }
        public HumanWalkSnipeFilter Setting { get; internal set; }
        public bool SpinPokeStop { get; set; }
        public bool CatchPokemon { get; set; }
        public double NearestDistance { get; internal set; }
        public List<FortData> Pokestops { get; internal set; }
        public int CurrentBalls { get; internal set; }
        public int MinBallsToSnipe { get; internal set; }
        public object WalkSpeedApplied { get; internal set; }
        public List<HumanWalkSnipeTask.SnipePokemonInfo> Pokemons { get; internal set; }
        public string UniqueId { get; internal set; }
        public int PauseDuration { get; internal set; }
    }
}