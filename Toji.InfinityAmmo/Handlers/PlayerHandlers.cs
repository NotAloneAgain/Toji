using Exiled.Events.EventArgs.Player;
using Toji.ExiledAPI.Extensions;

namespace Toji.InfinityAmmo.Handlers
{
    internal sealed class PlayerHandlers
    {
        public void OnReloadingWeapon(ReloadingWeaponEventArgs ev)
        {
            if (ev.IsValid() && ev.Firearm == null)
            {
                return;;
            }

            ev.Player.ClearAmmo();

            ev.IsAllowed = true;

            ev.Player.SetAmmo(ev.Firearm.AmmoType, ev.Firearm.MaxAmmo);
        }
    }
}
