using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.ClassD.Single
{
    public class TwoFaced : DSingleSubclass, IHintSubclass, ICustomHintSubclass, IRandomSubclass
    {
        public override string Name => "Двуликий";

        public override string Desc => "Ты можешь копировать поведение и внешнее строение мертвых существ.";

        public override List<string> Tags { get; } = new List<string>(2) { "Halloween", "Voting2023Winner" };

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(1)
        {
            new RespawnCharacteristic(true)
        };

        public string HintText => string.Empty;

        public string HintColor => "#480607";

        public float HintDuration => 15;

        public int Chance => 9;
    }
}
