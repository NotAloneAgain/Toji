using System;
using System.Collections.Generic;
using Toji.Classes.API.Enums;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Relations;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.ClassD.Single
{
    public class Letting : DSingleSubclass, IHintSubclass, IRandomSubclass
    {
        public override string Name => "Попущенный";

        public override string Desc => "Попущенный блатным, вынужден драить его камеру и туалеты";

        public override List<BaseRelation> Relations { get; } = new List<BaseRelation>(1)
        {
            new SubclassRelation(RelationType.Required, GetInstance(typeof(Gang)))
        };

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(1)
        {
            new HealthCharacteristic(90)
        };

        public int Chance => 35;
    }
}
