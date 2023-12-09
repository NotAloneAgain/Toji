﻿using Exiled.API.Enums;
using PlayerRoles;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.Subclasses.Abilities.Active;

namespace Toji.Classes.Subclasses.Chaos.Group
{
    public class DefaultMarauderSubclass : ChaosGroupSubclass
    {
        public override string Name => "Мародёр";

        public override RoleTypeId Role => RoleTypeId.ChaosMarauder;

        public override string Desc => "Стандартный мародёр повстанцев хаоса";

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            new KnockAbility(110, 5, new HashSet<DoorType>()
            {
                DoorType.LczArmory,
                DoorType.HczArmory,
                DoorType.HID,
            })
        };
    }
}
