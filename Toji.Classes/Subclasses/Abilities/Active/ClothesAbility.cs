using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using Toji.Classes.API.Extensions;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Abilities;
using Toji.Global;
using UnityEngine;

namespace Toji.Classes.Subclasses.Abilities.Active
{
    public class ClothesAbility : CooldownAbility
    {
        private Dictionary<Player, BaseSubclass> _copiedSubclasses;
        private HashSet<RoleTypeId> _ignoredRoles;
        private bool _fullCopy;

        public ClothesAbility(uint cooldown) : base(cooldown)
        {
            _copiedSubclasses = new(50);
        }

        public ClothesAbility(uint cooldown, bool fullCopy) : this(cooldown)
        {
            _ignoredRoles = new HashSet<RoleTypeId>()
            {
                RoleTypeId.Overwatch,
                RoleTypeId.Filmmaker,
                RoleTypeId.Scp3114,
                RoleTypeId.Spectator,
                RoleTypeId.CustomRole,
                RoleTypeId.Scp079,
                RoleTypeId.Scp0492,
            };

            _fullCopy = fullCopy;
        }

        public ClothesAbility(uint cooldown, bool fullCopy, IEnumerable<RoleTypeId> roles) : this(cooldown, fullCopy)
        {
            _ignoredRoles = roles.ToHashSet();
        }

        public override string Name => "Перевоплощение";

        public override string Desc => $"Ты можешь копировать внешнее строение и поведение трупов{(_fullCopy ? ", вместе с их способностями и характеристиками" : string.Empty)}";

        public override void OnEnabled(Player player)
        {
            base.OnEnabled(player);

            player.SendConsoleMessage($"Ты не сможешь перевоплотиться в трупы: {string.Join(", ", _ignoredRoles.Select(role => role.Translate()))}.", "red");
        }

        public override bool Activate(Player player, out object result)
        {
            if (!base.Activate(player, out result))
            {
                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            var ragdoll = Ragdoll.List.Where(r => !_ignoredRoles.Contains(r.Role)).OrderBy(r => Vector3.Distance(r.Position, player.Position)).First();

            if (ragdoll == null || ragdoll.Room != player.CurrentRoom || Vector3.Distance(ragdoll.Position, player.Position) > (player.Zone == ZoneType.Surface ? 3.8f : 5))
            {
                result = "Не удалось найти труп! (Возможно десинхронизация клиент-сервер)";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            player.Role.Set(ragdoll.Role, SpawnReason.Revived, RoleSpawnFlags.None);

            player.DisableAllEffects();

            if (_fullCopy)
            {
                if (_copiedSubclasses.TryGetValue(player, out var previous))
                {
                    previous.Update(player, false);

                    _copiedSubclasses.Remove(player);
                }

                if (BaseSubclass.TryGet(ragdoll, out var subclass))
                {
                    var ownerSubclass = player.GetSubclass();

                    player.SendConsoleMessage($"Ты скопировал: {subclass.ConsoleMessage}", "yellow");

                    subclass.Update(player, true, false);
                    subclass.ShowHint(player);

                    foreach (var ability in subclass.Abilities)
                    {
                        if (ability == null || ability is ReturnFaceAbility or ClothesAbility or AmogusAbility || ownerSubclass.Abilities.Any(a => a.GetType() == ability.GetType()))
                        {
                            continue;
                        }

                        ownerSubclass.Abilities.Add(ability);

                        ability.OnEnabled(player);
                    }

                    _copiedSubclasses.Add(player, subclass);
                }
            }

            AddUse(player, DateTime.Now, true, result);

            return true;
        }
    }
}
