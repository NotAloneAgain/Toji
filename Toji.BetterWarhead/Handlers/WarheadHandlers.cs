using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Warhead;
using MEC;
using System.Collections.Generic;
using Toji.BetterWarhead.API;
using Toji.BetterWarhead.API.Features;
using UnityEngine;

namespace Toji.BetterWarhead.Handlers
{
    internal sealed class WarheadHandlers
    {
        private List<BaseEvent> _events;

        internal WarheadHandlers() => CreateEvents();

        public void OnDetonating(DetonatingEventArgs ev)
        {
            if (!ev.IsAllowed)
            {
                return;
            }

            var surface = Door.Get(DoorType.SurfaceGate);

            Door.Get(DoorType.EscapePrimary).LockdownDoor();
            Door.Get(DoorType.EscapeSecondary).LockdownDoor();
            surface.LockdownDoor();
            Door.Get(DoorType.NukeSurface).LockdownDoor();

            Map.TurnOffAllLights(3, ZoneType.Surface);

            foreach (var pickup in Pickup.List)
            {
                if (pickup == null || !pickup.Position.InComplex())
                {
                    continue;
                }

                pickup.Destroy();
            }

            foreach (var player in Player.List)
            {
                if (player == null || player.IsHost || player.IsDead || player.IsNPC || !player.Position.InComplex())
                {
                    continue;
                }

                if (player.IsGodModeEnabled && !player.RemoteAdminAccess)
                {
                    player.Teleport(surface.Position + Vector3.up);

                    continue;
                }

                player.ClearInventory();
            }
        }

        public void OnDetonated()
        {
            foreach (var ragdoll in Ragdoll.List)
            {
                if (ragdoll == null || !ragdoll.Position.InComplex())
                {
                    continue;
                }

                ragdoll.Destroy();
            }

            var selectedEvent = _events.GetRandomValue();

            if (Random.Range(0, 100) > selectedEvent.Chance)
            {
                return;
            }

            Timing.CallDelayed(Random.Range(5, 20), selectedEvent.Start);
        }

        public void OnStarting(StartingEventArgs ev)
        {
            if (!ev.IsAllowed)
            {
                return;
            }

            string musicFile = ev.IsAuto switch
            {
                true => "Добавь сюда сирену долбаеб",
                false => "А тут выбор рандомного музончика"
            };

            // Some shit...
        }

        private void CreateEvents()
        {
            _events = new List<BaseEvent>();

            System.Type eventType = typeof(BaseEvent);

            foreach (System.Type type in GetType().Assembly.GetTypes())
            {
                if (!type.IsClass || type.IsAbstract || !type.IsSubclassOf(eventType))
                {
                    continue;
                }

                BaseEvent malfunction = System.Activator.CreateInstance(type) as BaseEvent;

                _events.Add(malfunction);
            }
        }
    }
}
