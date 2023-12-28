using Exiled.Events.EventArgs.Scp096;
using Toji.ExiledAPI.Extensions;

namespace Toji.Hud.Handlers
{
    internal sealed class Scp096Handlers
    {
        public void OnAddingTarget(AddingTargetEventArgs ev)
        {
            if (!ev.IsValid() || !ev.IsLooking || !ev.IsAllowed)
            {
                return;
            }

            ev.Target.ShowHint("<line-height=90%><voffset=4.5em><size=88%><color=#7E1717>Вы посмотрели на SCP-096, кажется это конец...</color></size></voffset>", 6);
        }
    }
}
