using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using Toji.Cleanups.API;
using Toji.Cleanups.API.Enums;
using Toji.Cleanups.API.Features;
using Toji.Global;

namespace Toji.Cleanups.List
{
    public class LateRagdollsCleanup : RagdollCleanup
    {
        public override CleanupType Type => CleanupType.Ragdolls;

        public override GameStage Stage => GameStage.Late;

        public override void Cleanup(List<Ragdoll> ragdolls, List<Player> players, out float cooldown)
        {
            cooldown = 70;

            foreach (var ragdoll in ragdolls)
            {
                if (RoleExtensions.GetTeam(ragdoll.Role) == Team.SCPs || ragdoll.ExistenceTime < 16)
                {
                    continue;
                }

                var room = ragdoll.Room;
                var position = ragdoll.Position;

                if (players.Any(ply => ply.CurrentRoom == room && (room.Type != RoomType.Surface || ply.Position.GetDistance(position) > 11)) && ragdoll.GameObject.IsValid())
                {
                    continue;
                }

                ragdoll.Destroy();
            }
        }
    }
}
