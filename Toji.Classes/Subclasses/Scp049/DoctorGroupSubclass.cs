using PlayerRoles;
using Toji.Classes.API.Features;

namespace Toji.Classes.Subclasses.Scp049
{
    public abstract class DoctorGroupSubclass : GroupSubclass
    {
        public sealed override RoleTypeId Role => RoleTypeId.Scp049;
    }
}
