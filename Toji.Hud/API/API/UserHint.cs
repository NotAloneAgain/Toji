using System;
using System.Collections.Generic;
using Toji.Hud.API.Enums;

namespace Toji.Hud.API.API
{
    public class UserHint
    {
        private string _original;

        public UserHint(string text, float duration, HintPosition position = HintPosition.Center)
        {
            _original = text;
            Duration = duration;
            Position = position;

            Start = DateTime.MinValue;
            End = DateTime.MinValue;

            Variables = new(10);
        }

        public Dictionary<string, Func<string>> Variables { get; }

        public HintPosition Position { get; }

        public string Text
        {
            get
            {
                var result = _original;

                foreach (var variable in Variables)
                {
                    result = result.Replace($"%{variable.Key}%", variable.Value.Invoke());
                }

                return result;
            }
            set => _original = value;
        }

        public float Duration { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public void AddVariable(string name, string result) => Variables.Add(name, () => result);

        public void AddVariable(string name, Func<string> func) => Variables.Add(name, func);
    }
}
