using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Passive;

namespace Toji.Classes.Subclasses.Scp0492.Group
{
    public class Poisoned : ZombieGroupSubclass, IHintSubclass, ILimitableGroup, IRandomSubclass
    {
        public override string Name => "Ядовитый";

        public override string Desc => "Крайне опасный зомби, вызывающий яд и разложение органических тканей у жертв";

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            new PoisonedAttackAbility(),
        };

        public int Chance => 15;

        public int Max => 4;
    }
}
