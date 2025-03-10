﻿using Exiled.Events.EventArgs.Player;
using Exiled.Events.Handlers;
using Toji.Classes.API.Features.Abilities;
using Toji.ExiledAPI.Extensions;

namespace Toji.Classes.Subclasses.Abilities.Passive
{
    public class SurviveAbility(int chance) : ChanceAbility(chance)
    {
        public override string Name => "Выживание";

        public override string Desc => $"Ты можешь пережить смертельную атаку с шансом {Chance}%";

        public override void Subscribe() => Player.Dying += OnDying;

        public override void Unsubscribe() => Player.Dying -= OnDying;

        private void OnDying(DyingEventArgs ev)
        {
            if (!ev.IsFullyValid() || !IsEnabled || !ev.IsAllowed || !Has(ev.Player) || !ev.DamageHandler.Type.IsValid() || !GetRandom())
            {
                return;
            }

            ev.IsAllowed = false;
        }
    }
}
