using Exiled.API.Enums;
using PlayerRoles;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Active;
using Toji.Classes.Subclasses.Abilities.Passive;
using Toji.Classes.Subclasses.Characteristics;
using UnityEngine;

namespace Toji.Classes.Subclasses.Chaos.Single
{
    public class Juggernaut : ChaosSingleSubclass, IHintSubclass, IRandomSubclass
    {
        public override string Name => "Джаггернаут";

        public override RoleTypeId Role => RoleTypeId.ChaosRepressor;

        public override string Desc => "Усиленный результатами тренировок и экспериментов боец";

        public override List<string> Tags { get; } = new List<string>(1) { "Shadow" };

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(2)
        {
            new ImmunityEffectsAbility(EffectType.SinkHole, EffectType.Traumatized, EffectType.Bleeding, EffectType.Poisoned),
            new KnockAbility(100, 5, new HashSet<DoorType>()
            {
                DoorType.HID,
            })
        };

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(4)
        {
            new HealthCharacteristic(140),
            new SizeCharacteristic(Vector3.one * 1.12f),
            new ArtificalShieldCharacteristic(40, 75, -0.15f, 1, 8, true),
            new HurtMultiplayerCharacteristic(0.9f)
        };

        public int Chance => 10;
    }
}
