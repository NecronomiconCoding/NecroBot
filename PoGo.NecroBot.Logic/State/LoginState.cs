#region using directives

using System;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.Exceptions;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public class LoginState : IState
    {
        public async Task<IState> Execute(Session session, StateMachine machine)
        {
            machine.Fire(new NoticeEvent { Message = session.Translations.GetTranslation(Common.TranslationString.LoggingIn, session.Settings.AuthType) });
            try
            {
                switch (session.Settings.AuthType)
                {
                    case AuthType.Ptc:
                        try
                        {
                            await session.Client.Login.DoPtcLogin();
                        }
                        catch (AggregateException ae)
                        {
                            throw ae.Flatten().InnerException;
                        }
                        break;
                    case AuthType.Google:
                        await session.Client.Login.DoGoogleLogin();
                        break;
                    default:
                        machine.Fire(new ErrorEvent {Message = session.Translations.GetTranslation(Common.TranslationString.WrongAuthType)});
                        return null;
                }
            }
            catch (PtcOfflineException)
            {
                machine.Fire(new ErrorEvent
                {
                    Message = session.Translations.GetTranslation(Common.TranslationString.PtcOffline)
                });
                machine.Fire(new NoticeEvent {Message = session.Translations.GetTranslation(Common.TranslationString.TryingAgainIn, 20)});
                await Task.Delay(20000);
                return this;
            }
            catch (AccountNotVerifiedException)
            {
                machine.Fire(new ErrorEvent {Message = session.Translations.GetTranslation(Common.TranslationString.AccountNotVerified)});
                return null;
            }

            await DownloadProfile(session, machine);

            return new PositionCheckState();
        }

        public async Task DownloadProfile(Session session, StateMachine machine)
        {
            session.Profile = await session.Client.Player.GetPlayer();
            machine.Fire(new ProfileEvent {Profile = session.Profile});
        }
    }
}