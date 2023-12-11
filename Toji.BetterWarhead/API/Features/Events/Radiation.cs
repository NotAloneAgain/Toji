using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Toji.BetterWarhead.API.Features.Events
{
    public class Radiation : BaseEvent
    {
        private Bounds _chaosBounds;
        private Bounds _ntfBounds;

        public Radiation()
        {
            _chaosBounds = (980f, 1008f).CalculateBounds(new Vector2(-10.172f, 2.879f), new Vector2(-10.172f, -15.871f), new Vector2(10.891f, 2.844f), new Vector2(11.137f, -16.223f));
            _ntfBounds = (980f, 1008f).CalculateBounds(new Vector2(65.292f, -32.957f), new Vector2(60.787f, -32.957f), new Vector2(60.207f, -37.652f), new Vector2(65.740f, -37.707f));
        }

        public override int Chance => 35;

        public override string Text => "Произошел выброс радиации из взорванного комплекса. Немедленно укрыться!";

        private protected override void Activate() => Timing.RunCoroutine(_CheckPlayers());

        private IEnumerator<float> _CheckPlayers()
        {
            DateTime startTime = DateTime.Now;

            while ((DateTime.Now - startTime).TotalSeconds < 60 && Round.InProgress)
            {
                foreach (var player in Player.List)
                {
                    if (player == null || player.IsDead || !InRadiationZone(player.Position))
                    {
                        continue;
                    }

                    Timing.RunCoroutine(_RadiationDamage(player));
                }

                yield return Timing.WaitForSeconds(1.25f);
            }
        }

        private IEnumerator<float> _RadiationDamage(Player player)
        {
            while (player is not null and { IsAlive: true } && InRadiationZone(player.Position))
            {
                player.Hurt(player.IsScp ? 32 : 8, "Радиоцинное облучение");

                yield return Timing.WaitForSeconds(1);
            }
        }

        private bool InRadiationZone(Vector3 position) => _chaosBounds.Contains(position) || _ntfBounds.Contains(position);
    }
}
