using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Console.Server.Models
{
    public class ResponseModel<T>
    {
        public int status;
        public string message;
        public T data;
    }
}
