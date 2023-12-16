using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System.Linq;
using Toji.Classes.API.Extensions;
using Toji.Classes.API.Features;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Characteristics;
using Toji.ExiledAPI.Extensions;

namespace Toji.Classes.Handlers
{
    internal sealed class PlayerHandlers
    {
        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (!ev.IsAllowed || !ev.IsValid(false))
            {
                return;
            }

            bool isSpawn = (int)ev.SpawnFlags == 3 || ev.SpawnFlags == RoleSpawnFlags.All;
            bool isPlayable = ev.NewRole is not RoleTypeId.None and not RoleTypeId.Overwatch and not RoleTypeId.Filmmaker and not RoleTypeId.Spectator;

            if (ev.Player.TryGetSubclass(out var subclass))
            {
                if (ev.NewRole == RoleTypeId.Tutorial && !isSpawn)
                {
                    return;
                }

                if (ev.Reason == SpawnReason.Escaped)
                {
                    System.Delegate action = subclass.OnEscaped;

                    action.CallDelayed(default, ev.Player);

                    return;
                }

                if (subclass.Characteristics.Find(x => x is RespawnCharacteristic) is RespawnCharacteristic respawn && respawn.Value && ev.Reason is not SpawnReason.ForceClass and not SpawnReason.Destroyed)
                {
                    return;
                }

                subclass.Revoke(ev.Player);
            }

            if (!isPlayable || !isSpawn || !BaseSubclass.TryGet(ev.NewRole, out var subclasses))
            {
                return;
            }

            foreach (var sub in subclasses)
            {
                if (!sub.Can(ev.Player))
                {
                    continue;
                }

                sub.DelayedAssign(ev.Player, 0.05f);
            }
        }

        public void OnHurting(HurtingEventArgs ev)
        {
            if (ev == null || !ev.IsAllowed || !ev.IsValid())
            {
                return;
            }

            if (ev.Player.TryGetSubclass(out var subclass))
            {
                if (subclass is IHurtController controller)
                {
                    controller?.OnHurt(ev);
                }

                if (subclass.Abilities?.Any() ?? false)
                {
                    foreach (var ability in subclass.Abilities)
                    {
                        if (ability == null || ability is not IHurtController abilityController)
                        {
                            continue;
                        }

                        abilityController.OnHurt(ev);
                    }
                }

                if (subclass.Characteristics?.Any() ?? false)
                {
                    foreach (var characteristic in subclass.Characteristics)
                    {
                        if (characteristic == null || characteristic is not HurtMultiplayerCharacteristic multiplayer)
                        {
                            continue;
                        }

                        ev.Amount *= multiplayer.Value;
                    }
                }
            }

            if (ev.IsNotSelfDamage() && ev.Attacker.TryGetSubclass(out var attackerSubclass))
            {
                if (attackerSubclass is IDamageController controller)
                {
                    controller?.OnDamage(ev);
                }

                if (subclass.Abilities?.Any() ?? false)
                {
                    foreach (var ability in subclass.Abilities)
                    {
                        if (ability == null || ability is not IDamageController abilityController)
                        {
                            continue;
                        }

                        abilityController.OnDamage(ev);
                    }
                }

                if (subclass.Characteristics?.Any() ?? false)
                {
                    foreach (var characteristic in subclass.Characteristics)
                    {
                        if (characteristic == null)
                        {
                            continue;
                        }

                        if (characteristic is DamageCharacteristic damage)
                        {
                            ev.Amount = damage.Value;
                        }

                        if (characteristic is DamageMultiplayerCharacteristic multiplayer)
                        {
                            ev.Amount *= multiplayer.Value;
                        }
                    }
                }
            }
        }
    }
}
