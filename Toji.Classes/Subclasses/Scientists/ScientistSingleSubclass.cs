using PlayerRoles;
using Toji.Classes.API.Features;

namespace Toji.Classes.Subclasses.ClassD
{
    public abstract class ScientistSingleSubclass : SingleSubclass
    {
        public sealed override RoleTypeId Role => RoleTypeId.Scientist;
    }
}
