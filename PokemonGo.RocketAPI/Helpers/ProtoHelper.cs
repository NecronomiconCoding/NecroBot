using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;

namespace PokemonGo.RocketAPI.Helpers
{
    public class ProtoHelper
    {
        public static byte[] EncodeUlongList(List<ulong> integers)
        {
            var output = new List<byte>();
            foreach (var integer in integers.OrderBy(c => c))
            {
                output.AddRange(VarintBitConverter.GetVarintBytes(integer));
            }

            return output.ToArray();
        }
    }
}
