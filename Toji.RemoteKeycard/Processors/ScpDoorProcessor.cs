using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toji.RemoteKeycard.API.Enums;
using Toji.RemoteKeycard.API.Features;

namespace Toji.RemoteKeycard.Processors
{
    public class ScpDoorProcessor : BaseDoorProcessor
    {
        public override DoorProcessorType Type => DoorProcessorType.Scp;

        protected override bool ProcessCheckpoint(CheckpointDoor door, Player player)
        {
            if (player.Role.Type == RoleTypeId.Scp079)
            {
                return true;
            }

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
            if (player.Role.Type == RoleTypeId.Scp079)
            {
                return true;
            }

            if (gate.IsLocked)
            {
                return gate.DoorLockType switch
                {
                    DoorLockType.Regular079 or DoorLockType.Lockdown079 => true,
                    _ => false,
                };
            }

            return false;
        }

        protected override bool ProcessKeycard(BasicDoor door, Player player)
        {
            if (player.Role.Type == RoleTypeId.Scp079)
            {
                return true;
            }

            if (door.IsLocked)
            {
                return door.DoorLockType switch
                {
                    DoorLockType.Regular079 or DoorLockType.Lockdown079 => true,
                    _ => false,
                };
            }

            return false;
        }

        protected override bool ProcessNotKeycard(BasicDoor door, Player player) => true;
    }
}
