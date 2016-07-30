using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using PokemonGo.RocketAPI;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Common
{
    public class ApiFailureStrategy : IApiFailureStrategy
    {
        private int _retryCount = 0;
        private ISession _session;

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
            switch (_session.Settings.AuthType)
            {
                case AuthType.Ptc:
                    try
                    {
                        await _session.Client.Login.DoPtcLogin(_session.Settings.Username, _session.Settings.Password);
                    }
                    catch (AggregateException ae)
                    {
                        throw ae.Flatten().InnerException;
                    }
                    break;
                case AuthType.Google:
                    await _session.Client.Login.DoGoogleLogin(_session.Settings.Username, _session.Settings.Password);
                    break;
                default:
                    _session.EventDispatcher.Send(new ErrorEvent { Message = _session.Translation.GetTranslation(Common.TranslationString.WrongAuthType) });
                    break;
            }
        }
    }
}
