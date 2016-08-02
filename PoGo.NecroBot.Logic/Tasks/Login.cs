#region using directives

using System;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.Exceptions;

#endregion

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
                    try
                    {
                        _session.Client.Login.DoLogin()
                            .Wait();
                    }
                    catch (AggregateException ae)
                    {
                        throw ae.Flatten().InnerException;
                    }
            }
            catch (PtcOfflineException)
            {
                _session.EventDispatcher.Send(new ErrorEvent
                {
                    Message = _session.Translation.GetTranslation(TranslationString.PtcOffline)
                });
                _session.EventDispatcher.Send(new NoticeEvent
                {
                    Message = _session.Translation.GetTranslation(TranslationString.TryingAgainIn, 20)
                });
            }
            catch (AccountNotVerifiedException)
            {
                _session.EventDispatcher.Send(new ErrorEvent
                {
                    Message = _session.Translation.GetTranslation(TranslationString.AccountNotVerified)
                });
            }
        }
    }
}