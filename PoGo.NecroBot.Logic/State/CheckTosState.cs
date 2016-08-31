#region using directives

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using POGOProtos.Data.Player;
using POGOProtos.Enums;
using POGOProtos.Networking.Responses;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Utils;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public class CheckTosState : IState
    {
        public async Task<IState> Execute(ISession session, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (session.LogicSettings.AutoCompleteTutorial)
            {
                var tutState = session.Profile.PlayerData.TutorialState;
                if (!tutState.Contains(TutorialState.LegalScreen))
                {
                    await
                        session.Client.Misc.MarkTutorialComplete(new RepeatedField<TutorialState>()
                        {
                            TutorialState.LegalScreen
                        });
                    session.EventDispatcher.Send(new NoticeEvent()
                    {
                        Message = "Just read the Niantic ToS, looks legit, accepting!"
                    });
                    await DelayingUtils.DelayAsync(9000, 2000);
                }
                if (!tutState.Contains(TutorialState.AvatarSelection))
                {
                    var gen = Gender.Male;
                    switch (session.LogicSettings.DesiredGender)
                    {
                        case "Male":
                            gen = Gender.Male;
                            break;
                        case "Female":
                            gen = Gender.Female;
                            break;
                        default:
                            session.EventDispatcher.Send(new NoticeEvent()
                            {
                                Message = "You didn't set a valid gender, setting to default: MALE"
                            });
                            //I know it is useless, but I prefer keep it
                            gen = Gender.Male;
                            break;
                    }
                    var avatarRes = await session.Client.Player.SetAvatar(new PlayerAvatar()
                    {
                        Backpack = 0,
                        Eyes = 0,
                        Gender = gen,
                        Hair = 0,
                        Hat = 0,
                        Pants = 0,
                        Shirt = 0,
                        Shoes = 0,
                        Skin = 0
                    });
                    if (avatarRes.Status == SetAvatarResponse.Types.Status.AvatarAlreadySet ||
                        avatarRes.Status == SetAvatarResponse.Types.Status.Success)
                    {
                        await session.Client.Misc.MarkTutorialComplete(new RepeatedField<TutorialState>()
                        {
                            TutorialState.AvatarSelection
                        });
                        session.EventDispatcher.Send(new NoticeEvent()
                        {
                            Message = $"Selected your avatar, now you are {gen}!"
                        });
                    }
                }
                if (!tutState.Contains(TutorialState.PokemonCapture))
                {
                    await CatchFirstPokemon(session);
                }
                if (!tutState.Contains(TutorialState.NameSelection))
                {
                    await SelectNicnname(session);
                }
                if (!tutState.Contains(TutorialState.FirstTimeExperienceComplete))
                {
                    await
                        session.Client.Misc.MarkTutorialComplete(new RepeatedField<TutorialState>()
                        {
                            TutorialState.FirstTimeExperienceComplete
                        });
                    session.EventDispatcher.Send(new NoticeEvent()
                    {
                        Message = "First time experience complete, looks like i just spinned an virtual pokestop :P"
                    });
                    await DelayingUtils.DelayAsync(3000, 2000);
                }
            }
            return new FarmState();
        }

        public async Task<bool> CatchFirstPokemon(ISession session)
        {
            var firstPokeList = new List<PokemonId>
            {
                PokemonId.Bulbasaur,
                PokemonId.Charmander,
                PokemonId.Squirtle
            };
            var firstpokenum = 0;
            switch (session.LogicSettings.DesiredStarter)
            {
                case "Bulbasaur":
                    firstpokenum = 0;
                    break;
                case "Charmander":
                    firstpokenum = 1;
                    break;
                case "Squirtle":
                    firstpokenum = 2;
                    break;
                default:
                    session.EventDispatcher.Send(new NoticeEvent()
                    {
                        Message = "You didn't set a valid starter, setting to default: Bulbasaur"
                    });
                    //I know it is useless, but I prefer keep it
                    firstpokenum = 0;
                    break;
            }
            
            var firstPoke = firstPokeList[firstpokenum];

            var res = await session.Client.Encounter.EncounterTutorialComplete(firstPoke);
            await DelayingUtils.DelayAsync(7000, 2000);
            if (res.Result != EncounterTutorialCompleteResponse.Types.Result.Success) return false;
            session.EventDispatcher.Send(new NoticeEvent()
            {
                Message = $"Caught Tutorial pokemon! it's {firstPoke}!"
            });
            return true;
        }

        public async Task<bool> SelectNicnname(ISession session)
        {
            if (string.IsNullOrEmpty(session.LogicSettings.DesiredNickname))
            {
                session.EventDispatcher.Send(new NoticeEvent()
                {
                    Message = "You didn't pick the desired nickname!"
                });
                return false;
            }

            if (session.LogicSettings.DesiredNickname.Length > 15)
            {
                session.EventDispatcher.Send(new NoticeEvent()
                {
                    Message = "You selected too long Desired name, max length: 15!"
                });
                return false;
            }
			
            var res = await session.Client.Misc.ClaimCodename(session.LogicSettings.DesiredNickname);
            if (res.Status == ClaimCodenameResponse.Types.Status.Success)
            {
                session.EventDispatcher.Send(new NoticeEvent()
                {
                    Message = $"Your name is now: {res.Codename}"
                });
                await session.Client.Misc.MarkTutorialComplete(new RepeatedField<TutorialState>()
                        {
                            TutorialState.NameSelection
                        });
            }
            else if (res.Status == ClaimCodenameResponse.Types.Status.CodenameChangeNotAllowed || res.Status == ClaimCodenameResponse.Types.Status.CurrentOwner)
            {
                await session.Client.Misc.MarkTutorialComplete(new RepeatedField<TutorialState>()
                        {
                            TutorialState.NameSelection
                        });
            }
            else
            {
                var errorText = "Niantic error";
                switch (res.Status)
                {
                    case ClaimCodenameResponse.Types.Status.Unset:
                        errorText = "Unset, somehow";
                        break;
                    case ClaimCodenameResponse.Types.Status.Success:
                        errorText = "No errors, nickname changed";
                        break;
                    case ClaimCodenameResponse.Types.Status.CodenameNotAvailable:
                        errorText = "That nickname isn't available, pick another one and restart the bot!";
                        break;
                    case ClaimCodenameResponse.Types.Status.CodenameNotValid:
                        errorText = "That nickname isn't valid, pick another one!";
                        break;
                    case ClaimCodenameResponse.Types.Status.CurrentOwner:
                        errorText = "You already own that nickname!";
                        break;
                    case ClaimCodenameResponse.Types.Status.CodenameChangeNotAllowed:
                        errorText = "You can't change your nickname anymore!";
                        break;
                }

                session.EventDispatcher.Send(new NoticeEvent()
                {
                    Message = $"Name selection failed! Error: {errorText}"
                });

                // Pause here so the user can restart the bot.
                Console.ReadKey();
            }
            await DelayingUtils.DelayAsync(3000, 2000);
            return res.Status == ClaimCodenameResponse.Types.Status.Success;
        }
    }
}