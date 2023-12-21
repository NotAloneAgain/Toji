using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Loader;
using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

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

        public virtual void Activate(int duration) => Log.Info($"Activate {GetType().Name}");

        public virtual void Subscribe() { }

        public virtual void Unsubscribe() { }

        protected virtual IEnumerator<float> _Coroutine()
        {
            while (Round.InProgress && !Warhead.IsDetonated)
            {
                var minutes = Existance.TotalMinutes;

                if (Loader.Random.Next(0, 100) < Chance + minutes)
                {
                    var min = Mathf.Clamp(MinDuration + (int)(minutes * 2), MinDuration, MaxDuration - 20);
                    var max = Mathf.Clamp(MaxDuration + (int)(minutes * 3), MinDuration + 20, MaxDuration);

                    var duration = Loader.Random.Next(min, max);

                    Subscribe();

                    Activate(duration);

                    yield return Timing.WaitForSeconds(duration);

                    Unsubscribe();

                    yield return Timing.WaitForSeconds(60 + Cooldown);
                }
                else
                {
                    yield return Timing.WaitForSeconds(60);
                }
            }
        }

        protected ZoneType SelectZone()
        {
            var zone = UnityEngine.Random.Range(0, 101) switch
            {
                >= 80 => ZoneType.Surface,
                >= 60 => ZoneType.Entrance,
                >= 40 => ZoneType.HeavyContainment,
                >= 20 => ZoneType.Other,
                >= 10 => ZoneType.Unspecified,
                _ => ZoneType.LightContainment
            };

            if (Warhead.IsDetonated)
            {
                zone = ZoneType.Surface;
            }

            if (Map.IsLczDecontaminated && zone == ZoneType.LightContainment)
            {
                zone = ZoneType.HeavyContainment;
            }

            return zone;
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
