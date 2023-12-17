using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;

namespace Toji.Malfunctions.API.Features.Malfunctions
{
    public class GateMalfunction : BaseDoorMalfunction<Gate>
    {
        public override string Name => "Поломка ворот";

        public override int MinDuration => 15;

        public override int MaxDuration => 50;

        public override int Cooldown => 210;

        public override int Chance => 23;

        public override void Activate(int duration)
        {
            base.Activate(duration);

            if (Value == null)
            {
                return;
            }

            Map.Broadcast(12, $"<color=#780000><b>Внимание всем!\nПроизошла {Name.ToLower()} {Parse(Value.Type)}, исправление займет {GetSecondsString(duration)}</b></color>");

            Value.Lock(duration, DoorLockType.SpecialDoorFeature);
        }

        private string Parse(DoorType door) => door switch
        {
            DoorType.GateB => "Б",
            DoorType.GateA => "А",
            DoorType.SurfaceGate => "на Поверхности",
            DoorType.Scp914Gate => "SCP-914",
            DoorType.Scp173NewGate => "SCP-173",
            DoorType.Scp173Gate => "старого К.С. SCP-173",
            DoorType.Scp049Gate => "SCP-049",
            DoorType.GR18Gate => "GR-18",
            DoorType.CheckpointGate => "КПП",
            _ => "???"
        };
    }
}
