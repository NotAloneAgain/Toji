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

        private IEnumerator<float> _CleanupItems()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(1);
            }
        }

        private IEnumerator<float> _CleanupRagdolls()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(1);
            }
        }
    }
}
