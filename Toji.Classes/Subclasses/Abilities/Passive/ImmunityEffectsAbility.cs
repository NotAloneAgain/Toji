using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.Events.EventArgs.Player;
using System.Collections.Generic;
using System.Linq;
using Toji.Classes.API.Features.Abilities;
using Toji.ExiledAPI.Extensions;

namespace Toji.Classes.Subclasses.Abilities.Passive
{
    public class ImmunityEffectsAbility : PassiveAbility
    {
        private HashSet<EffectType> _blockedEffects;

        public ImmunityEffectsAbility(HashSet<EffectType> blockedEffects) => _blockedEffects = blockedEffects;

        public ImmunityEffectsAbility(params EffectType[] effects) : this(effects.ToHashSet()) { }

        public override string Name => "Иммунитет";

        public override string Desc => "Ты имеешь иммунитет к некоторым эффектам";

        public override void Subscribe() => Exiled.Events.Handlers.Player.ReceivingEffect += OnReceivingEffect;

        public override void Unsubscribe() => Exiled.Events.Handlers.Player.ReceivingEffect -= OnReceivingEffect;

        private void OnReceivingEffect(ReceivingEffectEventArgs ev)
        {
            if (!ev.IsAllowed || !ev.IsValid() || !Has(ev.Player))
            {
                return;
            }

            ev.IsAllowed = !_blockedEffects.Contains(ev.Effect.GetEffectType());
        }
    }
}
