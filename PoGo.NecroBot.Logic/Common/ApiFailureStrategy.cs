#region using directives

using System;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.Exceptions;
using PokemonGo.RocketAPI.Extensions;

#endregion

namespace PoGo.NecroBot.Logic.Common
{
    public class ApiFailureStrategy : IApiFailureStrategy
    {
        private readonly ISession _session;
        private int _retryCount;

        public ApiFailureStrategy(ISession session)
        {
            _session = session;
        }

        public async Task<ApiOperation> HandleApiFailure()
        {
            if (_retryCount == 11)
                return ApiOperation.Abort;

            await Task.Delay(500);
            _retryCount++;

            if (_retryCount % 5 == 0)
            {
                DoLogin();
            }

            return ApiOperation.Retry;
        }

        public void HandleApiSuccess()
        {
            _retryCount = 0;
        }

        private async void DoLogin()
        {
            try
            {
                switch (_session.Settings.AuthType)
                {
                    case AuthType.Ptc:
                        await
                            _session.Client.Login.DoPtcLogin(_session.Settings.PtcUsername,
                                _session.Settings.PtcPassword);
                        break;
                    case AuthType.Google:
                        await
                            _session.Client.Login.DoGoogleLogin(_session.Settings.GoogleUsername,
                                _session.Settings.GooglePassword);
                        break;
                    default:
                        _session.EventDispatcher.Send(new ErrorEvent
                        {
                            Message = _session.Translation.GetTranslation(TranslationString.WrongAuthType)
                        });
                        break;
                }
            }
            catch (AggregateException ae)
            {
                throw ae.Flatten().InnerException;
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

        }
    }
}