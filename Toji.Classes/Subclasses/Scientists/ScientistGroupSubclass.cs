using PlayerRoles;
using Toji.Classes.API.Features;

namespace Toji.Classes.Subclasses.Scientists
{
    public abstract class ScientistGroupSubclass : GroupSubclass
    {
        public sealed override RoleTypeId Role => RoleTypeId.Scientist;
    }
}
