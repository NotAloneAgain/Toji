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
            if (!ev.IsAllowed || ev.Player == null)
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

                if (subclass.Characteristics.Find(x => x is RespawnCharacteristic) is RespawnCharacteristic respawn && respawn.Value)
                {
                    return;
                }

                subclass.Revoke(ev.Player);

                return;
            }

            if (!isPlayable || !BaseSubclass.TryGet(ev.NewRole, out var subclasses))
            {
                return;
            }

            foreach (var sub in subclasses)
            {
                if (!sub.Can(ev.Player))
                {
                    continue;
                }

                sub.DelayedAssign(ev.Player);
            }
        }

        public void OnHurting(HurtingEventArgs ev)
        {
            if (!ev.IsAllowed || !ev.IsValid())
            {
                return;
            }

            if (ev.Player.TryGetSubclass(out var subclass))
            {
                if (subclass is IHurtController controller)
                {
                    controller.OnHurt(ev);
                }

                if (subclass.Abilities.Any())
                {
                    foreach (var ability in subclass.Abilities)
                    {
                        if (ability is not IHurtController abilityController)
                        {
                            continue;
                        }

                        abilityController.OnHurt(ev);
                    }
                }

                if (subclass.Characteristics.Find(x => x is HurtMultiplayerCharacteristic) is HurtMultiplayerCharacteristic multiplayer && multiplayer.Value != 1)
                {
                    ev.Amount *= multiplayer.Value;
                }
            }

            if (ev.IsNotSelfDamage() && ev.Attacker.TryGetSubclass(out var attackerSubclass))
            {
                if (attackerSubclass is IDamageController controller)
                {
                    controller.OnDamage(ev);
                }

                if (subclass.Abilities.Any())
                {
                    foreach (var ability in subclass.Abilities)
                    {
                        if (ability is not IDamageController abilityController)
                        {
                            continue;
                        }

                        abilityController.OnDamage(ev);
                    }
                }

                if (attackerSubclass.Characteristics.Find(x => x is DamageCharacteristic) is DamageCharacteristic damage)
                {
                    ev.Amount = damage.Value;
                }

                if (attackerSubclass.Characteristics.Find(x => x is DamageMultiplayerCharacteristic) is DamageMultiplayerCharacteristic multiplayer && multiplayer.Value != 1)
                {
                    ev.Amount *= multiplayer.Value;
                }
            }
        }
    }
}
