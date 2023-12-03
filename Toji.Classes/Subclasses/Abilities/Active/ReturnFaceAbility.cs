using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System;
using System.Collections.Generic;
using Toji.Classes.API.Features.Abilities;
using Toji.Global;

namespace Toji.Classes.Subclasses.Abilities.Active
{
    public class ReturnFaceAbility : CooldownAbility
    {
        private Dictionary<Player, RoleTypeId> _previousRoles;

        public ReturnFaceAbility(uint cooldown) : base(cooldown)
        {
            _previousRoles = new Dictionary<Player, RoleTypeId>(50);
        }

        public override string Name => "Возвращение облика";

        public override string Desc => "Ты можешь вернуть свой предыдущий облик";

        public override void Subscribe()
        {
            base.Subscribe();

            Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;
            Exiled.Events.Handlers.Player.Died += OnDied;
        }

        public override void Unsubscribe()
        {
            Exiled.Events.Handlers.Player.Died -= OnDied;
            Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;

            base.Unsubscribe();
        }

        public override bool Activate(Player player, out object result)
        {
            if (!base.Activate(player, out result))
            {
                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            var role = GetPreviousRole(player);

            if (role == RoleTypeId.None)
            {
                result = "Не удалось найти предыдущий облик!";

                return false;
            }

            _previousRoles.SetOrAdd(player, player.Role.Type);

            player.Role.Set(role, SpawnReason.Revived, RoleSpawnFlags.None);

            player.DisableAllEffects();

            AddUse(player, DateTime.Now, true, result);

            return true;
        }

        private RoleTypeId GetPreviousRole(Player player) => player == null ? RoleTypeId.None : _previousRoles.TryGetValue(player, out var value) ? value : RoleTypeId.None;

        private void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (!Has(ev.Player) || ev.SpawnFlags != RoleSpawnFlags.None || ev.Reason != SpawnReason.Revived || ev.NewRole == GetPreviousRole(ev.Player))
            {
                return;
            }

            _previousRoles.SetOrAdd(ev.Player, ev.Player.Role.Type);
        }

        private void OnDied(DiedEventArgs ev)
        {
            if (!Has(ev.Player))
            {
                return;
            }

            _previousRoles.SetOrAdd(ev.Player, RoleTypeId.None);
        }
    }
}
