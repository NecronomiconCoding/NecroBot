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
        public async Task<IState> Execute(Context ctx, StateMachine machine)
        {
            machine.Fire(new NoticeEvent {Message = $"Logging in using {ctx.Settings.AuthType}"});
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
                        machine.Fire(new ErrorEvent {Message = "wrong AuthType"});
                        return null;
                }
            }
            catch (PtcOfflineException)
            {
                machine.Fire(new ErrorEvent
                {
                    Message = "PTC Servers are probably down OR your credentials are wrong. Try google"
                });
                machine.Fire(new NoticeEvent {Message = "Trying again in 20 seconds..."});
                await Task.Delay(20000);
                return this;
            }
            catch (AccountNotVerifiedException)
            {
                machine.Fire(new ErrorEvent {Message = "Account not verified. - Exiting"});
                return null;
            }

            await DownloadProfile(ctx, machine);

            return new PositionCheckState();
        }

        public async Task DownloadProfile(Context ctx, StateMachine machine)
        {
            ctx.Profile = await ctx.Client.Player.GetPlayer();
            machine.Fire(new ProfileEvent {Profile = ctx.Profile});
        }
    }
}