using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using System;

namespace Toji.Malfunctions.API.Features.Malfunctions
{
    public class DoorMalfunction : BaseDoorMalfunction<BreakableDoor>
    {
        public override string Name => "Поломка двери";

        public override int MinDuration => 20;

        public override int MaxDuration => 60;

        public override int Cooldown => 115;

        public override int Chance => 37;

        public override Func<BreakableDoor, bool> Condition => (BreakableDoor door) => base.Condition(door) && !door.IsGate;

        public override void Activate(int duration)
        {
            base.Activate(duration);

            if (Value == null)
            {
                return;
            }

            Map.Broadcast(12, $"<color=#780000><b>Внимание всем!\nПроизошла {Name.ToLower()} {TranslateZone(Value.Zone)}, исправление займет {GetSecondsString(duration)}</b></color>");

            Value.Lock(duration, DoorLockType.SpecialDoorFeature);
        }
    }
}
