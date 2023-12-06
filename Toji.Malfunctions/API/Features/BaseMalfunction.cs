﻿using Exiled.API.Features;
using Toji.Malfunctions.API.Interfaces;

namespace Toji.Malfunctions.API.Features
{
    public abstract class BaseMalfunction
    {
        public abstract string Name { get; }

        public abstract int Chance { get; }

        public abstract int Duration { get; }

        public bool IsActive { get; protected set; }

        public virtual void Activate(int duration)
        {
            IsActive = true;

            if (this is ICassieMalfunction cassie)
            {
                Cassie.MessageTranslated(cassie.WarningText, cassie.WarningSubtitles);
            }

            if (this is IBroadcastMalfunction broadcast)
            {
                Map.Broadcast(12, broadcast.BroadcastText);
            }

            Subscribe();
        }

        public virtual void Deactivate()
        {
            IsActive = false;

            Unsubscribe();
        }

        private protected abstract void Subscribe();

        private protected abstract void Unsubscribe();
    }
}
