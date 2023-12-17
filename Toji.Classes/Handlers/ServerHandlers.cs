using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toji.Classes.API.Extensions;

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
        }
    }
}
