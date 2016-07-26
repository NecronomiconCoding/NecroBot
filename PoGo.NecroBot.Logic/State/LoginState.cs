#region using directives

using PoGo.NecroBot.Logic.Event;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.Exceptions;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public class LoginState : IState
    {
        public IState Execute(Context ctx, StateMachine machine)
        {
            try
            {
                switch (ctx.Settings.AuthType)
                {
                    case AuthType.Ptc:
                        try
                        {
                            ctx.Client.Login.DoPtcLogin(ctx.Settings.PtcUsername, ctx.Settings.PtcPassword).Wait();
                        }
                        catch (System.AggregateException ae)
                        {
                            throw ae.Flatten().InnerException;
                        }
                        break;
                    case AuthType.Google:
                        ctx.Client.Login.DoGoogleLogin().Wait();
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
                machine.RequestDelay(20000);
                return this;
            }
            catch (AccountNotVerifiedException)
            {
                machine.Fire(new ErrorEvent {Message = "Account not verified. - Exiting"});
                return null;
            }

            DownloadProfile(ctx, machine);

            return new InfoState();
        }

        public void DownloadProfile(Context ctx, StateMachine machine)
        {
            ctx.Profile = ctx.Client.Player.GetPlayer().Result;
            machine.Fire(new ProfileEvent {Profile = ctx.Profile});
        }
    }
}