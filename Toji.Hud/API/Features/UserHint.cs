using Exiled.API.Features;
using Hints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Toji.Hud.API.Enums;

namespace Toji.Hud.API.Features
{
    public class UserHint
    {
        private string _original;

        public UserHint(string text, float duration, HintPosition position = HintPosition.Center)
        {
            _original = text;
            Duration = duration;
            Position = position;

            StringVariables = new(10);
            PlayerVariables = new(10);
            Variables = new(10);
        }

        public Dictionary<string, Func<string, string>> StringVariables { get; }

        public Dictionary<string, Func<Player, string>> PlayerVariables { get; }

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

                foreach (var variable in StringVariables)
                {
                    var value = variable.Value.Invoke(result.Replace($"%{variable.Key}%", string.Empty));

                    result = result.Replace($"%{variable.Key}%", value);
                }

                return result;
            }
            set => _original = value;
        }

        public float Duration { get; }

        public float Time { get; set; } = 0;

        public string GetTextFor(Player player)
        {
            var result = _original;

            foreach (var variable in Variables)
            {
                result = result.Replace($"%{variable.Key}%", variable.Value.Invoke());
            }

            foreach (var variable in PlayerVariables)
            {
                result = result.Replace($"%{variable.Key}%", variable.Value.Invoke(player));
            }

            foreach (var variable in StringVariables)
            {
                var value = variable.Value.Invoke(result.Replace($"%{variable.Key}%", string.Empty));

                result = result.Replace($"%{variable.Key}%", value);
            }

            return result;
        }

        public void AddAdaptiveSpace() => StringVariables.Add("adaptive_space", GetSpaces);

        public void AddVariable(string name, string result) => Variables.Add(name, () => result);

        public void AddVariable(string name, Func<string> func) => Variables.Add(name, func);

        public void AddVariable(string name, Func<string, string> func) => StringVariables.Add(name, func);

        public void AddVariable(string name, Func<Player, string> func) => PlayerVariables.Add(name, func);

        private static string GetSpaces(string text)
        {
            float multiplayer = 1;

            if (text.Contains("size="))
            {
                var value = text.Split('<', '>').First(x => x.Contains("size"));

                if (value.Last() != '%')
                {
                    multiplayer = 1;
                }
                else
                {
                    value = value.Split('=').Last();

                    value = value.Remove(value.Length - 1, 1);

                    multiplayer = float.Parse(value) / 100f;
                }
            }

            int main = 0;

            var originalSize = text.GetWidth(multiplayer);
            var spaceSize = " ".GetWidth(multiplayer) * 2.2f;

            while (originalSize + spaceSize * main <= Constants.MaxWidth)
            {
                main++;
            }

            int second = 0;

            originalSize += spaceSize * main;

            while (originalSize + spaceSize * second * 0.1f <= Constants.MaxWidth)
            {
                second++;
            }

            return $"<space={main}{(second > 0 ? $".{second}" : string.Empty)}em>";
        }
    }
}
