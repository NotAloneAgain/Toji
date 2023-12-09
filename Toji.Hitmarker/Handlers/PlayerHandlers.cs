using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using Toji.ExiledAPI.Extensions;
using UnityEngine;

namespace Toji.Hitmarker.Handlers
{
    internal sealed class PlayerHandlers
    {
        private List<string> _deathTexts;

        internal PlayerHandlers(List<string> deathTexts)
        {
            _deathTexts = deathTexts;
        }

        public void OnHurting(HurtingEventArgs ev)
        {
            if (!ev.IsNotSelfDamage() || !ev.IsAllowed || !ev.DamageHandler.Type.IsValid() || ev.Player.LeadingTeam == ev.Attacker.LeadingTeam || ev.Amount <= 0)
            {
                return;
            }

            if (ev.Player.Role.Is<Scp3114Role>(out var role) && RoleExtensions.GetTeam(role.StolenRole).GetLeadingTeam() == ev.Attacker.LeadingTeam && !Server.FriendlyFire)
            {
                return;
            }

            ev.Attacker.ShowHint($"<line-height=90%><voffset=4.5em><size=88%><color=#E55807>{Mathf.RoundToInt(ev.Amount)}</color></size></voffset>", 1);
        }

        public void OnDying(DyingEventArgs ev)
        {
            if (!ev.IsNotSelfDamage() || !ev.IsAllowed || !ev.DamageHandler.Type.IsValid())
            {
                return;
            }

            ev.Attacker.ShowHint($"<line-height=90%><voffset=4.5em><size=88%><color=#7E1717>{_deathTexts.GetRandomValue()}</color></size></voffset>", 1.25f);
        }

        public void OnFailingEscapePocketDimension(FailingEscapePocketDimensionEventArgs ev)
        {
            if (!ev.IsValid() || !ev.IsAllowed)
            {
                return;
            }

            string text = $"<line-height=90%><voffset=4.5em><size=88%><color=#7E1717>{_deathTexts.GetRandomValue()}</color></size></voffset>";

            foreach (var player in Player.List)
            {
                if (player.Role.Type != RoleTypeId.Scp106)
                {
                    continue;
                }

                player.ShowHint(text, 1.25f);
            }
        }
    }
}
