using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;
using System.Collections.Generic;
using Toji.ExiledAPI.Extensions;
using UnityEngine;

namespace Toji.BetterRoles.Handlers
{
    internal sealed class PlayerHandlers
    {
        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (!ev.IsAllowed || !ev.IsValid(false))
            {
                return;
            }

            ev.Player.IsUsingStamina = true;

            if (ev.NewRole == RoleTypeId.Scp079)
            {
                Timing.RunCoroutine(OnChangingToScp079(ev.Player));

                return;
            }

            if (ev.NewRole.IsFlamingo())
            {
                Timing.RunCoroutine(OnChangingToFlamingo(ev.Player, ev.NewRole == RoleTypeId.AlphaFlamingo));

                return;
            }

            if (ev.NewRole == RoleTypeId.Scp3114 && ev.SpawnFlags == RoleSpawnFlags.All)
            {
                Timing.RunCoroutine(OnChangingToScp3114(ev.Player));

                return;
            }

            Team team = RoleExtensions.GetTeam(ev.NewRole);

            if (team is not Team.FoundationForces and not Team.ChaosInsurgency || ev.NewRole != RoleTypeId.NtfPrivate && team == Team.FoundationForces || !SelectGrenade(out var item))
            {
                return;
            }

            ev.Items.Add(item);
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
            if (Random.Range(0, 100) < 46)
            {
                item = ItemType.None;

                return false;
            }

            item = Random.Range(0, 100) switch
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

        private IEnumerator<float> OnChangingToFlamingo(Player player, bool isAlpha)
        {
            yield return Timing.WaitForSeconds(0.0003f);

            player.MaxHealth = isAlpha ? 550 : 400;
            player.Health = isAlpha ? 550 : 400;
            player.EnableEffect(EffectType.MovementBoost, 8, 0);
            player.AddItem(ItemType.ArmorHeavy);
            player.IsUsingStamina = false;
        }

        private IEnumerator<float> OnChangingToScp3114(Player player)
        {
            yield return Timing.WaitForSeconds(0.0003f);

            player.MaxHealth = 1100;
            player.Health = 1100;

            var transform = Door.Get(DoorType.Scp173Gate).GameObject.transform;

            var position = transform.position;
            var rotation = transform.rotation;

            var direction = rotation * Vector3.back;

            player.Teleport(position + Vector3.up * 2 + direction * 4);
        }
    }
}
