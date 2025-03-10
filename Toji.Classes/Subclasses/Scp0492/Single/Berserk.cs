﻿using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Passive;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.Scp0492.Single
{
    public class Berserk : ZombieSingleSubclass, IHintSubclass, IRandomSubclass
    {
        public override string Name => "Берсерк";

        public override string Desc => "Яростный зомби, он скорее погибнет, чем отпустит добычу";

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            new ScalingAbility(350, 20, 0.25f, 2),
        };

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(3)
        {
            new HurtMultiplayerCharacteristic(0.88f),
        };

        public int Chance => 16;
    }
}
