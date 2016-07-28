using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Service;
using PoGo.NecroBot.Logic.State;
using PokemonGo.RocketAPI;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.Exceptions;
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
        private readonly ISession _session;

        public Login(ISession session)
        {
            _session = session;
        }

        public void DoLogin()
        {
            try
            {
                if (_session.Client.AuthType == AuthType.Ptc)
                {
                    try
                    {
                        _session.Client.Login.DoPtcLogin(_session.Client.Settings.PtcUsername,_session.Client.Settings.PtcPassword).Wait();
                    }
                    catch (AggregateException ae)
                    {
                        throw ae.Flatten().InnerException;
                    }
                }
                else
                {
                    _session.Client.Login.DoGoogleLogin().Wait();
                }
            }
            catch (PtcOfflineException)
            {
              //  _session.EventDispatcher.Send(new ErrorEvent { Message = _session..GetFormat(TranslationString.PtcOffline) });
              //  _session.EventDispatcher.Send(new NoticeEvent { Message = _session.Localizer.GetFormat(TranslationString.TryingAgainIn, 20) });
                
            }
            catch (AccountNotVerifiedException)
            {
              //  _session.EventDispatcher.Send(new ErrorEvent { Message = _session.Localizer.GetFormat(TranslationString.AccountNotVerified) });
            }
        }
    }
}
