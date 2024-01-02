using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Hints;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toji.Hud.API.Enums;

namespace Toji.Hud.API.API
{
    public class UserInterface
    {
        private const string HintSkeleton = "<voffset=8.5em><line-height=95%>[Top][AfterTop][Center][AfterCenter]";
        private const float HintsUpdateTime = 0.1f;
        private const int CenterLinesPadding = 9;
        private const int TopLinesPadding = 16;

        private static List<(string Tag, UserHint Hint)> _globalHints;
        private static List<UserInterface> _interfaces;

        static UserInterface()
        {
            _globalHints = new (100);
            _interfaces = new(100);
        }

        private List<(string Tag, UserHint Hint)> _hints;
        private Player _player;

        public UserInterface()
        {
            _hints = new();

            _interfaces.Add(this);

            Timing.RunCoroutine(_Update());
        }

        public UserInterface(Player player) : this() => _player = player;

        public static UserInterface Get(Player player) => _interfaces.Find(ui => ui.Owner == player);

        public static bool TryGet(Player player, out UserInterface ui) => (ui = Get(player)) != null;

        public static void AddGlobal(string tag, UserHint hint, bool isInstant = false)
        {
            if (isInstant)
            {
                _globalHints.Insert(0, (tag, hint));
            }
            else
            {
                _globalHints.Add((tag, hint));
            }
        }

        public static bool RemoveGlobal(string tag)
        {
            var data = _globalHints.Find(data => data.Tag.Equals(tag, StringComparison.OrdinalIgnoreCase));

            return _globalHints.Remove(data);
        }

        public static void ClearGlobal() => _globalHints.Clear();

        public Player Owner => _player;

        public void Add(string tag, UserHint hint)
        {
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

        public (string Tag, UserHint Hint) GetData(string tag) => _hints.Find(data => data.Tag.Equals(tag, StringComparison.OrdinalIgnoreCase));

        public bool Remove(string tag)
        {
            var data = GetData(tag);

            return _hints.Remove(data);
        }

        public void Clear() => _hints.Clear();

        private IEnumerator<float> _Update()
        {
            while (Owner != null && !Owner.IsHost && Owner.IsConnected && !Owner.IsNPC)
            {
                yield return Timing.WaitForSeconds(HintsUpdateTime);

                var hints = GetAllHints();

                if (hints.IsEmpty())
                {
                    yield return Timing.WaitUntilTrue(() => !GetAllHints().IsEmpty());

                    continue;
                }

                string nextHintText = HintSkeleton;

                int top = 0;
                int center = 0;
                int bottom = 0;

                foreach (var hint in hints)
                {
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
                    .Replace("[AfterTop]", GetNewLines(center + top, TopLinesPadding))
                    .Replace("[Center]", string.Empty)
                    .Replace("[AfterCenter]", GetNewLines(bottom, CenterLinesPadding));

                _player.HintDisplay.Show(new TextHint(nextHintText,
                [
                    new StringHintParameter(nextHintText)
                ],
                [
                    HintEffectPresets.PulseAlpha(0.734f, 1, 1)
                ], HintsUpdateTime * 5));

                hints.Clear();

                ListPool<UserHint>.Pool.Return(hints);
            }

            Clear();
        }

        private List<UserHint> GetAllHints()
        {
            var hints = ListPool<UserHint>.Pool.Get(100);

            for (var i = 0; i < _globalHints.Count; i++)
            {
                (string Tag, UserHint Hint) data = _globalHints[i];

                var hint = data.Hint;

                if (DateTime.Now >= hint.End)
                {
                    _globalHints.Remove(data);

                    continue;
                }

                hints.Add(hint);

                if (hint.Start == DateTime.MinValue)
                {
                    hint.Start = DateTime.Now;
                    hint.End = DateTime.Now.AddSeconds(hint.Duration);
                }
            }

            for (var i = 0; i < _hints.Count; i++)
            {
                (string Tag, UserHint Hint) data = _hints[i];

                var hint = data.Hint;

                if (DateTime.Now >= hint.End)
                {
                    _hints.Remove(data);

                    continue;
                }

                hints.Add(hint);

                if (hint.Start == DateTime.MinValue)
                {
                    hint.Start = DateTime.Now;
                    hint.End = DateTime.Now.AddSeconds(hint.Duration);
                }
            }

            return hints;
        }

        private static string GetNewLines(int currentLinesCount, int needLinesCount) => new('\n', needLinesCount - currentLinesCount);
    }
}
