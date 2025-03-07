﻿using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using System.Collections.Generic;
using Toji.Classes.API.Features.Abilities;
using Toji.Classes.API.Interfaces;
using Toji.ExiledAPI.Extensions;

namespace Toji.Classes.Subclasses.Abilities.Active
{
    public class CurseAbility : ActiveAbility, ISubscribable, IDamageController
    {
        private Dictionary<string, HashSet<Player>> _cursed;

        public CurseAbility()
        {
            _cursed = new(Server.MaxPlayerCount);
        }

        public override string Name => "Проклятье";

        public override string Desc => "Атакуемые тобой и твой убийца получают проклятье, после твоей смерти оно будет забирать 1.44 единицы здоровья в секунду";

        public override bool AllowConsole => false;

        public override bool Activate(Player player, out object result)
        {
            if (!base.Activate(player, out result))
            {
                return false;
            }

            Timing.RunCoroutine(_CursedDamage(player));

            return true;
        }

        public void Subscribe() => Exiled.Events.Handlers.Player.Dying += OnDying;

        public void Unsubscribe() => Exiled.Events.Handlers.Player.Dying -= OnDying;

        public void OnDying(DyingEventArgs ev)
        {
            if (!ev.IsAllowed || !ev.IsNotSelfDamage() || !Has(ev.Player))
            {
                return;
            }

            if (!_cursed.TryGetValue(ev.Player.UserId, out var cursed))
            {
                _cursed.Add(ev.Player.UserId, new HashSet<Player>(Server.MaxPlayerCount) { ev.Attacker });
            }
            else
            {
                cursed.Add(ev.Attacker);
            }

            foreach (var player in _cursed[ev.Player.UserId])
            {
                Activate(player, out _);
            }

            _cursed.Remove(ev.Player.UserId);
        }

        public void OnDamage(HurtingEventArgs ev)
        {
            if (!ev.IsNotSelfDamage())
            {
                return;
            }

            if (!_cursed.TryGetValue(ev.Attacker.UserId, out var cursed))
            {
                _cursed.Add(ev.Attacker.UserId, new HashSet<Player>(Server.MaxPlayerCount) { ev.Player });
            }
            else
            {
                cursed.Add(ev.Player);
            }
        }

        private IEnumerator<float> _CursedDamage(Player player)
        {
            while (player?.IsAlive ?? false)
            {
                player.Hurt(1.44f);

                yield return Timing.WaitForSeconds(1);
            }
        }
    }
}
