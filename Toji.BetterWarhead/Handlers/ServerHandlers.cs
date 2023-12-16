using UnityEngine;

namespace Toji.BetterWarhead.Handlers
{
    internal sealed class ServerHandlers
    {
        public void OnRestartingRound() => Physics.gravity = new(0, -9.81f, 0);
    }
}
