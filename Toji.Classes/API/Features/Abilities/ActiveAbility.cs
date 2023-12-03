using Exiled.API.Features;

namespace Toji.Classes.API.Features.Abilities
{
    public abstract class ActiveAbility : BaseAbility
    {
        public virtual bool AllowConsole => true;

        public virtual bool Activate(Player player, out object result)
        {
            result = null!;

            return player != null && !player.IsHost && !player.IsNPC && player.IsAlive;
        }
    }
}
