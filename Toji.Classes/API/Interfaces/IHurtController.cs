using Exiled.Events.EventArgs.Player;

namespace Toji.Classes.API.Interfaces
{
    public interface IHurtController
    {
        void OnHurt(HurtingEventArgs ev);
    }
}
