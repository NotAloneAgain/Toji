using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System;
using Toji.Classes.API.Extensions;
using Toji.ExiledAPI.Extensions;
using Toji.ReduxRespawning.API;
using UnityEngine;

namespace Toji.ReduxRespawning.Handlers
{
    internal sealed class PlayerHandlers
    {
        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (!ev.IsAllowed || !ev.IsValid(false) || !Extensions.ReplaceToChaos)
            {
                return;
            }

            if (ev.NewRole != RoleTypeId.FacilityGuard || ev.Reason is not SpawnReason.RoundStart and not SpawnReason.LateJoin)
            {
                return;
            }

            ev.NewRole = RoleTypeId.Spectator;
            ev.SpawnFlags = RoleSpawnFlags.AssignInventory;

            if (ev.Reason == SpawnReason.LateJoin)
            {
                ev.NewRole = RoleTypeId.ChaosRifleman;

                Action<Vector3> action = ev.Player.Teleport;

                action.CallDelayed(default, Extensions.ChaosSpawns.GetRandomValue());

                return;
            }

            ev.Player.AddToWaiting();
        }
    }
}
