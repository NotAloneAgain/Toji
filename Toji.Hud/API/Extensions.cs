using Exiled.API.Features;
using MEC;
using System.Linq;
using Toji.Hud.API.Features;
using static System.Net.Mime.MediaTypeNames;

namespace Toji.Hud.API
{
    public static class Extensions
    {
        public static UserInterface GetUserInterface(this Player player)
        {
            if (player == null || player.GameObject == null)
            {
                return null;
            }

            if (player.GameObject.TryGetComponent<UserInterface>(out var ui))
            {
                return ui;
            }
            else
            {
                return player.GameObject.AddComponent<UserInterface>();
            }
        }

        public static float GetWidth(this string str)
        {
            float multiplayer = 1;

            if (str.Contains("size="))
            {
                var value = str.Split('<', '>').First(x => x.Contains("size"));

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

            return str.GetWidth(multiplayer);
        }

        public static float GetWidth(this string str, float multiplayer = 1) => str.Select(x => Constants.Sizes.TryGetValue(x, out var value) ? value : 0).Sum() * multiplayer;
    }
}
