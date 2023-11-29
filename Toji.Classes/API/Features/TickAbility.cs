using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;

namespace Toji.Classes.API.Features
{
    public abstract class TickAbility : PassiveAbility
    {
        private CoroutineHandle _coroutineHandle;

        public abstract int TickPerSecond { get; }

        public virtual Func<bool> GlobalCondition { get; } = () => Round.InProgress;

        public virtual Func<Player, bool> PlayerCondition { get; } = (_) => true;

        public virtual void GlobalIteration() { }

        public virtual void Iteration(Player player) { }

        public override void Subscribe()
        {
            throw new System.NotImplementedException();
        }

        public override void Unsubscribe()
        {
            throw new System.NotImplementedException();
        }

        internal protected void StartCoroutine() => _coroutineHandle = Timing.RunCoroutine(_Coroutine());

        internal protected void StopCoroutine()
        {
            if (_coroutineHandle == default)
            {
                return;
            }

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
                    {
                        continue;
                    }

                    Iteration(player);
                }

                GlobalIteration();

                yield return Timing.WaitForSeconds(1 / TickPerSecond);
            }
        }
    }
}
