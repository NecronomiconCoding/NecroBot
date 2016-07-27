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
        public async Task<IState> Execute(Session ctx, StateMachine machine)
        {
            machine.Fire(new NoticeEvent { Message = ctx.Translations.GetTranslation(Common.TranslationString.LoggingIn, ctx.Settings.AuthType) });
            try
            {
                switch (ctx.Settings.AuthType)
                {
                    case AuthType.Ptc:
                        try
                        {
                            await ctx.Client.Login.DoPtcLogin();
                        }
                        catch (AggregateException ae)
                        {
                            throw ae.Flatten().InnerException;
                        }
                        break;
                    case AuthType.Google:
                        await ctx.Client.Login.DoGoogleLogin();
                        break;
                    default:
                        machine.Fire(new ErrorEvent {Message = ctx.Translations.GetTranslation(Common.TranslationString.WrongAuthType)});
                        return null;
                }
            }
            catch (PtcOfflineException)
            {
                machine.Fire(new ErrorEvent
                {
                    Message = ctx.Translations.GetTranslation(Common.TranslationString.PtcOffline)
                });
                machine.Fire(new NoticeEvent {Message = ctx.Translations.GetTranslation(Common.TranslationString.TryingAgainIn, 20)});
                await Task.Delay(20000);
                return this;
            }
            catch (AccountNotVerifiedException)
            {
                machine.Fire(new ErrorEvent {Message = ctx.Translations.GetTranslation(Common.TranslationString.AccountNotVerified)});
                return null;
            }

            await DownloadProfile(ctx, machine);

            return new PositionCheckState();
        }

        public async Task DownloadProfile(Session ctx, StateMachine machine)
        {
            ctx.Profile = await ctx.Client.Player.GetPlayer();
            machine.Fire(new ProfileEvent {Profile = ctx.Profile});
        }
    }
}