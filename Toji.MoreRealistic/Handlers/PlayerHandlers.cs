using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using Toji.ExiledAPI.Extensions;
using UnityEngine;

namespace Toji.MoreRealistic.Handlers
{
    internal sealed class PlayerHandlers
    {
        private const string HintText
            = "<line-height=95%><size=95%><voffset=-20em><color=#E32636>Вы слышите <i>циньк</i>, но звука не происходит...</color></size></voffset>";

        public void OnShot(ShotEventArgs ev)
        {
            if (!ev.IsValid() || ev.Player.CurrentItem.Type is ItemType.ParticleDisruptor or ItemType.GunShotgun || ev.Player.IsScp)
            {
                return;
            }

            Firearm weapon = ev.Player.CurrentItem.As<Firearm>();

            if (weapon == null || weapon.Ammo >= Mathf.RoundToInt(weapon.MaxAmmo * 0.67f))
            {
                return;
            }

            if (Random.Range(0, 101) >= 99)
            {
                ev.Player.CurrentItem.As<Firearm>().Ammo = 0;

                ev.Player.ShowHint(HintText, 6);
            }
        }

        public void OnHurting(HurtingEventArgs ev)
        {
            if (!ev.IsAllowed || !ev.IsNotSelfDamage() || !ev.DamageHandler.Type.IsValid() || ev.Player.IsScp || ev.Player.LeadingTeam == ev.Attacker.LeadingTeam)
            {
                return;
            }

            ItemType armor = ev.Player.CurrentArmor?.Type ?? ItemType.None;

            if (ev.Attacker.CurrentItem?.IsWeapon ?? false)
            {
                float duration = armor switch
                {
                    ItemType.ArmorHeavy => 0.25f,
                    ItemType.ArmorCombat => 3,
                    ItemType.ArmorLight => 4,
                    _ => 5
                };

                ev.Player.EnableEffect(EffectType.Bleeding, duration, true);
            }

            if (ev.DamageHandler.Type == DamageType.Scp0492)
            {
                ev.Player.Stamina -= 0.03f;

                int chance = armor switch
                {
                    ItemType.ArmorHeavy => 100,
                    ItemType.ArmorCombat => 98,
                    ItemType.ArmorLight => 96,
                    _ => 94
                };

                if (Random.Range(0, 100) >= chance)
                {
                    ev.Player.DropHeldItem();
                }
            }

            if (ev.DamageHandler.Type == DamageType.Scp939)
            {
                float duration = armor switch
                {
                    ItemType.ArmorHeavy => 1,
                    ItemType.ArmorCombat => 4,
                    ItemType.ArmorLight => 5,
                    _ => 6
                };

                ev.Player.EnableEffect(EffectType.Bleeding, duration, true);

                ev.Amount = Mathf.Clamp(ev.Player.MaxHealth * 0.4f, 25, 65);
            }
         }

        public void OnDying(DyingEventArgs ev)
        {
            if (!ev.IsAllowed || !ev.IsFullyValid())
            {
                return;
            }

            if (ev.DamageHandler.Type != DamageType.Poison || ev.Attacker.Role.Type != RoleTypeId.Scp0492 || Random.Range(0, 100) < 88)
            {
                return;
            }

            ev.IsAllowed = false;

            if (!ev.Player.IsInventoryEmpty)
            {
                ev.Player.DropAllWithoutKeycard();
            }

            ev.Player.Role.Set(RoleTypeId.Scp0492, SpawnReason.Revived, RoleSpawnFlags.None);

            ev.Player.CurrentItem = null;
        }
    }
}
