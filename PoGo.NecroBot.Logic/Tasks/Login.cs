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

        public async void DoLogin()
        {
            try
            {
                if (_session.Settings.AuthType != AuthType.Google || _session.Settings.AuthType != AuthType.Ptc)
                {
                    await _session.Client.Login.DoLogin();
                }
                else
                {
                    _session.EventDispatcher.Send(new ErrorEvent
                    {
                        Message = _session.Translation.GetTranslation(TranslationString.WrongAuthType)
                    });
                }
            }
            catch (AggregateException ae)
            {
                throw ae.Flatten().InnerException;
            }
            catch(LoginFailedException)
            {
                _session.EventDispatcher.Send(new ErrorEvent
                {
                    Message = _session.Translation.GetTranslation(TranslationString.LoginInvalid)
                });
            }
            catch (Exception ex) when (ex is PtcOfflineException || ex is AccessTokenExpiredException)
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