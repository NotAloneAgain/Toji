using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using MapGeneration.Distributors;
using System.Collections.Generic;
using System.Linq;
using Toji.ExiledAPI.Extensions;
using Toji.Global;
using Toji.RemoteKeycard.API.Enums;

namespace Toji.RemoteKeycard.API.Features
{
    public abstract class BasePermissionProcessor
    {
        private static List<BasePermissionProcessor> _processors;

        static BasePermissionProcessor()
        {
            _processors = new List<BasePermissionProcessor>(5);
        }

        public BasePermissionProcessor()
        {
            _processors.Add(this);
        }

        public static IReadOnlyCollection<BasePermissionProcessor> ReadOnlyCollection => _processors.AsReadOnly();

        public static BasePermissionProcessor Get(DoorProcessorType type) => ReadOnlyCollection.FirstOrDefault(processor => processor.Type == type);

        public abstract DoorProcessorType Type { get; }

        public bool ProcessDoor(Door door, Player player)
        {
            if (door == null || door.IsLocked && !door.AllowsScp106 || player.IsCuffed || player.HasEffect<SeveredHands>())
            {
                return false;
            }

            if (door is Gate gate)
            {
                return ProcessGate(gate, player);
            }

            if (door.IsPartOfCheckpoint || door.Type is DoorType.CheckpointArmoryA or DoorType.CheckpointArmoryB or DoorType.CheckpointEzHczA or DoorType.CheckpointEzHczB or DoorType.CheckpointLczA or DoorType.CheckpointLczB)
            {
                return ProcessCheckpoint(door, player);
            }

            return door.IsKeycardDoor ? ProcessKeycard(door, player) : ProcessNotKeycard(door, player);
        }

        public bool ProcessLocker(Locker locker, LockerChamber chamber, Player player)
        {
            bool hasAccess = player.CheckPermissions(locker.GetPermissions(chamber));

            return hasAccess || locker.StructureType switch
            {
                StructureType.ScpPedestal => player.CheckPermissions(KeycardPermissions.Checkpoints) && player.CheckPermissions(KeycardPermissions.ContainmentLevelTwo),
                StructureType.StandardLocker => !chamber.HasDanger(true) && player.CheckPermissions(KeycardPermissions.ArmoryLevelOne),
                StructureType.LargeGunLocker => !chamber.HasDanger(true) && player.CheckPermissions(KeycardPermissions.ArmoryLevelOne),
                StructureType.SmallWallCabinet => !chamber.HasDanger(true) && player.CheckPermissions(KeycardPermissions.ArmoryLevelOne),
                _ => false,
            };
        }

        protected abstract bool ProcessGate(Gate gate, Player player);

        protected abstract bool ProcessCheckpoint(Door door, Player player);

        protected abstract bool ProcessNotKeycard(Door door, Player player);

        protected abstract bool ProcessKeycard(Door door, Player player);
    }
}
