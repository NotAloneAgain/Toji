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

        public static bool CheckPermissions(this Player player, KeycardPermissions perms)
        {
            if (player.IsBypassModeEnabled || (int)perms <= 0 || player.HasItem(ItemType.KeycardO5) || perms == KeycardPermissions.ScpOverride && player.IsScp)
            {
                return true;
            }

            var keycards = player.GetKeycards().DistinctWhere((Keycard first, Keycard second) => first.Type == second.Type && first.Permissions == second.Permissions);

            foreach (var keycard in keycards)
            {
                if (keycard == null || (int)keycard.Permissions <= 0 || !keycard.Permissions.HasFlagFast(perms))
                {
                    continue;
                }

                return true;
            }

            return false;
        }

        public static bool HasFlagFast(this KeycardPermissions perm, KeycardPermissions other)
        {
            var value = perm & other;

            return value == other;
        }
    }
}
