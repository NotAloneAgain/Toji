using Exiled.API.Enums;
using Exiled.API.Features.Pickups;
using MapGeneration.Distributors;
using System.Linq;
using Toji.NwPluginAPI.API.Extensions;
using UnityEngine;

namespace Toji.Global
{
    public static class LockerExtensions
    {
        public static bool IsArmory(this Locker locker) => locker.StructureType switch
        {
            StructureType.LargeGunLocker => true,
            _ => false,
        };

        public static KeycardPermissions GetPermissions(this Locker locker, LockerChamber chamber) => locker.StructureType switch
        {
            StructureType.LargeGunLocker => chamber.HasDanger() ? KeycardPermissions.ArmoryLevelTwo : KeycardPermissions.ArmoryLevelOne,
            StructureType.ScpPedestal => KeycardPermissions.ContainmentLevelTwo | KeycardPermissions.Checkpoints,
            _ => KeycardPermissions.None,
        };

        public static bool HasDanger(this LockerChamber chamber, bool checkGrenades = false) => chamber.AcceptableItems.Any(item => item.IsWeapon() || checkGrenades && item.IsGrenade());
    }
}
