﻿using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerRoles;

namespace Toji.BetterEscape.API
{
    public static class Extensions
    {
        public static void HandleEscape(this Player player, bool isCuffed)
        {
            if (!isCuffed && player.Role.Type == RoleTypeId.FacilityGuard)
            {
                player.Role.Set(RoleTypeId.NtfPrivate, SpawnReason.Escaped, RoleSpawnFlags.All);

                return;
            }

            var role = player.Role.Team == Team.ChaosInsurgency ? RoleTypeId.NtfPrivate : RoleTypeId.ChaosConscript;

            player.Role.Set(role, SpawnReason.Escaped, RoleSpawnFlags.All);
        }
    }
}
