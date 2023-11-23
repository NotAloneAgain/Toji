using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Player;
using Toji.Malfunctions.API.Interfaces;

namespace Toji.Malfunctions.API.Features
{
    public abstract class DoorMalfunction<TDoor> : ObjectMalfunction<TDoor> where TDoor : Door
    {
        private protected override void Subscribe()
        {
            Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingElevator;
        }

        private protected override void Unsubscribe()
        {
            Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingElevator;
        }

        public abstract void OnFailedUse(InteractingDoorEventArgs ev);

        private void OnInteractingElevator(InteractingDoorEventArgs ev)
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
    }
}
