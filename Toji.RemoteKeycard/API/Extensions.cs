using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pools;
using MapGeneration.Distributors;
using System;
using System.Collections.Generic;
using System.Linq;
using Toji.Global;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

namespace Toji.RemoteKeycard.API
{
    public static class Extensions
    {
        public static List<Keycard> GetKeycards(this Player player)
        {
            List<Keycard> result = new(8);

            foreach (var item in player.Items)
            {
                if (item == null || item.Category != ItemCategory.Keycard || item is not Keycard keycard)
                {
                    continue;
                }

                result.Add(keycard);
            }

            return result;
        }

        public static bool CheckPermissions(this Player player, KeycardPermissions perms, bool checkAll = false)
        {
            if (player.IsBypassModeEnabled || (int)perms <= 0 || player.HasItem(ItemType.KeycardO5) || perms == KeycardPermissions.None || perms == KeycardPermissions.ScpOverride && player.IsScp)
            {
                return true;
            }

            var keycards = player.GetKeycards().DistinctWhere((Keycard first, Keycard second) => first.Type == second.Type && first.Permissions == second.Permissions);

            Func<KeycardPermissions, KeycardPermissions, bool> func = checkAll ? HasAllFlagsFast : HasFlagFast;

            foreach (var keycard in keycards)
            {
                if (keycard == null || keycard.Permissions == KeycardPermissions.None || !func(keycard.Permissions, perms))
                {
                    continue;
                }

                return true;
            }

            return false;
        }

        public static bool CheckPermissions(this Player player, Locker locker, LockerChamber chamber)
        {
            bool hasAccess = player.CheckPermissions(locker.GetPermissions(chamber), true);
            bool hasChkp = player.CheckPermissions(KeycardPermissions.Checkpoints);
            bool hasCont = player.CheckPermissions(KeycardPermissions.Checkpoints);

            return hasAccess || !chamber.HasDanger() && hasChkp && hasCont;
        }

        public static bool HasFlagFast(this KeycardPermissions perm, KeycardPermissions other)
        {
            var value = perm & other;

            return value != 0 || value == other;
        }

        public static bool HasAllFlagsFast(this KeycardPermissions perm, KeycardPermissions other)
        {
            var value = perm & other;

            return value == 0;
        }
    }
}
