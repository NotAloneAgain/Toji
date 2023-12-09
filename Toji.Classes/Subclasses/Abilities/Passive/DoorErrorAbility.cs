using Exiled.API.Enums;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Handlers;
using System.Linq;
using Toji.Classes.API.Features.Abilities;
using Toji.ExiledAPI.Extensions;

namespace Toji.Classes.Subclasses.Abilities.Passive
{
    public class DoorErrorAbility : ChanceAbility
    {
        public DoorErrorAbility(int chance) : base(chance) { }

        public override string Name => "Ошибочный доступ";

        public override string Desc => $"Ты можешь открыть дверь, не имея к ней доступа, с шансом {Chance}%";

        public override void Subscribe() => Player.InteractingDoor += OnInteractingDoor;

        public override void Unsubscribe() => Player.InteractingDoor -= OnInteractingDoor;

        private void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (!ev.IsValid() || ev.IsAllowed || !Has(ev.Player) || !GetRandom() || ev.Door.IsOpen)
            {
                return;
            }

            foreach (Door door in ev.Player.CurrentRoom.Doors.Where(door => door != ev.Door))
            {
                door.IsOpen = false;
                door.Lock(0.75f, DoorLockType.NoPower);
            }

            if (ev.Player.CurrentRoom == null)
            {
                ev.Door.Room?.TurnOffLights(1);
            }
            else
            {
                ev.Player.CurrentRoom.TurnOffLights(1);
            }

            ev.IsAllowed = true;
        }
    }
}
