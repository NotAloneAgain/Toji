using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Passive;
using Toji.ExiledAPI.Extensions;

namespace Toji.Classes.Subclasses.Scientists.Single
{
    public class Runner : ScientistSingleSubclass, IHintSubclass, IRandomSubclass
    {
        public override string Name => "Неутомимый";

        public override string Desc => "В результате неудачного биологического эксперимента получил сверх-выносливость и иммунитет к замедлению";

        public override List<string> Tags { get; } = new List<string>(1) { "Shadow" };

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(2)
        {
            new InfinityStaminaAbility(),
            new ImmunityEffectsAbility(EffectType.SinkHole, EffectType.Stained)
        };

        public int Chance => 15;
    }
}
