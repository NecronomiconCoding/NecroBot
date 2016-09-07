#region using directives

using System;
using PokemonGo.RocketAPI.Extensions;
using POGOProtos.Map.Fort;

using POGOProtos.Networking.Responses;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{

    public class SetMoveToTargetTask
    {
        public static Boolean SetMoveToTargetEnabled { get; set; } = false;
        public static Boolean SetMoveToTargetAccept { get; set; } = false;
        public static double SetMoveToTargetLat { get; set; }
        public static double SetMoveToTargetLng { get; set; }


        public static void CheckSetMoveToTargetStatus(ref FortDetailsResponse fortInfo,ref FortData pokeStop)
        {
            if (SetMoveToTargetEnabled)
            {
                SetMoveToTargetAccept = true;
                fortInfo.Name = "User Destination.";
                fortInfo.Latitude = pokeStop.Latitude = SetMoveToTargetLat;
                fortInfo.Longitude = pokeStop.Longitude = SetMoveToTargetLng;
            }
        }
        public static bool CheckStopforSetMoveToTarget()
        {
            return (SetMoveToTargetEnabled && !SetMoveToTargetAccept);
        }
        public static bool CheckReachTarget(ISession session)
        {
            if (SetMoveToTargetEnabled && SetMoveToTargetAccept)
            {
                session.EventDispatcher.Send(new FortUsedEvent
                {
                    Id = "",
                    Name = "User Destination.",
                    Exp = 0,
                    Gems = 0,
                    Items = "",
                    Latitude = SetMoveToTargetLat,
                    Longitude = SetMoveToTargetLng,
                    InventoryFull = false
                });
                SetMoveToTargetAccept = false;
                SetMoveToTargetEnabled = false;
                return true;
            }
            return false;
        }
        public static void Execute(double lat ,double lng)
        {
            SetMoveToTargetLat = lat;
            SetMoveToTargetLng = lng;
            SetMoveToTargetEnabled = true;
        }
    }
}