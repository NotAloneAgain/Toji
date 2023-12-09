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
        public override bool ShowInfo => true;

        public override string Name => "Ядовитый";

        public override string Desc => "Очень крепкий и сильный зомби, способный выдержать множество атак";

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(4)
        {
            new HealthCharacteristic(1500),
            new SizeCharacteristic(Vector3.one * 1.11f),
            new EffectsCharacteristic(EffectType.SinkHole),
            new HurtMultiplayerCharacteristic(0.95f)
        };

        public int Chance => 18;

        public int Max => 3;
    }
}
