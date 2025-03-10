﻿using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using Toji.Global;

namespace Toji.LastPlayers.Handlers
{
    internal sealed class PlayerHandlers
    {
        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            var team = ev.Player.Role.Team;

            if (Player.List.Count < 10 || team is Team.Dead or Team.OtherAlive || team == Team.SCPs && Player.List.First(ply => ply.Role.Team == Team.SCPs).Role.Type == RoleTypeId.Scp079 || Player.List.Count(ply => ply.Role.Team == team) != 1)
            {
                return;
            }

            Timing.RunCoroutine(_PrintMessages(Player.List.FirstOrDefault(ply => ply.Role.Team == team), team));
        }

        private IEnumerator<float> _PrintMessages(Player player, Team team)
        {
            yield return Timing.WaitForSeconds(0.00005f);

            if (player == null || player.IsHost || player.Role.Team != team)
            {
                yield break;
            }

            player.Broadcast(10, $"<color=#E55807><b>Вы последний {Translate(team)}!</b></color>");

            yield return Timing.WaitForSeconds(15);

            while (player.IsAlive && Player.List.Count(ply => ply.Role.Team == team) == 1)
            {
                Map.Broadcast(10, $"<color=#E55807><b>Последний {Translate(team)} находится {player.CurrentRoom.Zone.TranslateZone()}!</b></color>");

                yield return Timing.WaitForSeconds(60);
            }
        }

        private string Translate(Team team) => team switch
        {
            Team.SCPs => "SCP-Объект",
            Team.FoundationForces => "МОГовец",
            Team.ChaosInsurgency => "Хаосит",
            Team.Scientists => "Ученый",
            Team.ClassD => "Персонал класса D",
            Team.Dead => "Мертвец",
            _ => "Неизвестно",
        };
    }
}
