using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Toji.Hud.API.Features
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
            if (player == null)
            {
                return;
            }

            if (_coroutine.TryGetValue(player, out var coroutine))
            {
                Timing.KillCoroutines(coroutine);

                _coroutine.Remove(player);

                var component = player.GetUserInterface();

                if (component != null)
                {
                    GameObject.Destroy(component);
                }
            }
        }

        protected IEnumerator<float> _Coroutine(Player player)
        {
            while (Conditions(player))
            {
                yield return Timing.WaitForSeconds(Constants.UpdateTime);

                try
                {
                    Iteration(player, player.GetUserInterface());
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
