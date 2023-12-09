using PlayerRoles;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.Chaos.Single
{
    public class JuggernautSubclass : ChaosSingleSubclass, IHintSubclass, IRandomSubclass
    {
        public override bool ShowInfo => true;

        public override string Name => "Джаггернаут";

        public override RoleTypeId Role => RoleTypeId.ChaosRepressor;

        public override string Desc => "Усиленный результатами тренировок и экспериментов боец";

        public override List<string> Tags { get; } = new List<string>(1) { "Shadow" };

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(2)
        {
            new HealthCharacteristic(150),
            new ArtificalShieldCharacteristic(75, 75, 0, 1, 8, true)
        };

        public int Chance => 15;
    }
}
