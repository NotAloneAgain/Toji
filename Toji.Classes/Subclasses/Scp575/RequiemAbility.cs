using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;
using Toji.Classes.API.Features.Abilities;
using UnityEngine;

namespace Toji.Classes.Subclasses.Scp575
{
    public class RequiemAbility : CooldownAbility
    {
        private Dictionary<Player, int> _souls;

        public RequiemAbility(uint cooldown) : base(cooldown)
        {
            _souls = new Dictionary<Player, int>(Server.MaxPlayerCount);
        }

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

        public override bool Activate(Player player, out object result)
        {
            if (!base.Activate(player, out result))
            {
                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (Round.ElapsedTime.TotalMinutes < 2)
            {
                result = "Способность разблокируется через 2 минуты после начала раунда!";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (player.CurrentRoom.Zone == ZoneType.Surface)
            {
                result = "Ты не можешь активировать ярость на Поверхности!";

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
                ply.Hurt(amount, DamageType.Crushed);
            }

            _souls[player] = 0;

            AddUse(player, DateTime.Now, true, result);

            return true;
        }

        private void OnDied(DiedEventArgs ev)
        {
            if (!Has(ev.Attacker) || !_souls.ContainsKey(ev.Attacker))
            {
                return;
            }

            _souls[ev.Attacker] = Mathf.Min(_souls[ev.Attacker] + 1, 18);
        }
    }
}
