#region using directives

using System;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.Exceptions;

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
                        try
                        {
                            await session.Client.Login.DoPtcLogin(session.Settings.Username, session.Settings.Password);
                        }
                        catch (AggregateException ae)
                        {
                            throw ae.Flatten().InnerException;
                        }
                        break;
                    case AuthType.Google:
                        await session.Client.Login.DoGoogleLogin(session.Settings.Username, session.Settings.Password);
                        break;
                    default:
                        session.EventDispatcher.Send(new ErrorEvent { Message = session.Translation.GetTranslation(Common.TranslationString.WrongAuthType) });
                        return null;
                }
            }
            catch (PtcOfflineException)
            {
                session.EventDispatcher.Send(new ErrorEvent
                {
                    Message = session.Translation.GetTranslation(TranslationString.PtcOffline)
                });
                session.EventDispatcher.Send(new NoticeEvent { Message = session.Translation.GetTranslation(TranslationString.TryingAgainIn, 20) });
                await Task.Delay(20000);
                return this;
            }
            catch (AccountNotVerifiedException)
            {
                session.EventDispatcher.Send(new ErrorEvent { Message = session.Translation.GetTranslation(TranslationString.AccountNotVerified) });
                await Task.Delay(2000);
                Environment.Exit(0);
            }
            catch (GoogleException e)
            {
                if (e.Message.Contains("NeedsBrowser"))
                {
                    session.EventDispatcher.Send(new ErrorEvent { Message = session.Translation.GetTranslation(TranslationString.GoogleTwoFactorAuth) });
                    session.EventDispatcher.Send(new ErrorEvent { Message = session.Translation.GetTranslation(TranslationString.GoogleTwoFactorAuthExplanation) });
                    await Task.Delay(7000, cancellationToken);
                    try
                    {
                        System.Diagnostics.Process.Start("https://security.google.com/settings/security/apppasswords");
                    }
                    catch (Exception)
                    {
                        session.EventDispatcher.Send(new ErrorEvent { Message = "https://security.google.com/settings/security/apppasswords" });
                        throw;
                    }
                }
                session.EventDispatcher.Send(new ErrorEvent { Message = session.Translation.GetTranslation(TranslationString.GoogleError) });
                await Task.Delay(2000, cancellationToken);
                Environment.Exit(0);
            }

            await DownloadProfile(session);

            return new PositionCheckState();
        }

        private static async Task CheckLogin(ISession session, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (
                (session.Settings.AuthType == AuthType.Google || session.Settings.AuthType == AuthType.Ptc) &&
                (session.Settings.Username == null || session.Settings.Password == null) )
            {
                session.EventDispatcher.Send(new ErrorEvent
                {
                    Message = session.Translation.GetTranslation(TranslationString.MissingCredentials)
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