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

        protected override bool ProcessCheckpoint(Door door, Player player) => player.CheckPermissions(KeycardPermissions.Checkpoints);

        protected override bool ProcessGate(Gate gate, Player player) => player.CheckPermissions(gate.RequiredPermissions.RequiredPermissions);

        protected override bool ProcessKeycard(Door door, Player player) => !door.IsLocked && player.CheckPermissions(door.RequiredPermissions.RequiredPermissions);

        protected override bool ProcessNotKeycard(Door door, Player player) => !door.IsLocked && !player.IsCuffed && !player.HasEffect<SeveredHands>();
    }
}
