using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using System;
using Toji.Malfunctions.API.Interfaces;

namespace Toji.Malfunctions.API.Features
{
    public abstract class LiftMalfunction : ObjectMalfunction<Lift>
    {
        private protected override void Subscribe()
        {
            Exiled.Events.Handlers.Player.InteractingElevator += OnInteractingElevator;
        }

        private protected override void Unsubscribe()
        {
            Exiled.Events.Handlers.Player.InteractingElevator -= OnInteractingElevator;
        }

        public abstract void OnFailedUse(InteractingElevatorEventArgs ev);

        private void OnInteractingElevator(InteractingElevatorEventArgs ev)
        {
            if (Object != ev.Lift)
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
