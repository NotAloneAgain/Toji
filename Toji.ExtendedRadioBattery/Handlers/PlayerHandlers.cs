using Exiled.Events.EventArgs.Player;

namespace Toji.ExtendedRadioBattery.Handlers
{
    internal sealed class PlayerHandlers
    {
        public void OnUsingRadioBattery(UsingRadioBatteryEventArgs ev) => ev.Drain *= 0.09f;
    }
}
