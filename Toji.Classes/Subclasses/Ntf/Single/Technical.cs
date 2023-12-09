using PlayerRoles;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Inventory;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Active;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.Ntf.Single
{
    public class Technical : NtfSingleSubclass, IHintSubclass, IRandomSubclass, INeedRole
    {
        public override bool ShowInfo => true;

        public override string Name => "Технический специалист";

        public override RoleTypeId Role => RoleTypeId.NtfSergeant;

        public override string Desc => "Белый хакер, программист и просто человек увлекающийся IT";

        public override List<string> Tags { get; } = new List<string>(1) { "Shadow" };

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            new HackAbility(190)
        };

        public RoleTypeId NeedRole => RoleTypeId.Scp079;

        public int Chance => 12;
    }
}
