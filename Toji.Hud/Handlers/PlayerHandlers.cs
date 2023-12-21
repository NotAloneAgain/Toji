using Exiled.Events.EventArgs.Player;
using Toji.ExiledAPI.Extensions;

namespace Toji.Hud.Handlers
{
    internal sealed class PlayerHandlers
    {
        public void OnVerified(VerifiedEventArgs ev)
        {
            if (!ev.IsValid())
            {
                return;
            }

            ev.Player.Broadcast(8, $"<size=140%><b><color=#068DA9>Привет, {ev.Player.CustomName}!\nРады тебя видить, надеемся что ты задержишься у нас!</color></b></size>");
        }
    }
}
