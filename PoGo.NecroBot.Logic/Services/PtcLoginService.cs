using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Player;
using PokemonGo.RocketAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Services
{
    public class PtcLoginService : ILoginService
    {
        private ISession _session;
        public PtcLoginService(ISession session)
        {
            _session = session;
        }
        public async Task Login()
        {
            try
            {
                try
                {
                    await _session.Client.Login.DoPtcLogin();
                }
                catch (AggregateException ae)
                {
                    throw ae.Flatten().InnerException;
                }
            }
            catch (PtcOfflineException)
            {
                _session.EventHandler.OnEvent(new ErrorEvent
                {
                    Message = "PTC Servers are probably down OR your credentials are wrong. Try google"
                });
                _session.EventHandler.OnEvent(new NoticeEvent { Message = "Trying again in 20 seconds..." });
            }
            catch (AccountNotVerifiedException)
            {
                _session.EventHandler.OnEvent(new ErrorEvent { Message = "Account not verified. - Exiting" });
            }
        }
    }
}
