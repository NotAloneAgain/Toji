using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using Toji.Classes.API.Extensions;
using Toji.Classes.API.Features;
using Toji.Classes.Subclasses.Abilities.Active;
using Toji.Cleanups.API.Enums;
using Toji.Cleanups.API.Features;
using Toji.Global;

namespace Toji.Cleanups.List
{
    public class EarlyRagdollsCleanup : RagdollCleanup
    {
        public override CleanupType Type => CleanupType.Ragdolls;

        public override GameStage Stage => GameStage.Early;

        public override void Cleanup(List<Ragdoll> ragdolls, List<Player> players, out float cooldown)
        {
            cooldown = 120;

            var subclasses = players.Select(x => x.GetSubclass());
            var hasMimicry = subclasses.Any(sub => sub != null && sub.Abilities.Any(ability => ability.GetType() == typeof(ClothesAbility)));

            foreach (var ragdoll in ragdolls)
            {
                if (RoleExtensions.GetTeam(ragdoll.Role) == Team.SCPs || ragdoll.ExistenceTime < 31)
                {
                    continue;
                }

                var room = ragdoll.Room;
                var position = ragdoll.Position;

                if (players.Any(ply => ply.CurrentRoom == room && (room.Type != RoomType.Surface || ply.Position.GetDistance(position) <= 20)) || hasMimicry && BaseSubclass.TryGet(ragdoll, out _))
                {
                    continue;
                }

                ragdoll.Destroy();
            }
        }
    }
}
