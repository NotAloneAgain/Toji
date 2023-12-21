using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Interactables.Interobjects.DoorUtils;
using PlayerRoles;
using Toji.ExiledAPI.Extensions;
using Toji.RemoteKeycard.API;
using Toji.RemoteKeycard.API.Enums;
using Toji.RemoteKeycard.API.Features;

namespace Toji.RemoteKeycard.Processors
{
    public class HumanDoorProcessor : BaseDoorProcessor
    {
        public override DoorProcessorType Type => DoorProcessorType.Human;

        protected override bool ProcessCheckpoint(CheckpointDoor door, Player player) => !door.IsLocked && player.CheckPermissions(KeycardPermissions.Checkpoints);

        protected override bool ProcessGate(Gate gate, Player player) => !gate.IsLocked && (player.CheckPermissions(gate.RequiredPermissions.RequiredPermissions) || player.Role is { Team: Team.FoundationForces, Type: not RoleTypeId.FacilityGuard });

        protected override bool ProcessKeycard(BasicDoor door, Player player) => !door.IsLocked && player.CheckPermissions(door.RequiredPermissions.RequiredPermissions);

        protected override bool ProcessNotKeycard(BasicDoor door, Player player) => !door.IsLocked && !player.IsCuffed && !player.HasEffect<SeveredHands>();
    }
}
