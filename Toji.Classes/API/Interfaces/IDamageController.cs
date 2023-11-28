using Exiled.Events.EventArgs.Player;

namespace Toji.Classes.API.Interfaces
{
    public interface IDamageController
    {
        void OnDamage(HurtingEventArgs ev);
    }
}
