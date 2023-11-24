using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using Toji.Classes.API.Extensions;
using Toji.Classes.API.Features;
using Toji.Classes.API.Interfaces;
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
                if (ev.NewRole == RoleTypeId.Tutorial && isSpawn)
                {
                    return;
                }

                if (ev.NewRole == RoleTypeId.Spectator && (subclass is not ILifesSubclass lifes || !lifes.KeepAfterDeath))
                {
                    return;
                }

                if (ev.Reason == SpawnReason.Escaped)
                {
                    System.Delegate action = subclass.OnEscaped;

                    action.CallDelayed(default, ev.Player);

                    return;
                }

                subclass.Revoke(ev.Player);
            }

            if (!isPlayable || !BaseSubclass.TryGet(ev.NewRole, out var subclasses))
            {
                return;
            }

            foreach (var sub in subclasses)
            {
                if (!sub.Can(ev.Player))
                {
                    return;
                }

                sub.DelayedAssign(ev.Player);
            }
        }

        public void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            if (!ev.Player.TryGetSubclass(out var subclass) || subclass is not ICustomTeslaSubclass customTesla)
            {
                return;
            }

            ev.IsTriggerable = customTesla.TriggeringTesla;
            ev.IsAllowed = customTesla.TriggeringTesla;
        }

        public void OnHurting(HurtingEventArgs ev)
        {
            if (!ev.IsAllowed || !ev.IsValid())
            {
                return;
            }

            if (ev.Player.TryGetSubclass(out var subclass) && subclass is ICustomHurtSubclass customHurt)
            {
                customHurt.OnHurt(ev);

                if (customHurt.HurtMultiplayer != 1)
                {
                    ev.Amount *= customHurt.HurtMultiplayer;
                }
            }

            if (ev.IsNotSelfDamage() && ev.Attacker.TryGetSubclass(out var attackerSubclass) && attackerSubclass is ICustomDamageSubclass customDamage)
            {
                customDamage.OnDamage(ev);

                if (customDamage.Damage != -2)
                {
                    ev.Amount = customDamage.Damage;
                }

                if (customDamage.DamageMultiplayer != 1)
                {
                    ev.Amount *= customDamage.DamageMultiplayer;
                }
            }
        }
    }
}
