using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;
using System.Collections.Generic;
using UnityEngine;

namespace Toji.BetterRoles.Handlers
{
    internal sealed class PlayerHandlers
    {
        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (!ev.IsAllowed || ev.Player.IsHost || ev.Player.IsNPC)
            {
                return;
            }

            if (ev.NewRole == RoleTypeId.Scp079)
            {
                Timing.RunCoroutine(OnChangingToScp079(ev.Player));

                return;
            }

            if (ev.NewRole == RoleTypeId.Scp3114)
            {
                Timing.RunCoroutine(OnChangingToScp3114(ev.Player));

                return;
            }

            Team team = RoleExtensions.GetTeam(ev.NewRole);

            if (team is not Team.FoundationForces or Team.ChaosInsurgency || ev.NewRole != RoleTypeId.NtfPrivate && team == Team.FoundationForces || !SelectGrenade(out var item))
            {
                return;
            }

            ev.Player.AddItem(item);
        }

        public void OnEnteringPocketDimension(EnteringPocketDimensionEventArgs ev)
        {
            if (!ev.IsAllowed || ev.Player == null)
            {
                return;
            }

            if (Warhead.IsDetonated)
            {
                ev.IsAllowed = false;
                ev.Player.Kill(DamageType.Scp106);
            }
        }

        private bool SelectGrenade(out ItemType item)
        {
            if (Random.Range(0, 101) < 46)
            {
                item = ItemType.None;

                return false;
            }

            item = Random.Range(0, 101) switch
            {
                >= 49 => ItemType.GrenadeHE,
                _ => ItemType.GrenadeFlash
            };

            return true;
        }

        private IEnumerator<float> OnChangingToScp079(Player player)
        {
            yield return Timing.WaitForSeconds(0.0003f);

            var scp = player.Role.Base as Scp079Role;

            if (scp == null || player.IsHost || player.IsNPC || !scp.SubroutineModule.TryGetSubroutine(out Scp079LostSignalHandler lost))
            {
                yield break;
            }

            lost._ghostlightLockoutDuration = 15;
        }

        private IEnumerator<float> OnChangingToScp3114(Player player)
        {
            yield return Timing.WaitForSeconds(0.0003f);

            player.MaxHealth = 1250;
            player.Health = 1250;

            var transform = Door.Get(DoorType.Scp173Gate).GameObject.transform;

            var position = transform.position;
            var rotation = transform.rotation;

            var direction = rotation * Vector3.back;

            player.Teleport(position + Vector3.up * 2 + direction * 4);
        }
    }
}
