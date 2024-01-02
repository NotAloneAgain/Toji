using Exiled.API.Features;
using Toji.Hud.API.API;

namespace Toji.Hud.API
{
    public static class Extensions
    {
        public static UserInterface GetUserInterface(this Player player)
        {
            if (UserInterface.TryGet(player, out var ui))
            {
                return ui;
            }
            else
            {
                return new UserInterface(player);
            }
        }
    }
}
