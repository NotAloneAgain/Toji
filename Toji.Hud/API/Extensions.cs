using Exiled.API.Features;
using MEC;
using Toji.Hud.API.Features;

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
    }
}
