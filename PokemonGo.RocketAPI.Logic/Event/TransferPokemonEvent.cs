using POGOProtos.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Logic.Event
{
    public class TransferPokemonEvent : IEvent
    {
        public PokemonId Id;
        public double Perfection;
        public int Cp;
        public int BestCp;
        public double BestPerfection;
    }
}
