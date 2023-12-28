using PlayerRoles;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.SpawnRules;
using Toji.Classes.API.Features.Subclasses;

namespace Toji.Classes.Subclasses.ClassD
{
    public abstract class DGroupSubclass : GroupSubclass
    {
        public sealed override BaseSpawnRules SpawnRules { get; } = new RoleSpawnRules(RoleTypeId.ClassD);
    }
}
