using PlayerRoles;
using Toji.Classes.API.Features;

namespace Toji.Classes.Subclasses.Guards
{
    public abstract class GuardSingleSubclass : SingleSubclass
    {
        public sealed override RoleTypeId Role => RoleTypeId.FacilityGuard;
    }
}
