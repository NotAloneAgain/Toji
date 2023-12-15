using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Toji.Pocket.API
{
    public static class Extensions
    {
        private static Dictionary<Player, DateTime> _enterTime = new(Server.MaxPlayerCount);
        private static HashSet<Player> _players = new(Server.MaxPlayerCount);

        public static bool IsInPortal(this Player player) => _players.Contains(player);

        public static void RegisterEntering(this Player player)
        {
            if (player.IsInPortal() || _enterTime.ContainsKey(player))
            {
                return;
            }

            _enterTime.Add(player, DateTime.Now);
        }

        public static double GetStayingTime(this Player player)
        {
            if (player.IsInPortal())
            {
                return 0;
            }

            if (_enterTime.TryGetValue(player, out var time))
            {
                return (DateTime.Now - time).TotalSeconds;
            }
            else
            {
                player.RegisterEntering();

                return 0;
            }
        }

        public static void PortalTeleport(this Player player) => Timing.RunCoroutine(PortalAnimation(player));

        private static IEnumerator<float> PortalAnimation(Player player)
        {
            if (!player.Role.Is<FpcRole>(out var role))
            {
                yield break;
            }

            _players.Add(player);

            bool inGodMode = player.IsGodModeEnabled;
            bool isUsingStamina = player.IsUsingStamina;
            player.IsGodModeEnabled = true;
            player.IsUsingStamina = false;

            var startPosition = player.Position;
            var endPosition = player.Position - Vector3.up * 10 * player.GameObject.transform.localScale.y;

            for (float i = 0; i < 50; i++)
            {
                player.Position = Vector3.LerpUnclamped(startPosition, endPosition, i / 50f);

                yield return Timing.WaitForSeconds(0.02f);
            }

            if (Warhead.IsDetonated)
            {
                player.IsGodModeEnabled = inGodMode;

                player.Kill(DamageType.Warhead);

                yield break;
            }

            yield return Timing.WaitForSeconds(1);

            player.IsUsingStamina = isUsingStamina;
            player.IsGodModeEnabled = inGodMode;

            player.Hurt(10, DamageType.PocketDimension);
            player.EnableEffect(EffectType.PocketCorroding);

            _players.Remove(player);
            _enterTime.Remove(player);
        }
    }
}
