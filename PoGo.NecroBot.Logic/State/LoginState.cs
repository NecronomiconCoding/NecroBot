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
                switch (session.Settings.AuthType)
                {
                    case AuthType.Ptc:
                        await
                            session.Client.Login.DoPtcLogin(session.Settings.PtcUsername,
                                session.Settings.PtcPassword);
                        break;
                    case AuthType.Google:
                        await
                            session.Client.Login.DoGoogleLogin(session.Settings.GoogleUsername,
                                session.Settings.GooglePassword);
                        break;
                    default:
                        session.EventDispatcher.Send(new ErrorEvent
                        {
                            Message = session.Translation.GetTranslation(TranslationString.WrongAuthType)
                        });
                        return null;
                }
            }
            catch (AggregateException ae)
            {
                throw ae.Flatten().InnerException;
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
                await Task.Delay(20000, cancellationToken);
                return this;
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

            int maxTheoreticalItems = session.LogicSettings.TotalAmountOfPokeballsToKeep +
                session.LogicSettings.TotalAmountOfPotionsToKeep +
                session.LogicSettings.TotalAmountOfRevivesToKeep;

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
            session.Profile = await session.Client.Player.GetPlayer();
            session.EventDispatcher.Send(new ProfileEvent { Profile = session.Profile });
        }
    }
}