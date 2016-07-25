using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.Exceptions;
using PokemonGo.RocketAPI.Logic.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Logic.State
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
                        ctx.Client.DoPtcLogin(ctx.Settings.PtcUsername, ctx.Settings.PtcPassword).Wait();
                        break;
                    case AuthType.Google:
                        ctx.Client.DoGoogleLogin().Wait();
                        break;
                    default:
                        machine.Fire(new ErrorEvent { Message= "wrong AuthType" });
                        return null;
                }

                ctx.Client.SetServer().Wait();
            }
            catch (PtcOfflineException)
            {
                machine.Fire(new ErrorEvent { Message = "PTC Servers are probably down OR your credentials are wrong. Try google" });
                machine.Fire(new NoticeEvent { Message = "Trying again in 20 seconds..." });
                machine.RequestDelay(20000);
                return this;
            }
            catch (AccountNotVerifiedException)
            {
                machine.Fire(new ErrorEvent { Message = "Account not verified. - Exiting" });
                return null;
            }

            DownloadProfile(ctx, machine);

            return new PositionCheckState();
        }

        public void DownloadProfile(Context ctx, StateMachine machine)
        {
            ctx.Profile = ctx.Client.GetProfile().Result;
            machine.Fire(new ProfileEvent { Profile = ctx.Profile });
        }
    }
}
