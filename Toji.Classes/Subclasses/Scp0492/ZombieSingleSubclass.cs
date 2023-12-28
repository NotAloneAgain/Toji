using PlayerRoles;
using Toji.Classes.API.Features.SpawnRules;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Subclasses;

namespace Toji.Classes.Subclasses.Scp0492
{
    public abstract class ZombieSingleSubclass : SingleSubclass
    {
        public sealed override BaseSpawnRules SpawnRules { get; } = new RoleSpawnRules(RoleTypeId.Scp0492);
    }
}
