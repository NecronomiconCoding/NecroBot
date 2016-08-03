#region using directives

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.Exceptions;
using PoGo.NecroBot.Logic.Logging;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public class LoginState : IState
    {
        public async Task<IState> Execute(ISession session, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            session.EventDispatcher.Send(new NoticeEvent
            {
                Message = session.Translation.GetTranslation(TranslationString.LoggingIn, session.Settings.AuthType)
            });

            await CheckLogin(session, cancellationToken);

            try
            {
                if (session.Settings.AuthType != AuthType.Google || session.Settings.AuthType != AuthType.Ptc)
                {
                    await session.Client.Login.DoLogin();
                }
                else
                {
                    session.EventDispatcher.Send(new ErrorEvent
                    {
                        Message = session.Translation.GetTranslation(TranslationString.WrongAuthType)
                    });
                }
            }
            catch (AggregateException ae)
            {
                throw ae.Flatten().InnerException;
            }
            catch (LoginFailedException)
            {
                session.EventDispatcher.Send(new ErrorEvent
                {
                    Message = session.Translation.GetTranslation(TranslationString.LoginInvalid)
                });
                await Task.Delay(2000, cancellationToken);
                Environment.Exit(0);
            }
            catch (Exception ex) when (ex is PtcOfflineException || ex is AccessTokenExpiredException)
            {
                session.EventDispatcher.Send(new ErrorEvent
                {
                    Message = session.Translation.GetTranslation(TranslationString.PtcOffline)
                });
                session.EventDispatcher.Send(new NoticeEvent
                {
                    Message = session.Translation.GetTranslation(TranslationString.TryingAgainIn, 20)
                });
            }
            catch (AccountNotVerifiedException)
            {
                session.EventDispatcher.Send(new ErrorEvent
                {
                    Message = session.Translation.GetTranslation(TranslationString.AccountNotVerified)
                });
                await Task.Delay(2000, cancellationToken);
                Environment.Exit(0);
            }
            catch (GoogleException e)
            {
                if (e.Message.Contains("NeedsBrowser"))
                {
                    session.EventDispatcher.Send(new ErrorEvent
                    {
                        Message = session.Translation.GetTranslation(TranslationString.GoogleTwoFactorAuth)
                    });
                    session.EventDispatcher.Send(new ErrorEvent
                    {
                        Message = session.Translation.GetTranslation(TranslationString.GoogleTwoFactorAuthExplanation)
                    });
                    await Task.Delay(7000, cancellationToken);
                    try
                    {
                        Process.Start("https://security.google.com/settings/security/apppasswords");
                    }
                    catch (Exception)
                    {
                        session.EventDispatcher.Send(new ErrorEvent
                        {
                            Message = "https://security.google.com/settings/security/apppasswords"
                        });
                        throw;
                    }
                }
                session.EventDispatcher.Send(new ErrorEvent
                {
                    Message = session.Translation.GetTranslation(TranslationString.GoogleError)
                });
                await Task.Delay(2000, cancellationToken);
                Environment.Exit(0);
            }
            catch (InvalidProtocolBufferException ex) when (ex.Message.Contains("SkipLastField"))
            {
                session.EventDispatcher.Send(new ErrorEvent
                {
                    Message = session.Translation.GetTranslation(TranslationString.IPBannedError)
                });
                await Task.Delay(2000, cancellationToken);
                Environment.Exit(0);
            }
            catch (Exception)
            {
                await Task.Delay(20000, cancellationToken);
                return this;
            }

            await DownloadProfile(session);
            if (session.Profile == null)
            {
                await Task.Delay(20000, cancellationToken);
                Logger.Write("Due to login failure your player profile could not be retrieved. Press any key to re-try login.", LogLevel.Warning);
                Console.ReadKey();
            }

            int maxTheoreticalItems = session.LogicSettings.TotalAmountOfPokeballsToKeep +
                session.LogicSettings.TotalAmountOfPotionsToKeep +
                session.LogicSettings.TotalAmountOfRevivesToKeep +
                session.LogicSettings.TotalAmountOfBerriesToKeep;

            if (maxTheoreticalItems > session.Profile.PlayerData.MaxItemStorage)
            {
                Logger.Write(session.Translation.GetTranslation(TranslationString.MaxItemsCombinedOverMaxItemStorage, maxTheoreticalItems, session.Profile.PlayerData.MaxItemStorage), LogLevel.Error);
                Logger.Write("Press any key to exit, then fix your configuration and run the bot again.", LogLevel.Warning);
                Console.ReadKey();
                System.Environment.Exit(1);
            }

            return new PositionCheckState();
        }

        private static async Task CheckLogin(ISession session, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (session.Settings.AuthType == AuthType.Google &&
                (session.Settings.GoogleUsername == null || session.Settings.GooglePassword == null))
            {
                session.EventDispatcher.Send(new ErrorEvent
                {
                    Message = session.Translation.GetTranslation(TranslationString.MissingCredentialsGoogle)
                });
                await Task.Delay(2000, cancellationToken);
                Environment.Exit(0);
            }
            else if (session.Settings.AuthType == AuthType.Ptc &&
                     (session.Settings.PtcUsername == null || session.Settings.PtcPassword == null))
            {
                session.EventDispatcher.Send(new ErrorEvent
                {
                    Message = session.Translation.GetTranslation(TranslationString.MissingCredentialsPtc)
                });
                await Task.Delay(2000, cancellationToken);
                Environment.Exit(0);
            }
        }

        public async Task DownloadProfile(ISession session)
        {
            try
            {
                session.Profile = await session.Client.Player.GetPlayer();
                session.EventDispatcher.Send(new ProfileEvent { Profile = session.Profile });
            }
            catch (System.UriFormatException e)
            {
                session.EventDispatcher.Send(new ErrorEvent { Message = e.ToString() });
            }
            catch (Exception ex)
            {
                session.EventDispatcher.Send(new ErrorEvent { Message = ex.ToString() });
            }
        }
    }
}