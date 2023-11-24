using Exiled.Events.EventArgs.Player;

namespace Toji.Classes.API.Interfaces
{
    public interface ICustomDamageSubclass
    {
        float Damage { get; }

        float DamageMultiplayer { get; }

        void OnDamage(HurtingEventArgs ev);
    }
}
