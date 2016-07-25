using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POGOProtos.Networking.Responses;
using POGOProtos.Data;
using POGOProtos.Enums;
using POGOProtos.Settings.Master;
using POGOProtos.Inventory;
using POGOProtos.Inventory.Item;
using POGOProtos.Data.Player;
using PokemonGo.RocketAPI.Logic.Event;
namespace PokemonGo.RocketAPI.Logic.Event
{
    public class PokemonEvolveEvent : IEvent
    {
        public PokemonId Id;
        public int Exp;
        public EvolvePokemonOut.Types.EvolvePokemonStatus Result;
    }
}
