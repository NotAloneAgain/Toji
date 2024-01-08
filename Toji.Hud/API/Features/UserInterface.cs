using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Hints;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using Toji.Hud.API.Enums;
using UnityEngine;

namespace Toji.Hud.API.Features
{
    public class UserInterface : MonoBehaviour
    {
        private List<(string Tag, UserHint Hint)> _hints;
        private CoroutineHandle _coroutine;
        private Player _player;

        public Player Owner => _player;

        public void Add(string tag, UserHint hint)
        {
            hint.Time = 0;

            for (var index = 0; index < _hints.Count; index++)
            {
                var data = _hints[index];

                if (data.Tag != tag)
                {
                    continue;
                }

                data.Hint = hint;

                return;
            }

            _hints.Add((tag, hint));
        }

        public void AddInstant(string tag, UserHint hint)
        {
            hint.Time = 0;

            for (var index = 0; index < _hints.Count; index++)
            {
                var data = _hints[index];

                if (data.Tag != tag)
                {
                    continue;
                }

                _hints.Remove(data);
            }

            _hints.Insert(0, (tag, hint));
        }

        public bool Remove(string tag)
        {
            var data = _hints.Find(data => data.Tag.Equals(tag, StringComparison.OrdinalIgnoreCase));

            return _hints.Remove(data);
        }

        public void Clear() => _hints.Clear();

        private void Awake()
        {
            _hints = [];

            _player = Player.Dictionary[gameObject];
        }

        private void Start() => _coroutine = Timing.RunCoroutine(_Update());

        private void OnDestroy()
        {
            _hints.Clear();

            Owner.ShowHint(string.Empty, 0);

            if (_coroutine.IsRunning)
            {
                Timing.KillCoroutines(_coroutine);
            }

            _coroutine = default;

            Owner.ShowHint(string.Empty, 0);
        }

        private IEnumerator<float> _Update()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(Constants.UpdateTime);

                if (gameObject == null || Owner == null || Owner.IsHost || !Owner.IsConnected || Owner.IsNPC)
                {
                    Destroy(this);

                    yield break;
                }

                var hints = GetAllHints();

                if (hints.IsEmpty())
                {
                    continue;
                }

                string nextHintText = Constants.HintSkeleton;

                int top = 0;
                int center = 0;
                int bottom = 0;

                foreach (var hint in hints)
                {
                    hint.Time += Constants.UpdateTime;

                    switch (hint.Position)
                    {
                        case HintPosition.Top:
                            {
                                top += hint.Text.Count(c => c == '\n') + 1;

                                if (nextHintText.Contains("[Top]"))
                                {
                                    nextHintText = nextHintText.Replace("[Top]", hint.Text);

                                    continue;
                                }

                                nextHintText = nextHintText.Replace("[AfterTop]", '\n' + hint.Text + "[AfterTop]");

                                continue;
                            }
                        case HintPosition.Bottom:
                            {
                                bottom += hint.Text.Count(c => c == '\n') + 1;

                                nextHintText += Environment.NewLine + hint.Text;

                                continue;
                            }
                        default:
                            {
                                center += hint.Text.Count(x => x == '\n') + 1;

                                if (nextHintText.Contains("[Center]"))
                                {
                                    nextHintText = nextHintText.Replace("[Center]", hint.Text);
                                    continue;
                                }

                                nextHintText = nextHintText.Replace("[AfterCenter]", '\n' + hint.Text + "[AfterCenter]");

                                continue;
                            }
                    }
                }

                if (center == 0)
                {
                    center = 1;
                }

                if (top == 0)
                {
                    top = 1;
                }

                nextHintText = nextHintText.Replace("[Top]", string.Empty)
                    .Replace("[AfterTop]", GetNewLines(center + top, Constants.TopLinesPadding))
                    .Replace("[Center]", string.Empty)
                    .Replace("[AfterCenter]", GetNewLines(bottom, Constants.CenterLinesPadding));

                _player.HintDisplay.Show(new TextHint(nextHintText,
                [
                    new StringHintParameter(nextHintText)
                ], Constants.Effects));

                hints.Clear();

                ListPool<UserHint>.Pool.Return(hints);
            }
        }

        private List<UserHint> GetAllHints()
        {
            var hints = ListPool<UserHint>.Pool.Get(100);

            _hints.RemoveAll(x => x.Hint.Time > x.Hint.Duration);

            for (var i = 0; i < _hints.Count; i++)
            {
                (string Tag, UserHint Hint) data = _hints[i];

                var hint = data.Hint;

                hints.Add(hint);
            }

            return hints;
        }

        private static string GetNewLines(int currentLinesCount, int needLinesCount) => new ('\n', needLinesCount - currentLinesCount);
    }
}
