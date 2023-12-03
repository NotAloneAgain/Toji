using System;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.ClassD.Single
{
    public class Letting : DSingleSubclass, IHintSubclass, IRandomSubclass, INeedSubclass
    {
        public override string Name => "Попущенный";

        public override string Desc => "Попущенный блатным, вынужден драить его камеру и туалеты";

        public int Chance => 33;

        public Type Needed => typeof(Gang);

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>()
        {
            new HealthCharacteristic(90)
        };
    }
}
