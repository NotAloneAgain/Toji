using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Handlers;
using PlayerRoles;
using Toji.Classes.API.Features.Abilities;
using Toji.ExiledAPI.Extensions;
using Toji.Global;

namespace Toji.Classes.Subclasses.Abilities.Passive
{
    public class ReviveAbility : PassiveAbility
    {
        private RoleTypeId _target;
        private RoleTypeId _role;

        public ReviveAbility(RoleTypeId role, RoleTypeId target)
        {
            _target = target;
            _role = role;
        }

        public override string Name => "Возрождение";

        public override string Desc => $"После смерти ты станешь {_role.Translate()}";

        public override void Subscribe() => Player.Dying += OnDying;

        public override void Unsubscribe() => Player.Dying -= OnDying;

        private void OnDying(DyingEventArgs ev)
        {
            if (!ev.IsValid() || !IsEnabled || !ev.IsAllowed || !Has(ev.Player) || ev.DamageHandler.Type.IsStupid() || ev.Player.Role.Type != _target)
            {
                return;
            }

            ev.IsAllowed = false;

            ev.Player.Role.Set(_role, SpawnReason.Revived, RoleSpawnFlags.None);

            if (!_role.IsHuman() && !ev.Player.IsInventoryEmpty)
            {
                ev.Player.DropAllWithoutKeycard();
            }

            IsEnabled = false;
        }
    }
}
