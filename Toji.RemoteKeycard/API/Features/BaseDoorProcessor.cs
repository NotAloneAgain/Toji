using Exiled.API.Features;
using Exiled.API.Features.Doors;
using System.Collections.Generic;
using System.Linq;
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

        public static IReadOnlyCollection<BaseDoorProcessor> ReadOnlyCollection => _processors.AsReadOnly();

        public static BaseDoorProcessor Get(DoorProcessorType type) => ReadOnlyCollection.FirstOrDefault(processor => processor.Type == type);

        public abstract DoorProcessorType Type { get; }

        public bool ProcessDoor(Door door, Player player)
        {
            if (door == null || door.IsLocked && !door.AllowsScp106)
            {
                return false;
            }

            if (door is Gate gate)
            {
                return ProcessGate(gate, player);
            }

            if (door is CheckpointDoor checkpoint)
            {
                return ProcessCheckpoint(checkpoint, player);
            }

            var basic = door as BasicDoor;

            if (door.IsKeycardDoor)
            {
                return ProcessKeycard(basic, player);
            }
            else
            {
                return ProcessNotKeycard(basic, player);
            }
        }

        protected abstract bool ProcessGate(Gate gate, Player player);

        protected abstract bool ProcessCheckpoint(CheckpointDoor door, Player player);

        protected abstract bool ProcessNotKeycard(BasicDoor door, Player player);

        protected abstract bool ProcessKeycard(BasicDoor door, Player player);
    }
}
