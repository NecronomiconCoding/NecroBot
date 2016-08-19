#region using directives

using PoGo.NecroBot.Logic.Model;
using POGOProtos.Enums;
using Location = PoGo.NecroBot.Logic.Model.Settings.Location;

#endregion

namespace PoGo.NecroBot.Logic.Event
{
    public class SnipeScanEvent : IEvent
    {
        public Location Bounds { get; set; }
        public PokemonId PokemonId { get; set; }
        public double Iv { get; set; }
        public string Source { get; set; }
    }
}