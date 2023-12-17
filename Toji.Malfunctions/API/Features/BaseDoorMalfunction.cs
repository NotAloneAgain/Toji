using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Player;
using System;
using System.Linq;
using Toji.ExiledAPI.Extensions;

namespace Toji.Malfunctions.API.Features
{
    public abstract class BaseDoorMalfunction<TDoor> : ObjectMalfunction<TDoor> where TDoor : Door
    {
        public virtual Func<TDoor, bool> Condition => (TDoor door) => door == null || door.IsLocked || door.IsElevator || !door.AllowsScp106 || door.Is<BreakableDoor>(out var breakable) && breakable.IsDestroyed || door.Zone == ZoneType.LightContainment && Map.IsLczDecontaminated;

        public override void Subscribe() => Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;

        public override void Unsubscribe() => Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;

        public override TDoor SelectObject()
        {
            TDoor value = null!;

            var doors = Door.List.Where(door => door is TDoor);

            if (!doors.Any())
            {
                return null;
            }

            do
            {
                value = doors.GetRandomValue() as TDoor;
            } while (Condition(value));

            return value;
        }

        protected virtual void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (Value != ev.Door || ev.IsAllowed || !ev.Door.IsLocked || !ev.IsValid())
            {
                return;
            }

            ev.Player.ShowHint("<line-height=90%><voffset=4.5em><size=88%><color=#7E1717>Похоже что эта дверь сломана, ты не можешь воспользоваться ей</color></size></voffset>", 6);
        }
    }
}
