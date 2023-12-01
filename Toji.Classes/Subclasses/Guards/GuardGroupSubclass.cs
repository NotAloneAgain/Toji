using PlayerRoles;
using Toji.Classes.API.Features;

namespace Toji.Classes.Subclasses.Guards
{
    public abstract class GuardGroupSubclass : GroupSubclass
    {
        public sealed override RoleTypeId Role => RoleTypeId.FacilityGuard;
    }
}
