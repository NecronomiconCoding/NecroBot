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
                if (_session.Client.AuthType == AuthType.Ptc)
                {
                    try
                    {
                        _session.Client.Login.DoPtcLogin(_session.Settings.PtcUsername, _session.Settings.PtcPassword)
                            .Wait();
                    }
                    catch (AggregateException ae)
                    {
                        throw ae.Flatten().InnerException;
                    }
                }
                else
                {
                    _session.Client.Login.DoGoogleLogin(_session.Settings.GoogleUsername,
                        _session.Settings.GooglePassword).Wait();
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