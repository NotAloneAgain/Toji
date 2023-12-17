using Exiled.API.Enums;
using Exiled.API.Features;

namespace Toji.Malfunctions.API.Features.Malfunctions
{
    public class RoomIsolationMalfunction : ObjectMalfunction<Room>
    {
        public override string Name => "Изоляция комнаты";

        public override int MinDuration => 30;

        public override int MaxDuration => 60;

        public override int Cooldown => 260;

        public override int Chance => 21;

        public override void Activate(int duration)
        {
            base.Activate(duration);

            if (Value == null)
            {
                return;
            }

            Map.Broadcast(12, $"<color=#780000><b>Внимание всем!\nПроизошла {Name.ToLower()} {TranslateZone(Value.Zone)}, подождите {GetSecondsString(duration)}</b></color>");

            Value.Blackout(duration);

            foreach (var player in Value.Players)
            {
                player.EnableEffect(EffectType.Decontaminating, duration);
            }
        }

        public override Room SelectObject()
        {
            if (Warhead.IsDetonated)
            {
                return null;
            }

            Room value = null!;

            do
            {
                value = Room.Random();
            } while (value == null || value.Zone == ZoneType.LightContainment && Map.IsLczDecontaminated || value.Zone == ZoneType.Surface);

            return value;
        }
    }
}
