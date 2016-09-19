#region using directives

using System;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.Exceptions;
using PokemonGo.RocketAPI.Extensions;
using POGOProtos.Networking.Envelopes;
using POGOProtos.Networking.Requests;

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
                if (_session.Settings.AuthType == AuthType.Google || _session.Settings.AuthType == AuthType.Ptc)
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
            catch (NullReferenceException nre)
            {
                _session.EventDispatcher.Send(new ErrorEvent
                {
                    Message = "Causing Method: " + nre.TargetSite + " Source: " + nre.Source + " Data: " + nre.Data
                });
                throw nre.InnerException;
            }
            catch (LoginFailedException)
            {
                _session.EventDispatcher.Send(new ErrorEvent
                {
                    Message = _session.Translation.GetTranslation(TranslationString.LoginInvalid)
                });
            }
            catch (AccessTokenExpiredException)
            {
                _session.EventDispatcher.Send(new ErrorEvent
                {
                    Message = _session.Translation.GetTranslation(TranslationString.AccessTokenExpired)
                });
                _session.EventDispatcher.Send(new NoticeEvent
                {
                    Message = _session.Translation.GetTranslation(TranslationString.TryingAgainIn, 1)
                });

                await Task.Delay(1000);
            }
            catch (PtcOfflineException)
            {
                _session.EventDispatcher.Send(new ErrorEvent
                {
                    Message = _session.Translation.GetTranslation(TranslationString.PtcOffline)
                });
                _session.EventDispatcher.Send(new NoticeEvent
                {
                    Message = _session.Translation.GetTranslation(TranslationString.TryingAgainIn, 15)
                });

                await Task.Delay(15000);
            }
            catch (GoogleOfflineException)
            {
                _session.EventDispatcher.Send(new ErrorEvent
                {
                    Message = _session.Translation.GetTranslation(TranslationString.GoogleOffline)
                });
                _session.EventDispatcher.Send(new NoticeEvent
                {
                    Message = _session.Translation.GetTranslation(TranslationString.TryingAgainIn, 15)
                });

                await Task.Delay(15000);
            }
            catch (InvalidResponseException)
            {
                _session.EventDispatcher.Send(new ErrorEvent()
                {
                    Message = _session.Translation.GetTranslation(TranslationString.InvalidResponse)
                });
                _session.EventDispatcher.Send(new NoticeEvent
                {
                    Message = _session.Translation.GetTranslation(TranslationString.TryingAgainIn, 5)
                });

                await Task.Delay(5000);
            }
            catch (Exception ex)
            {
                _session.EventDispatcher.Send(new ErrorEvent
                {
                    Message = (ex.InnerException ?? ex).ToString()
                });
            }
        }
        public void HandleApiSuccess(RequestEnvelope request, ResponseEnvelope response)
        {
            if (response.StatusCode == 3)
            {
                for (int i = 0; i < request.Requests.Count; i++)
                {
                    if (request.Requests[i].RequestType == RequestType.GetInventory && response.Returns[i].IsEmpty)
                    {
                        _session.EventDispatcher.Send(new ErrorEvent
                        {
                            Message = _session.Translation.GetTranslation(TranslationString.AccountBanned)
                        });

                        _session.EventDispatcher.Send(new WarnEvent
                        {
                            Message = _session.Translation.GetTranslation(TranslationString.RequireInputText)
                        });

                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                }
            }

            _retryCount = 0;
        }

        public async Task<ApiOperation> HandleApiFailure(RequestEnvelope request, ResponseEnvelope response)
        {
            if (_retryCount == 11)
                return ApiOperation.Abort;

            await Task.Delay(500);
            _retryCount++;

            if (_retryCount % 5 == 0)
            {
                try
                {
                    DoLogin();
                }
                catch (PtcOfflineException)
                {
                    await Task.Delay(20000);
                }
                catch (AccessTokenExpiredException)
                {
                    await Task.Delay(2000);
                }
                catch (Exception ex) when (ex is InvalidResponseException || ex is TaskCanceledException)
                {
                    await Task.Delay(1000);
                }
            }

            return ApiOperation.Retry;
        }
    }
}
