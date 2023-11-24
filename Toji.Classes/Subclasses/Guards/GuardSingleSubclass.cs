using PlayerRoles;
using Toji.Classes.API.Features;

namespace Toji.Classes.Subclasses.ClassD
{
    public abstract class GuardSingleSubclass : SingleSubclass
    {
        public sealed override RoleTypeId Role => RoleTypeId.FacilityGuard;
    }
}
