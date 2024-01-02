using Exiled.API.Features;
using MEC;
using Toji.Hud.API.API;

namespace Toji.Hud.API
{
    public static class Extensions
    {
        public static UserInterface GetUserInterface(this Player player)
        {
            if (UserInterface.TryGet(player, out var ui))
            {
                ui.Awake();

                return ui;
            }
            else
            {
                ui = new UserInterface(player);

                ui.Awake();

                return ui;
            }
        }
    }
}
