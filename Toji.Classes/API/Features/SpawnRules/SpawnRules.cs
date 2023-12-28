using PlayerRoles;

namespace Toji.Classes.API.Features.SpawnRules
{
    public abstract class SpawnRules<TValue> : BaseSpawnRules
    {
        public SpawnRules(TValue value) : base(RoleTypeId.None) => Value = value;

        public SpawnRules(TValue value, RoleTypeId model) : base(model) => Value = value;

        public TValue Value { get; }

        public abstract bool Check(TValue value);
    }
}
