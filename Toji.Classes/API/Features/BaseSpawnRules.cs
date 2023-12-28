using Exiled.API.Features;
using PlayerRoles;

namespace Toji.Classes.API.Features
{
    public abstract class BaseSpawnRules
    {
        public BaseSpawnRules(RoleTypeId model) => Model = model;

        public RoleTypeId Model { get; }

        public abstract bool Check(Player player);
    }
}
