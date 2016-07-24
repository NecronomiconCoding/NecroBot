using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Logic.State
{
    public class LoginState : IState
    {
        public LoginState()
        {

        }

        public IState Execute(Context ctx)
        {
            return this;
        }
    }
}
