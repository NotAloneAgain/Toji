using CustomPlayerEffects;
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
            if (door == null || door.IsLocked && !door.AllowsScp106)
            {
                return false;
            }

            bool keycard = !player.IsCuffed && !player.HasEffect<SeveredHands>();

            var basic = door as BasicDoor;

            if (door.IsKeycardDoor)
            {
                keycard = keycard && ProcessKeycard(basic, player);
            }
            else
            {
                keycard = keycard && ProcessNotKeycard(basic, player);
            }

            if (door is Gate gate)
            {
                return ProcessGate(gate, player) && keycard;
            }

            if (door is CheckpointDoor checkpoint)
            {
                return ProcessCheckpoint(checkpoint, player) && keycard;
            }

            return keycard;
        }

        protected abstract bool ProcessGate(Gate gate, Player player);

        protected abstract bool ProcessCheckpoint(CheckpointDoor door, Player player);

        protected abstract bool ProcessNotKeycard(BasicDoor door, Player player);

        protected abstract bool ProcessKeycard(BasicDoor door, Player player);
    }
}
