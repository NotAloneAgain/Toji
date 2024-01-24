using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
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

            Action<Vector3> action = ev.Player.Teleport;

            if (!Extensions.ReplaceQueue.Any())
            {
                ev.NewRole = RoleTypeId.ChaosRifleman;
            }
            else
            {
                ev.NewRole = Extensions.ReplaceQueue.Dequeue();
            }

            action.CallDelayed(0.05f, Extensions.ChaosSpawns.GetRandomValue());
        }
    }
}
