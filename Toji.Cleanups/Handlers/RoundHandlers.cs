using MEC;
using System.Collections.Generic;

namespace Toji.Cleanups.Handlers
{
    internal sealed class RoundHandlers
    {
        private List<CoroutineHandle> _coroutines = new List<CoroutineHandle>(2);

        public void OnRoundStarted()
        {
            _coroutines.Clear();
        }

        public void OnRestartingRound()
        {
            Timing.KillCoroutines(_coroutines.ToArray());
        }
    }
}
