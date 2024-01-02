using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using System.Collections.Generic;
using Toji.ExiledAPI.Extensions;
using Toji.Hud.Timers;

namespace Toji.Hud.Handlers
{
    internal sealed class PlayerHandlers
    {
        private RespawnTimer _respawnTimer;

        public PlayerHandlers()
        {
            _respawnTimer = new();
        }

        public void OnVerified(VerifiedEventArgs ev)
        {
            if (!ev.IsValid())
            {
                return;
            }

            ev.Player.Broadcast(8, $"<size=140%><b><color=#068DA9>Привет, {ev.Player.CustomName}!\nРады тебя видить, надеемся что ты задержишься у нас!</color></b></size>");

            Timing.RunCoroutine(_AdaptiveShower(ev.Player));
        }

        private IEnumerator<float> _AdaptiveShower(Player player)
        {
            yield return Timing.WaitUntilTrue(() => Round.InProgress);

            while (Round.IsStarted)
            {
                if (player.IsDead)
                {
                    yield return Timing.WaitUntilDone(_respawnTimer.Start(player));
                }
                else
                {

                }

                yield return Timing.WaitForSeconds(1);
            }
        }
    }
}
