﻿using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Loader;
using MEC;
using System;
using System.Collections.Generic;
using Toji.Malfunctions.API.Features;

namespace Toji.Malfunctions.Handlers
{
    internal sealed class ServerHandlers
    {
        private List<BaseMalfunction> _handledMalfunctions;
        private List<BaseMalfunction> _allMalfunctions;

        internal ServerHandlers()
        {
            _handledMalfunctions = new List<BaseMalfunction>(10);
            _allMalfunctions = new List<BaseMalfunction>(100);
        }

        public void OnRoundStarted()
        {
            CreateMalfunctions();

            Timing.RunCoroutine(_Timer());
        }

        public void OnRestartingRound() => DestroyMalfunctions();

        private IEnumerator<float> _Timer()
        {
            var count = Player.List.Count;

            if (count <= 0)
            {
                yield break;
            }

            for (int i = 0; i < count / 10 && _allMalfunctions.Count > 0; i++)
            {
                yield return Timing.WaitForSeconds(Loader.Random.Next(10, 22));

                var malfunction = _allMalfunctions.GetRandomValue();

                if (Loader.Random.Next(0, 100) > 19)
                {
                    malfunction.Start();
                }

                _allMalfunctions.Remove(malfunction);
                _handledMalfunctions.Add(malfunction);
            }

            yield return Timing.WaitUntilTrue(() => Warhead.IsDetonated);

            DestroyMalfunctions();
        }

        private void CreateMalfunctions()
        {
            Type subclassType = typeof(BaseMalfunction);

            foreach (Type type in GetType().Assembly.GetTypes())
            {
                if (!type.IsClass || type.IsAbstract || !type.IsSubclassOf(subclassType))
                {
                    continue;
                }

                BaseMalfunction malfunction = Activator.CreateInstance(type) as BaseMalfunction;

                _allMalfunctions.Add(malfunction);
            }
        }

        private void DestroyMalfunctions()
        {
            foreach (var malfunction in _handledMalfunctions)
            {
                malfunction.Stop();
            }

            _allMalfunctions.AddRange(_handledMalfunctions);
            _handledMalfunctions.Clear();
        }
    }
}
