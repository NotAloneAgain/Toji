using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Mirror;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using Toji.Classes.API.Extensions;
using Toji.Classes.API.Features;
using Toji.Classes.Subclasses.Abilities.Active;
using Toji.Cleanups.API;
using Toji.Cleanups.API.Enums;
using Toji.Cleanups.API.Features;
using Toji.Global;

namespace Toji.Cleanups.List
{
    public class MidRagdollsCleanup : RagdollCleanup
    {
        public override CleanupType Type => CleanupType.Ragdolls;

        public override GameStage Stage => GameStage.Mid;

        public override void Cleanup(List<Ragdoll> ragdolls, List<Player> players, out float cooldown)
        {
            cooldown = 84;

            var subclasses = players.Select(ply => ply.GetSubclass());
            var hasMimicry = subclasses.Any(sub => sub != null && sub.Abilities.Any(ability => ability.GetType() == typeof(ClothesAbility)));

            var minTime = hasMimicry ? 150 : 21;

            foreach (var ragdoll in ragdolls)
            {
                if (RoleExtensions.GetTeam(ragdoll.Role) == Team.SCPs)
                {
                    continue;
                }

                var seconds = (Round.StartedTime - ragdoll.CreationTime).TotalSeconds;

                if (seconds <= minTime)
                {
                    continue;
                }

                var room = ragdoll.Room;

                if (room != null)
                {
                    var position = ragdoll.Position;
                    var isSurface = room?.Type == RoomType.Surface;

                    if (isSurface)
                    {
                        if (players.Any(ply => ply.Position.GetDistance(position) <= 14))
                        {
                            continue;
                        }
                    }
                    else if (players.Any(ply => ply.CurrentRoom == room))
                    {
                        continue;
                    }
                }

                try
                {
                    ragdoll.Destroy();
                }
                catch { }
            }
        }
    }
}
