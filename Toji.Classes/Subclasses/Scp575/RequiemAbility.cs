using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.DamageHandlers;
using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using Toji.Classes.API.Features.Abilities;
using Toji.ExiledAPI.Extensions;
using UnityEngine;

namespace Toji.Classes.Subclasses.Scp575
{
    public class RequiemAbility(uint cooldown) : CooldownAbility(cooldown)
    {
        private Dictionary<Player, int> _souls = new(Server.MaxPlayerCount);

        public override string Name => "Requiem";

        public override string Desc => "Высвобождаете души убитых вами людей, заставляя их атаковать живых";

        public override void OnEnabled(Player player)
        {
            base.OnEnabled(player);

            _souls.Add(player, 0);
        }

        public override void OnDisabled(Player player)
        {
            _souls.Remove(player);

            base.OnDisabled(player);
        }

        public override void Subscribe()
        {
            base.Subscribe();

            Exiled.Events.Handlers.Player.Died += OnDied;
        }

        public override void Unsubscribe()
        {
            Exiled.Events.Handlers.Player.Died -= OnDied;

            base.Unsubscribe();
        }

        public override bool Activate(Player player, out object result)
        {
            if (!base.Activate(player, out result))
            {
                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (player.CurrentRoom.Zone == ZoneType.Surface)
            {
                result = "Ты не можешь активировать способности на Поверхности!";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            var souls = _souls[player];

            if (souls == 0)
            {
                result = "У вас нет душ!";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            Map.TurnOffAllLights(1.25f * souls);

            var amount = 5 * souls;

            foreach (var ply in Player.List)
            {
                if (ply == null || ply.IsScp || ply.IsDead || ply.IsHost || ply.CurrentRoom?.Zone == ZoneType.Surface)
                {
                    continue;
                }

                ply.Hurt(amount);
            }

            _souls[player] = 0;

            AddUse(player, DateTime.Now, true, result);

            return true;
        }

        private void OnDied(DiedEventArgs ev)
        {
            if (!ev.IsNotSelfDamage(false) || !Owners.Any())
            {
                return;
            }

            foreach (var owner in Owners)
            {
                if (owner.Zone != ev.Attacker.Zone)
                {
                    return;
                }

                _souls[owner] = Mathf.Min(_souls[ev.Attacker] + 1, 18);
            }
        }
    }
}
