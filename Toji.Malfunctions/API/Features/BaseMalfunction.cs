using Exiled.API.Enums;
using Exiled.API.Features;
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

        public abstract int Cooldown { get; }

        public abstract int Chance { get; }

        public TimeSpan Existance => DateTime.Now - _activateTime;

        public void Start()
        {
            _activateTime = DateTime.Now;

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

        public abstract void Activate(int duration);

        public abstract void Subscribe();

        public abstract void Unsubscribe();

        protected virtual IEnumerator<float> _Coroutine()
        {
            while (true)
            {
                var minutes = Existance.TotalMinutes;

                if (Loader.Random.Next(0, 100) < Chance + minutes)
                {
                    var min = (int)(MinDuration + minutes * 2);
                    var max = (int)(MaxDuration + minutes * 3);

                    Log.Info($"Min: {min}");
                    Log.Info($"Max: {max}");

                    Activate(Loader.Random.Next(min, max));

                    yield return Timing.WaitForSeconds(60 + Cooldown);
                }
                else
                {
                    yield return Timing.WaitForSeconds(60);
                }
            }
        }

        protected string TranslateZone(ZoneType zone) => zone switch
        {
            ZoneType.Unspecified => "во всем объекте",
            ZoneType.Other => "в комплексе",
            ZoneType.LightContainment => "в лёгкой зоне содержания",
            ZoneType.HeavyContainment => "в тяжелой зоне содержания",
            ZoneType.Entrance => "в офисной зоне",
            ZoneType.Surface => "на Поверхности",
            _ => "неизвестно"
        };

        protected string GetSecondsString(int seconds)
        {
            var secondsInt = seconds;

            var secondsString = secondsInt switch
            {
                int n when n % 100 is >= 11 and <= 14 => "секунд",
                int n when n % 10 == 1 => "секунду",
                int n when n % 10 is >= 2 and <= 4 => "секунды",
                _ => "секунд"
            };

            return $"{secondsInt} {secondsString}";
        }
    }
}
