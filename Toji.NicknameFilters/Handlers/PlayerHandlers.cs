using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;
using Toji.ExiledAPI.Extensions;

namespace Toji.NicknameFilters.Handlers
{
    internal sealed class PlayerHandlers
    {
        private readonly List<string> _replaces;
        private readonly List<string> _kicks;
        private readonly List<string> _ads;

        internal PlayerHandlers(List<string> replaces, List<string> kicks, List<string> ads)
        {
            _replaces = replaces;
            _kicks = kicks;
            _ads = ads;
        }

        public void OnVerified(VerifiedEventArgs ev)
        {
            if (!ev.IsValid() || ev.Player.KickPower >= 250)
            {
                return;
            }

            foreach (var ad in _ads)
            {
                if (ev.Player.Nickname.IndexOf(ad, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                ev.Player.Kick("[NICKNAME-FILTER] РЕКЛАМА/ADVERTISEMENT", Server.Host);

                return;
            }

            foreach (var kick in _kicks)
            {
                if (ev.Player.Nickname.IndexOf(kick, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                ev.Player.Kick("[NICKNAME-FILTER] МАТЫ ИЛИ ЗАПРЕЩЕННЫЕ СЛОВА/BAD WORDS", Server.Host);

                return;
            }

            foreach (var replace in _replaces)
            {
                if (ev.Player.CustomName.IndexOf(replace, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                ev.Player.CustomName = ev.Player.CustomName.Replace(replace, new string('*', replace.Length));
            }
        }
    }
}
