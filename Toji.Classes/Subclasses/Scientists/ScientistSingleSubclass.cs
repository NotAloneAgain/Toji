using PlayerRoles;
using Toji.Classes.API.Features;

namespace Toji.Classes.Subclasses.Scientists
{
    public abstract class ScientistSingleSubclass : SingleSubclass
    {
        public sealed override RoleTypeId Role => RoleTypeId.Scientist;
    }
}
