using PoGo.NecroBot.Logic.Player;
using PokemonGo.RocketAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Services
{
    public class GoogleLoginService : ILoginService
    {
        private ISession _session;

        public GoogleLoginService()
        {
        }

        public GoogleLoginService(ISession session)
        {
            _session = session;
        }
        public async Task Login()
        {
            await _session.Client.Login.DoGoogleLogin();
        }
    }
}
