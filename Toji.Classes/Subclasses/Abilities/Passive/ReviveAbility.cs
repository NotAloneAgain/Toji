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
        private RoleTypeId _role;

        public ReviveAbility(RoleTypeId role) => _role = role;

        public override string Name => "Возрождение";

        public override string Desc => $"После смерти ты станешь {_role.Translate()}";

        public override void Subscribe() => Player.Dying += OnDying;

        public override void Unsubscribe() => Player.Dying -= OnDying;

        private void OnDying(DyingEventArgs ev)
        {
            if (!ev.IsValid() || !ev.IsAllowed || !Has(ev.Player) || ev.DamageHandler.Type == DamageType.Warhead)
            {
                return;
            }

            ev.IsAllowed = false;

            ev.Player.Role.Set(_role, SpawnReason.Revived, RoleSpawnFlags.None);

            if (!_role.IsHuman())
            {
                ev.Player.DropAllWithoutKeycard();
            }

            IsEnabled = false;
        }
    }
}
