using Exiled.API.Features;
using System;
using System.Collections.Generic;
using Toji.Classes.API.Interfaces;
using UnityEngine;

namespace Toji.Classes.API.Features.Abilities
{
    public abstract class CooldownAbility(uint cooldown) : ActiveAbility, ISubscribable
    {
        private List<AbilityUse> _lastUsed = new(200);

        public uint Cooldown { get; internal set; } = cooldown;

        public override bool Activate(Player player, out object result)
        {
            if (!base.Activate(player, out result))
                return false;

            var use = _lastUsed.FindLast(used => used.IsSuccessful && used.Player.UserId == player.UserId);
            var now = DateTime.Now;
            var different = now - (use?.Time ?? DateTime.MinValue);

            if (use != null && different.TotalSeconds < Cooldown)
            {
                result = Mathf.RoundToInt(Cooldown - (float)different.TotalSeconds);

                return false;
            }

            return true;
        }

        public virtual void Subscribe() => Exiled.Events.Handlers.Server.RestartingRound += OnRestartingRound;

        public virtual void Unsubscribe() => Exiled.Events.Handlers.Server.RestartingRound -= OnRestartingRound;

        protected void AddUse(AbilityUse use) => _lastUsed.Add(use);

        protected void AddUse(Player player, DateTime time, bool success, object result) => AddUse(new(player, time, success, result));

        private void OnRestartingRound() => _lastUsed.Clear();
    }
}
