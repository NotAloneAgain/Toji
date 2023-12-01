using Exiled.API.Features;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Toji.Classes.API.Features.Abilities
{
    public abstract class CooldownAbility : ActiveAbility
    {
        private List<AbilityUse> _lastUsed;

        public CooldownAbility(uint cooldown) => Cooldown = cooldown;

        public uint Cooldown { get; init; }

        public override bool Activate(Player player, out object result)
        {
            if (!base.Activate(player, out result))
                return false;

            var use = _lastUsed.FindLast(used => used.IsSuccessful && used.Player.UserId == player.UserId);
            var now = DateTime.Now;
            var different = now - use.Time;

            if (use != null && different.TotalSeconds < Cooldown)
            {
                result = Mathf.RoundToInt(Cooldown - (float)different.TotalSeconds);

                return false;
            }

            _lastUsed.Add(new(player, now, true, result));

            return true;
        }
    }
}
