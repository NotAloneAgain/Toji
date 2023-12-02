using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles.Ragdolls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toji.Classes.API.Extensions;
using Toji.Classes.API.Features;

namespace Toji.Classes.Handlers
{
    internal sealed class RagdollHandlers
    {
        public void OnRagdollRemoved(BasicRagdoll basic)
        {
            var ragdoll = Ragdoll.Get(basic);

            if (ragdoll == null || !BaseSubclass.TryGet(ragdoll, out var subclass))
            {
                return;
            }

            subclass.Owners.Remove(ragdoll);
        }

        public void OnSpawningRagdoll(SpawningRagdollEventArgs ev)
        {
            if (!ev.IsAllowed || !ev.Player.TryGetSubclass(out var subclass))
            {
                return;
            }

            ev.Scale = ev.Player.Scale;
        }

        public void OnSpawnedRagdoll(SpawnedRagdollEventArgs ev)
        {
            if (!ev.Player.TryGetSubclass(out var subclass) || ev.Ragdoll == null || ev.Ragdoll.Role != subclass.Role)
            {
                return;
            }

            subclass.Owners.Add(ev.Ragdoll);
        }
    }
}
