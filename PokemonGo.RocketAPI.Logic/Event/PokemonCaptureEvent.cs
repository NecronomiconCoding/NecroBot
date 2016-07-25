using POGOProtos.Enums;
using POGOProtos.Inventory.Item;
using POGOProtos.Networking.Responses;
using PokemonGo.RocketAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Logic.Event
{
    public class PokemonCaptureEvent : IEvent
    {
        public int Exp;
        public int Stardust;
        public CatchPokemonResponse.Types.CatchStatus Status;
        public double Level;
        public PokemonId Id;
        public int Cp;
        public int MaxCp;
        public double Perfection;
        public double Probability;
        public double Distance;
        public ItemType Pokeball;
        public int Attempt;
    }
}
