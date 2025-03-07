﻿using System.Collections.Generic;
using Toji.Classes.API.Enums;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Relations;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Active;
using Toji.Classes.Subclasses.Abilities.Passive;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.ClassD.Single
{
    public class Scp073 : DSingleSubclass, IHintSubclass, ICustomHintSubclass, IRandomSubclass
    {
        public override string Name => "SCP-073";

        public override string Desc => "Имеешь множество аномальных свойств, из-за чего тебя предпочитают не трогать";

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(2)
        {
            new KnockAbility(90, 5.06f),
            new ReflectionAbility(true, 0.296f)
        };

        public override List<BaseRelation> Relations { get; } = new List<BaseRelation>(1)
        {
            new SubclassRelation(RelationType.Not, GetInstance(typeof(Scp181)))
        };

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(2)
        {
            new HurtMultiplayerCharacteristic(0.64f),
            new ArtificalShieldCharacteristic(30, 50, -0.63f, 1, 5, true)
        };

        public string HintText => string.Empty;

        public string HintColor => "#009A63";

        public float HintDuration => 15;

        public int Chance => 7;
    }
}
