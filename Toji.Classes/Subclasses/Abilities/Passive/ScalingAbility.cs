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
    public class ScalingAbility(int health, int damage, float vampire, byte movement) : PassiveAbility, IDamageController
    {
        public override string Name => "Усиление";

        public override string Desc => "Каждая новая смерть приносит вам усиление";

        public int Health { get; set; }

        public int Damage { get; set; }

        public float Vampire { get; set; }

        public byte Movement { get; set; }

        public override void OnEnabled(Player player)
        {
            base.OnEnabled(player);

            Health = health;
            Damage = damage;
            Vampire = vampire;
            Movement = movement;

            player.Health = Health;
            player.MaxHealth = Health;

            Timing.RunCoroutine(_UpdateValues(player));
        }

        public override void Subscribe() => Exiled.Events.Handlers.Player.Died += OnDied;

        public override void Unsubscribe() => Exiled.Events.Handlers.Player.Died -= OnDied;

        public void OnDied(DiedEventArgs ev)
        {
            if (!ev.IsNotSelfDamage() || !IsEnabled || !Has(ev.Attacker))
            {
                return;
            }

            Damage = Mathf.Clamp(Damage + 10, 20, 120);
            Health = Mathf.Clamp(Health + 50, 350, 1600);
            Vampire = Mathf.Clamp(Vampire + 0.025f, 0.25f, 0.45f);
            Movement = (byte)Mathf.Clamp(Movement + 2, 2, 50);
        }

        public void OnDamage(HurtingEventArgs ev)
        {
            if (!Has(ev.Player) || !ev.IsAllowed || !IsEnabled)
            {
                return;
            }

            ev.Amount = Damage;
            ev.Attacker.Heal(ev.Amount * Vampire);
        }

        private IEnumerator<float> _UpdateValues(Player player)
        {
            while ((player?.IsAlive ?? false) && Has(player))
            {
                if (player.MaxHealth < Health)
                {
                    var value = player.MaxHealth - Health;

                    player.MaxHealth = Health;
                    player.Heal(value);
                }

                player.EnableEffect(EffectType.MovementBoost, Movement, 0, false);

                yield return Timing.WaitForSeconds(1);
            }
        }
    }
}
