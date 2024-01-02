using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;

namespace Toji.Hud.API.API
{
    public abstract class Timer
    {
        private Dictionary<Player, CoroutineHandle> _coroutine;

        public Timer() => _coroutine = new(Server.MaxPlayerCount);

        public abstract string Tag { get; }

        public virtual Func<Player, bool> Conditions { get; } = (Player player) => player != null && player.IsConnected && !player.IsHost && !player.IsNPC;

        public CoroutineHandle Start(Player player)
        {
            if (_coroutine.ContainsKey(player))
            {
                return default;
            }

            var coroutine = Timing.RunCoroutine(_Coroutine(player));

            _coroutine.Add(player, coroutine);

            return coroutine;
        }

        public void End(Player player)
        {
            if (_coroutine.TryGetValue(player, out var coroutine))
            {
                Timing.KillCoroutines(coroutine);

                _coroutine.Remove(player);
            }
        }

        protected IEnumerator<float> _Coroutine(Player player)
        {
            var component = player.GetUserInterface();

            while (Conditions(player))
            {
                yield return Timing.WaitForSeconds(0.1f);

                try
                {
                    Iteration(player, component);
                }
                catch (Exception err)
                {
                    Log.Error(err);
                }
            }
        }

        protected abstract void Iteration(Player player, UserInterface component);
    }
}
