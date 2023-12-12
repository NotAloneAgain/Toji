using Exiled.Events.EventArgs.Player;
using Toji.Cleanups.API;

namespace Toji.Cleanups.Handlers
{
    internal sealed class PlayerHandlers
    {
        public void OnSpawningRagdoll(SpawningRagdollEventArgs ev)
        {
            if (!ev.Player.IsInPocketDimension && ev.Player.GameObject.IsValid())
            {
                return;
            }

            ev.IsAllowed = false;
        }
    }
}
