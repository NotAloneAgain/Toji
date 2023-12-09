using CustomPlayerEffects;
using Exiled.Events.EventArgs.Player;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Passive;
using Toji.ExiledAPI.Extensions;

namespace Toji.Classes.Subclasses.Scientists.Single
{
    public class Runner : ScientistSingleSubclass, IHintSubclass, IRandomSubclass, ISubscribable
    {
        public override string Name => "Неутомимый";

        public override string Desc => "В результате неудачного биологического эксперимента получил сверх-выносливость и иммунитет к замедлению";

        public override List<string> Tags { get; } = new List<string>(1) { "Shadow" };

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            new InfinityStaminaAbility()
        };

        public int Chance => 15;

        public void Subscribe() => Exiled.Events.Handlers.Player.ReceivingEffect += OnReceivingEffect;

        public void Unsubscribe() => Exiled.Events.Handlers.Player.ReceivingEffect -= OnReceivingEffect;

        private void OnReceivingEffect(ReceivingEffectEventArgs ev)
        {
            if (!ev.IsAllowed || !ev.IsValid() || !Has(ev.Player))
            {
                return;
            }

            ev.IsAllowed = ev.Effect is not Sinkhole;
        }
    }
}
