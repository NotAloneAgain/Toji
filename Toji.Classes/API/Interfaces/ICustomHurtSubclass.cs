using Exiled.Events.EventArgs.Player;

namespace Toji.Classes.API.Interfaces
{
    public interface ICustomHurtSubclass
    {
        float HurtMultiplayer { get; }

        void OnHurt(HurtingEventArgs ev);
    }
}
