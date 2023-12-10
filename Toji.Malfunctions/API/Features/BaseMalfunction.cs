using Exiled.Loader;
using MEC;
using System;
using System.Collections.Generic;

namespace Toji.Malfunctions.API.Features
{
    public abstract class BaseMalfunction
    {
        private protected DateTime _activateTime;
        private CoroutineHandle _coroutine;

        public abstract string Name { get; }

        public abstract int MinDuration { get; }

        public abstract int MaxDuration { get; }

        public abstract int Chance { get; }

        public TimeSpan Existance => DateTime.Now - _activateTime;

        public void Start()
        {
            _coroutine = Timing.RunCoroutine(_Coroutine());
        }

        public void Stop()
        {
            if (_coroutine == default)
            {
                return;
            }

            _coroutine.IsRunning = false;

            _coroutine = default;
        }

        public virtual void Activate(int duration)
        {
            _activateTime = DateTime.Now;
        }

        public abstract void Subscribe();

        public abstract void Unsubscribe();

        protected virtual IEnumerator<float> _Coroutine()
        {
            while (true)
            {
                var minutes = Existance.TotalMinutes;

                if (Loader.Random.Next(0, 100) < Chance + minutes)
                {
                    Activate(Loader.Random.Next(MinDuration + (int)Math.Round(minutes * 2f, 0), MaxDuration + (int)Math.Round(minutes * 3f, 0)));
                }

                yield return Timing.WaitForSeconds(60);
            }
        }
    }
}
