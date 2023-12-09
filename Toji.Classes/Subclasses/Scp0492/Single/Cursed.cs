using Exiled.API.Enums;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Active;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.Scp0492.Single
{
    public class Cursed : ZombieSingleSubclass, IHintSubclass, IRandomSubclass
    {
        public override string Name => "Проклятый";

        public override string Desc => "Все его жертвы получают проклятье";

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            new CurseAbility(),
        };

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(3)
        {
            new EffectsCharacteristic(5, 0, EffectType.MovementBoost),
        };

        public int Chance => 16;
    }
}
