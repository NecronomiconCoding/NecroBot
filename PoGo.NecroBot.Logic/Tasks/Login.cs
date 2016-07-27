using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Tasks
{
    public interface ILogin
    {
        void DoLogin();
    }
    public class Login : ILogin
    {
        private readonly PokemonGo.RocketAPI.Rpc.Login _loginApi;
        public Login(PokemonGo.RocketAPI.Rpc.Login loginApi)
        {
            _loginApi = loginApi;
        }

        public void DoLogin()
        {

        }
    }
}
