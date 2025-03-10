﻿using PlayerRoles;
using System.Collections.Generic;
using Toji.Classes.API.Enums;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.SpawnRules;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Active;

namespace Toji.Classes.Subclasses.Ntf.Group
{
    public class DefaultPrivateSubclass : NtfGroupSubclass, IPrioritySubclass
    {
        public override bool ShowInfo => false;

        public override string Name => "Рядовой";

        public override string Desc => "Стандартный рядовой девятихвостой лисы";

        public sealed override BaseSpawnRules SpawnRules { get; } = new RoleSpawnRules(RoleTypeId.NtfPrivate);

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            new KnockAbility(90, 3.8f)
        };

        public LoadPriority Priority => LoadPriority.Lowest;
    }
}
