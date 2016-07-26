using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonGo.RocketAPI.GeneratedCode;

namespace PokemonGo.RocketAPI.Console.Server.Models
{
    public class ResponseModel<T>
    {
        public int status;
        public string message;
        public T data;

        public static ResponseModel<T> Factory(T item)
        {
            ResponseModel < T > model = new ResponseModel<T>();
            model.data = item;
            return model;
        }
    }
}
