using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Player;
using Toji.ExiledAPI.Extensions;

namespace Toji.Malfunctions.API.Features.Malfunctions
{
    public class LiftMalfunction : ObjectMalfunction<Lift>
    {
        public override string Name => "Поломка лифта";

        public override int MinDuration => 15;

        public override int MaxDuration => 40;

        public override int Cooldown => 180;

        public override int Chance => 26;

        public override void Activate(int duration)
        {
            base.Activate(duration);

            if (Value.Type is ElevatorType.LczA or ElevatorType.LczB && Map.IsLczDecontaminated)
            {
                return;
            }

            Map.Broadcast(12, $"<color=#780000><b>Внимание всем!\nПроизошла {Name.ToLower()} {Parse(Value.Type)}, исправление займет {GetSecondsString(duration)}</b></color>");

            foreach (ElevatorDoor door in Value.Doors)
            {
                door.Lock(duration, DoorLockType.SpecialDoorFeature);

                if (Value.CurrentLevel != 1)
                {
                    Value.TryStart(1, true);
                }

                Value.Base.RefreshLocks(Value.Base.AssignedGroup, door.Base);
            }
        }

        public override Lift SelectObject() => Lift.Random;

        public override void Subscribe() => Exiled.Events.Handlers.Player.InteractingElevator += OnInteractingElevator;

        public override void Unsubscribe() => Exiled.Events.Handlers.Player.InteractingElevator -= OnInteractingElevator;

        protected virtual void OnInteractingElevator(InteractingElevatorEventArgs ev)
        {
            if (Value != ev.Lift || ev.IsAllowed || !ev.Lift.IsLocked || !ev.IsValid())
            {
                return;
            }

            ev.Player.ShowHint("<line-height=90%><voffset=4.5em><size=88%><color=#7E1717>Похоже что этот лифт сломан, ты не можешь воспользоваться им</color></size></voffset>", 6);
        }

        private string Parse(ElevatorType lift) => lift switch
        {
            ElevatorType.GateA => "ворот А",
            ElevatorType.GateB => "ворот Б",
            ElevatorType.Nuke => "Альфа-Боеголовки",
            ElevatorType.Scp049 => "К.С. SCP-049",
            ElevatorType.LczA => "ЛКЗ-А",
            ElevatorType.LczB => "ЛКЗ-Б",
            _ => "???",
        };
    }
}
