using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toji.Malfunctions.API.Interfaces;

namespace Toji.Malfunctions.API.Features
{
    public abstract class BaseMalfunction
    {
        public virtual void Activate()
        {
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
            Unsubscribe();
        }

        private protected abstract void Subscribe();

        private protected abstract void Unsubscribe();
    }
}
