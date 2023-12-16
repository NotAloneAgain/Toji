using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using System.Collections.Generic;
using Toji.Classes.API.Features.Abilities;
using Toji.Classes.API.Interfaces;
using Toji.ExiledAPI.Extensions;
using UnityEngine;

namespace Toji.Classes.Subclasses.Abilities.Passive
{
    public class ScalingAbility : PassiveAbility, IDamageController
    {
        private Dictionary<Player, ScalingStats> _stats;

        public ScalingAbility()
        {
            _stats = new(Server.MaxPlayerCount);
        }

        public override string Name => "Ярость";

        public override string Desc => "С каждым убитым ярость и желание крови укрепляется";

        public override void OnEnabled(Player player)
        {
            base.OnEnabled(player);

            _stats.Add(player, new ScalingStats());

            player.Health = 350;
            player.MaxHealth = 350;

            Timing.RunCoroutine(_UpdateValues(player));
        }

        public override void OnDisabled(Player player)
        {
            base.OnDisabled(player);

            _stats.Remove(player);
        }

        public override void Subscribe() => Exiled.Events.Handlers.Player.Died += OnDied;

        public override void Unsubscribe() => Exiled.Events.Handlers.Player.Died -= OnDied;

        public void OnDied(DiedEventArgs ev)
        {
            if (!ev.IsNotSelfDamage() || !Has(ev.Attacker))
            {
                return;
            }

            var stats = _stats[ev.Attacker];

            stats.Damage = Mathf.Clamp(stats.Damage + 10, 20, 150);
            stats.Health = Mathf.Clamp(stats.Health + 50, 350, 1400);
            stats.Vampire = Mathf.Clamp(stats.Vampire + 0.025f, 0.25f, 0.425f);
            stats.Movement = (byte)Mathf.Clamp(stats.Movement + 2, 2, 40);
        }

        public void OnDamage(HurtingEventArgs ev)
        {
            if (!Has(ev.Player) || !_stats.TryGetValue(ev.Attacker, out var stats))
            {
                return;
            }

            ev.Amount = stats.Damage;
            ev.Attacker.Heal(ev.Amount * stats.Vampire);
        }

        private IEnumerator<float> _UpdateValues(Player player)
        {
            var boost = player.GetEffect(EffectType.MovementBoost) as MovementBoost;

            while ((player?.IsAlive ?? false) && Has(player))
            {
                var stats = _stats[player];

                if (player.MaxHealth < stats.Health)
                {
                    var value = player.MaxHealth - stats.Health;

                    player.MaxHealth = stats.Health;
                    player.Heal(value);
                }

                boost.ServerSetState(stats.Movement, 0, false);

                yield return Timing.WaitForSeconds(1);
            }
        }

        struct ScalingStats
        {
            public ScalingStats()
            {
                Health = 350;
                Damage = 20;
                Vampire = 0.25f;
                Movement = 2;
            }

            public int Health { get; set; }

            public int Damage { get; set; }

            public float Vampire { get; set; }

            public byte Movement { get; set; }
        }
    }
}
