using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using PlayerRoles;
using Toji.RemoteKeycard.API;
using Toji.RemoteKeycard.API.Enums;
using Toji.RemoteKeycard.API.Features;

namespace Toji.RemoteKeycard.Processors
{
    public class ScpDoorProcessor : BasePermissionProcessor
    {
        public override DoorProcessorType Type => DoorProcessorType.Scp;

        protected override bool ProcessCheckpoint(Door door, Player player)
        {
            if (door.IsLocked)
            {
                return door.DoorLockType switch
                {
                    DoorLockType.Regular079 or DoorLockType.Lockdown079 => true,
                    _ => false,
                };
            }

            return true;
        }

        protected override bool ProcessGate(Gate gate, Player player)
        {
            if (gate.IsLocked)
            {
                return gate.DoorLockType switch
                {
                    DoorLockType.Regular079 or DoorLockType.Lockdown079 => true,
                    _ => false,
                };
            }

            return player.Role.Type == RoleTypeId.Scp079 || gate.RequiredPermissions == null || player.CheckPermissions(gate.KeycardPermissions);
        }

        protected override bool ProcessKeycard(Door door, Player player)
        {
            if (door.IsLocked)
            {
                return door.DoorLockType switch
                {
                    DoorLockType.Regular079 or DoorLockType.Lockdown079 => true,
                    _ => false,
                };
            }

            return player.Role.Type == RoleTypeId.Scp079 || player.CheckPermissions(door.KeycardPermissions);
        }

        protected override bool ProcessNotKeycard(Door door, Player player)
        {
            if (door.IsLocked)
            {
                return door.DoorLockType switch
                {
                    DoorLockType.Regular079 or DoorLockType.Lockdown079 => true,
                    _ => false,
                };
            }

            return true;
        }
    }
}
