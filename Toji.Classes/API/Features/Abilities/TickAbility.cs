using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;

namespace Toji.Classes.API.Features.Abilities
{
    public abstract class TickAbility : PassiveAbility
    {
        private CoroutineHandle _coroutineHandle;

        public virtual ushort TicksPerSecond { get; } = 1;

        public virtual Func<bool> GlobalCondition { get; } = () => Round.InProgress;

        public virtual Func<Player, bool> PlayerCondition { get; } = (Player ply) => ply != null;

        public virtual void GlobalIteration() { }

        public virtual void Iteration(Player player) { }

        public override void Subscribe()
        {
            Exiled.Events.Handlers.Server.RestartingRound += StopCoroutine;
            Exiled.Events.Handlers.Server.RoundStarted += StartCoroutine;
        }

        public override void Unsubscribe()
        {
            StopCoroutine();

            Exiled.Events.Handlers.Server.RoundStarted -= StartCoroutine;
            Exiled.Events.Handlers.Server.RestartingRound -= StopCoroutine;
        }

        protected internal void StartCoroutine() => _coroutineHandle = Timing.RunCoroutine(_Coroutine());

        protected internal void StopCoroutine()
        {
            if (_coroutineHandle == default)
                return;

            Timing.KillCoroutines(_coroutineHandle);

            _coroutineHandle = default;
        }

        private IEnumerator<float> _Coroutine()
        {
            while (GlobalCondition())
            {
                foreach (var player in Owners)
                {
                    if (!PlayerCondition(player))
                        continue;

                    Iteration(player);
                }

                GlobalIteration();

                yield return Timing.WaitForSeconds(1 / TicksPerSecond);
            }
        }
    }
}
