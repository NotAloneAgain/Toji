using Exiled.API.Enums;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Characteristics;
using UnityEngine;

namespace Toji.Classes.Subclasses.Scp0492.Group
{
    public class Tank : ZombieGroupSubclass, IHintSubclass, ILimitableGroup, IRandomSubclass
    {
        public override string Name => "Ядовитый";

        public override string Desc => "Крайне опасный зомби, вызывающий яд и разложение органических тканей у жертв";

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(3)
        {
            new HealthCharacteristic(1500),
            new SizeCharacteristic(Vector3.one * 1.11f),
            new EffectsCharacteristic(EffectType.SinkHole)
        };

        public int Chance => 15;

        public int Max => 3;
    }
}
