using Exiled.Events.EventArgs.Player;

namespace Toji.InfinityAmmo.Handlers
{
    internal sealed class PlayerHandlers
    {
        public void OnReloadingWeapon(ReloadingWeaponEventArgs ev)
        {
            if (ev.Player == null || ev.Firearm == null || ev.Player.IsHost || ev.Player.IsNPC)

            ev.Player.ClearAmmo();

            ev.IsAllowed = true;

            ev.Player.SetAmmo(ev.Firearm.AmmoType, ev.Firearm.MaxAmmo);
        }
    }
}
