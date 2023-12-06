using Exiled.API.Enums;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.Scp0492.Group
{
    public class Runner : ZombieGroupSubclass, IHintSubclass, ILimitableGroup, IRandomSubclass
    {
        public override string Name => "Бегун";

        public override string Desc => "Очень быстрый и ловкий зомби";

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(2)
        {
            new EffectsCharacteristic(15, 0, EffectType.MovementBoost),
            new AttackCooldownCharacteristic(0.9f),
        };

        public int Chance => 16;

        public int Max => 5;
    }
}
