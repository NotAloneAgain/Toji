using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System;
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
                    Delegate action = subclass.OnEscaped;

                    action.CallDelayed(default, ev.Player);

                    return;
                }

                if (subclass.Characteristics.Find(x => x is RespawnCharacteristic) is RespawnCharacteristic respawn && respawn.Value && ev.Reason != SpawnReason.Destroyed)
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

                sub.DelayedAssign(ev.Player);

                break;
            }
        }

        public void OnDestroying(DestroyingEventArgs ev)
        {
            if (!ev.IsValid(false) || !ev.Player.TryGetSubclass(out var subclass))
            {
                return;
            }

            subclass.Revoke(ev.Player);
        }

        public void OnHurting(HurtingEventArgs ev)
        {
            if (ev == null || !ev.IsAllowed || !ev.IsValid() || ev.Amount <= 0)
            {
                return;
            }

            if (ev.Player.TryGetSubclass(out var subclass))
            {
                if (subclass is IHurtController controller)
                {
                    try
                    {
                        controller.OnHurt(ev);
                    }
                    catch (Exception err)
                    {
                        Log.Warn($"Error on processing hurt controller of {subclass.GetType().Name}: {err}");
                    }
                }

                if (subclass.Abilities.Any())
                {
                    foreach (var ability in subclass.Abilities)
                    {
                        if (ability == null || ability is not IHurtController abilityController)
                        {
                            continue;
                        }

                        try
                        {
                            abilityController.OnHurt(ev);
                        }
                        catch (Exception err)
                        {
                            Log.Warn($"Error on processing hurt controller of {ability.GetType().Name}: {err}");
                        }
                    }
                }

                if (subclass.Characteristics.Any())
                {
                    foreach (var characteristic in subclass.Characteristics)
                    {
                        if (characteristic == null || characteristic is not HurtMultiplayerCharacteristic multiplayer)
                        {
                            continue;
                        }

                        try
                        {
                            ev.Amount *= multiplayer.Value;
                        }
                        catch (Exception err)
                        {
                            Log.Warn($"Error on processing hurt multiplayer of {characteristic.GetType().Name}: {err}");
                        }
                    }
                }
            }

            if (ev.IsNotSelfDamage() && ev.Attacker.TryGetSubclass(out var attackerSubclass))
            {
                if (attackerSubclass is IDamageController controller)
                {
                    try
                    {
                        controller.OnDamage(ev);
                    }
                    catch (Exception err)
                    {
                        Log.Warn($"Error on processing damage controller of {subclass.GetType().Name}: {err}");
                    }
                }

                if (subclass.Abilities.Any())
                {
                    foreach (var ability in subclass.Abilities)
                    {
                        if (ability == null || ability is not IDamageController abilityController)
                        {
                            continue;
                        }

                        try
                        {
                            abilityController.OnDamage(ev);
                        }
                        catch (Exception err)
                        {
                            Log.Warn($"Error on processing damage controller of {ability.GetType().Name}: {err}");
                        }
                    }
                }

                if (subclass.Characteristics.Any())
                {
                    foreach (var characteristic in subclass.Characteristics)
                    {
                        if (characteristic == null)
                        {
                            continue;
                        }

                        if (characteristic is DamageCharacteristic damage)
                        {
                            try
                            {
                                ev.Amount = damage.Value;
                            }
                            catch (Exception err)
                            {
                                Log.Warn($"Error on processing damage value of {characteristic.GetType().Name}: {err}");
                            }
                        }

                        if (characteristic is DamageMultiplayerCharacteristic multiplayer)
                        {
                            try
                            {
                                ev.Amount *= multiplayer.Value;
                            }
                            catch (Exception err)
                            {
                                Log.Warn($"Error on processing damage multiplayer of {characteristic.GetType().Name}: {err}");
                            }
                        }
                    }
                }
            }
        }
    }
}
