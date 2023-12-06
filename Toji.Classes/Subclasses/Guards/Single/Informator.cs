using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System.Linq;
using Toji.Classes.API.Interfaces;
using Toji.Global;

namespace Toji.Classes.Subclasses.Guards.Single
{
    public class Informator : GuardSingleSubclass, IHintSubclass, IRandomSubclass
    {
        public override bool ShowInfo => true;

        public override string Name => "Информатор";

        public override string Desc => "Имея доступ к камерам безопасности ты точно знаешь, какие SCP-Объекты ходят по комплексу";

        public int Chance => 12;

        protected internal override void LazySubscribe()
        {
            base.LazySubscribe();

            Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;
        }

        protected internal override void LazyUnsubscribe()
        {
            Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;

            base.LazyUnsubscribe();
        }

        protected override void OnAdded(in Player player, bool withAbility)
        {
            base.OnAdded(player, withAbility);

            if (withAbility)
            {
                player.SendConsoleMessage($"Сбежали: {string.Join(", ", Player.List.Where(ply => ply.IsScp && ply.Role.Type != RoleTypeId.Scp0492).Select(ply => ply.Role.Type.Translate()))}", "yellow");
            }
        }

        private void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.NewRole.GetTeam() != Team.SCPs)
            {
                return;
            }

            if (ev.Player.IsScp)
            {
                var text = $"{ev.Player.Role.Type.Translate()} стал {ev.NewRole.Translate()}";

                Player.ShowHint($"<line-height=95%><size=95%><voffset=-20em><b><color=#FF9500>{text}</color></b></voffset></size>", 5);
                Player.SendConsoleMessage(text, "yellow");
            }
            else
            {
                var text = $"Появился {ev.NewRole.Translate()}";

                Player.ShowHint($"<line-height=95%><size=95%><voffset=-20em><b><color=#FF9500>{text}</color></b></voffset></size>", 5);
                Player.SendConsoleMessage(text, "yellow");
            }
        }
    }
}
