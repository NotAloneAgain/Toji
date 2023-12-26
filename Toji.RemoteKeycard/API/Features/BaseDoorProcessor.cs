using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using System.Collections.Generic;
using System.Linq;
using Toji.ExiledAPI.Extensions;
using Toji.RemoteKeycard.API.Enums;

namespace Toji.RemoteKeycard.API.Features
{
    public abstract class BaseDoorProcessor
    {
        private static List<BaseDoorProcessor> _processors;

        static BaseDoorProcessor()
        {
            _processors = new List<BaseDoorProcessor>(10);
        }

        public BaseDoorProcessor()
        {
            _processors.Add(this);
        }

        public static IReadOnlyCollection<BaseDoorProcessor> ReadOnlyCollection => _processors.AsReadOnly();

        public static BaseDoorProcessor Get(DoorProcessorType type) => ReadOnlyCollection.FirstOrDefault(processor => processor.Type == type);

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

        protected abstract bool ProcessGate(Gate gate, Player player);

        protected abstract bool ProcessCheckpoint(Door door, Player player);

        protected abstract bool ProcessNotKeycard(Door door, Player player);

        protected abstract bool ProcessKeycard(Door door, Player player);
    }
}
