using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System;
using System.Linq;
using Toji.BetterCuff.API;
using UnityEngine;

namespace Toji.BetterCuff.Handlers
{
    internal sealed class PlayerHandlers
    {
        public void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Player == null || ev.Player.IsHost || ev.Player.IsDead || ev.Player.IsNPC || ev.Player.IsGodModeEnabled || !ev.Player.IsCuffed && ev.Player.Cuffer == null)
            {
                return;
            }

            var cuffer = ev.Player.Cuffer;

            var attackerFaction = ev.Attacker.GetFaction();
            var cufferFaction = cuffer.GetFaction();

            if (ev.Attacker == null || ev.Attacker.IsHost || ev.Attacker.IsDead || ev.Attacker.IsNPC || attackerFaction != cufferFaction)
            {
                return;
            }

            var enemyFaction = cuffer.GetEnemyFaction();

            if (HasEnemyInRoom(ev.Player, enemyFaction) || HasEnemyInRoom(ev.Attacker, enemyFaction))
            {
                return;
            }

            bool damageByCuffer = ev.Attacker.UserId == cuffer.UserId;

            ev.Amount *= damageByCuffer ? 0.75f : 0.5f;

            if (!damageByCuffer)
            {
                cuffer.SendConsoleMessage($"Связанный вами игрок '{ev.Player.CustomName}' был атакован игроком '{ev.Attacker.CustomName}'. Урон: {Mathf.RoundToInt(ev.Amount)}", "red");
            }
        }

        public void OnDying(DyingEventArgs ev)
        {
            if (ev.Player == null || ev.Player.IsHost || ev.Player.IsDead || ev.Player.IsNPC || ev.Player.IsGodModeEnabled || !ev.Player.IsCuffed && ev.Player.Cuffer == null)
            {
                return;
            }

            var cuffer = ev.Player.Cuffer;

            if (ev.Attacker == null || ev.Attacker.IsHost || ev.Attacker.IsNPC || cuffer.UserId == ev.Attacker.UserId)
            {
                return;
            }

            string text = $"Связанный вами игрок '{ev.Player.CustomName}' был убит игроком '{ev.Attacker.CustomName}'.";

            cuffer.SendConsoleMessage(text, "red");
            cuffer.ShowHint($"<line-height=95%><size=95%><voffset=-18em><color=#EE204D><b>{text}</b></color></voffset></size>", 5);
        }

        public void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (!ev.IsAllowed || !ev.Pickup.Type.IsWeapon() || ev.Player.Role.Type != RoleTypeId.ClassD)
            {
                return;
            }

            ev.Player.TryAddFriendlyFire(RoleTypeId.ClassD, 1);
        }

        public void OnEscaping(EscapingEventArgs ev)
        {
            if (!ev.IsAllowed || !ev.Player.FriendlyFireMultiplier.TryGetValue(RoleTypeId.ClassD, out _))
            {
                return;
            }

            ev.Player.FriendlyFireMultiplier.Remove(RoleTypeId.ClassD);
        }

        public void OnHandcuffing(HandcuffingEventArgs ev)
        {
            if (!ev.IsAllowed || !ev.Target.FriendlyFireMultiplier.TryGetValue(RoleTypeId.ClassD, out _))
            {
                return;
            }

            ev.Player.FriendlyFireMultiplier.Remove(RoleTypeId.ClassD);
        }

        private bool HasEnemyInRoom(Player player, Faction faction) => player.CurrentRoom.Players.Any(ply => !ply.IsCuffed && ply.GetFaction() == faction);
    }
}
