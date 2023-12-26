using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Toji.RemoteKeycard.API;
using Toji.RemoteKeycard.API.Enums;
using Toji.RemoteKeycard.API.Features;

namespace Toji.RemoteKeycard.Processors
{
    public class HumanDoorProcessor : BaseDoorProcessor
    {
        public override DoorProcessorType Type => DoorProcessorType.Human;

        protected override bool ProcessCheckpoint(Door door, Player player) => !door.IsLocked && player.CheckPermissions(KeycardPermissions.Checkpoints);

        protected override bool ProcessGate(Gate gate, Player player) => !gate.IsLocked && player.CheckPermissions(gate.KeycardPermissions);

        protected override bool ProcessKeycard(Door door, Player player) => !door.IsLocked && player.CheckPermissions(door.KeycardPermissions);

        protected override bool ProcessNotKeycard(Door door, Player player) => !door.IsLocked;
    }
}
