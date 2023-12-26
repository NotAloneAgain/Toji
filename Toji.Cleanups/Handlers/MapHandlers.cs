﻿using Exiled.API.Extensions;
using Exiled.Events.EventArgs.Map;

namespace Toji.Cleanups.Handlers
{
    internal sealed class MapHandlers
    {
        public void OnPlacingBulletHole(PlacingBulletHoleEventArgs ev) => ev.IsAllowed = false;

        public void OnSpawningItem(SpawningItemEventArgs ev) => ev.IsAllowed = !ev.Pickup.Type.IsAmmo();
    }
}
