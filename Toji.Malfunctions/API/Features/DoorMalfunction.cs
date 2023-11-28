using Exiled.API.Extensions;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Player;
using System.Linq;
using Toji.Malfunctions.API.Interfaces;

namespace Toji.Malfunctions.API.Features
{
    public abstract class DoorMalfunction<TDoor> : ObjectMalfunction<TDoor> where TDoor : Door
    {
        private protected override void Subscribe()
        {
            Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
        }

        private protected override void Unsubscribe()
        {
            Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
        }

        public abstract void OnFailedUse(InteractingDoorEventArgs ev);

        private void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (Object != ev.Door)
            {
                return;
            }

            ev.IsAllowed = false;

            OnFailedUse(ev);

            if (ev.IsAllowed)
            {
                return;
            }

            if (this is IHintMalfunction hint)
            {
                ev.Player.ShowHint(hint.AttemptText, 6);
            }
        }

        protected override TDoor SelectObject() => Door.List.Select(door => door as TDoor).GetRandomValue();
    }
}
