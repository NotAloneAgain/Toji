using Exiled.API.Features;
using Toji.Classes.API.Extensions;
using Toji.Classes.API.Features;

namespace Toji.Classes.Handlers
{
    internal sealed class ServerHandlers
    {
        public void OnRoundRestarting()
        {
            foreach (var player in Player.List)
            {
                if (player == null || player.IsHost || player.IsNPC || !player.IsConnected || !player.TryGetSubclass(out var subclass))
                {
                    continue;
                }

                subclass.Revoke(player);
            }

            BaseSubclass.Clear();
        }
    }
}
