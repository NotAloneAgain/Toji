using Exiled.Events.EventArgs.Scp3114;
using UnityEngine;

namespace Toji.BetterRoles.Handlers
{
    internal sealed class Scp3114Handlers
    {
        public void OnDisguised(DisguisedEventArgs ev)
        {
            ev.Player.Scale = Vector3.one;
        }
    }
}
