using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using UnityEngine;
using Toji.ExiledAPI.Extensions;

namespace Toji.MoreRealistic.Handlers
{
    internal sealed class PlayerHandlers
    {
        private const string HintText
            = "<line-height=95%><size=95%><voffset=-20em><color=#E32636>Вы слышите <i>циньк</i>, но звука не происходит...</color></size></voffset>";

        public void OnShot(ShotEventArgs ev)
        {
            if (ev.Player.CurrentItem.Type is ItemType.ParticleDisruptor or ItemType.GunShotgun || !ev.CanHurt || ev.Target == null || ev.Player.IsScp || ev.Player.IsHost || ev.Player.IsNPC)
            {
                return;
            }

            Firearm weapon = ev.Player.CurrentItem.As<Firearm>();

            if (weapon == null || weapon.Ammo >= Mathf.RoundToInt(weapon.MaxAmmo * 0.6f))
            {
                return;
            }

            if (Random.Range(0, 100) == 100)
            {
                ev.Player.CurrentItem.As<Firearm>().Ammo = 0;

                ev.Player.ShowHint(HintText, 6);
            }
        }
        public void OnHurting(HurtingEventArgs ev)
        {
            if (!ev.IsAllowed || !ev.IsNotSelfDamage() || ev.Player.LeadingTeam == ev.Attacker.LeadingTeam || ev.DamageHandler.Type is DamageType.Explosion or DamageType.Scp018)
            {
                return;
            }

            var isHuman = ev.DamageHandler.Type.IsHumanDamage() || ev.Attacker.CurrentItem != null && ev.Attacker.CurrentItem.IsWeapon && ev.DamageHandler.Type != DamageType.Explosion;

            if (ev.DamageHandler.Type is not DamageType.PocketDimension and not DamageType.Poison and not DamageType.Bleeding && (isHuman || ev.Attacker.IsScp) && ev.Amount > 0)
            {
                if (ev.Player.Role.Is<Exiled.API.Features.Roles.Scp3114Role>(out var role) && RoleExtensions.GetTeam(role.StolenRole).GetLeadingTeam() == ev.Attacker.LeadingTeam)
                {
                    return;
                }

                if (ev.Player.Health - ev.Amount <= 0)
                {
                    ev.Attacker.ShowHint($"<line-height=95%><voffset=5em><size=90%><color=#E55807>Убит!</color></size></voffset>", 1);
                }
                else
                {
                    ev.Attacker.ShowHint($"<line-height=95%><voffset=5em><size=90%><color=#E55807>{Mathf.RoundToInt(ev.Amount)}</color></size></voffset>", 1);
                }
            }

            if (!ev.Player.IsHuman || (ev.Player.CurrentArmor?.Type ?? ItemType.None) == ItemType.ArmorHeavy || ev.Player.Health - ev.Amount <= 0)
            {
                return;
            }

            if (isHuman)
            {
                ev.Player.EnableEffect(EffectType.Bleeding, 4, true);
            }
            else if (ev.DamageHandler.Type == DamageType.Scp0492)
            {
                ev.Player.Stamina -= 0.03f;

                if (Random.Range(0, 100) >= 98)
                {
                    ev.Player.DropHeldItem();
                }
            }
            else if (ev.DamageHandler.Type == DamageType.Scp939)
            {
                ev.Player.EnableEffect(EffectType.Bleeding, 6, true);

                ev.Amount = Mathf.Clamp(ev.Player.MaxHealth * 0.4f, 20, 60);
            }
         }

        public void OnDying(DyingEventArgs ev)
        {
            if (!ev.IsAllowed || !ev.IsValid())
            {
                return;
            }

            if (ev.DamageHandler.Type != DamageType.Poison && (ev.Attacker == null || ev.Attacker.Role.Type != RoleTypeId.Scp0492) || Random.Range(0, 100) < 88)
            {
                return;
            }

            ev.IsAllowed = false;

            ev.Player.DropAllWithoutKeycard();

            ev.Player.Role.Set(RoleTypeId.Scp0492, SpawnReason.Revived, RoleSpawnFlags.None);
        }
    }
}
