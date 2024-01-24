using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using Toji.Classes.API.Extensions;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Abilities;
using Toji.Classes.Subclasses.Characteristics;
using Toji.Global;
using UnityEngine;

namespace Toji.Classes.Subclasses.Abilities.Active
{
    public class ClothesAbility(uint cooldown) : CooldownAbility(cooldown)
    {
        private Dictionary<Player, BaseSubclass> _copiedSubclasses = new(50);
        private HashSet<RoleTypeId> _ignoredRoles;
        private bool _fullCopy;

        public ClothesAbility(uint cooldown, bool fullCopy) : this(cooldown)
        {
            _ignoredRoles = [

                RoleTypeId.Overwatch,
                RoleTypeId.Filmmaker,
                RoleTypeId.Scp3114,
                RoleTypeId.Spectator,
                RoleTypeId.CustomRole,
                RoleTypeId.Scp079,
                RoleTypeId.Scp0492,
            ];

            _fullCopy = fullCopy;
        }

        public ClothesAbility(uint cooldown, bool fullCopy, IEnumerable<RoleTypeId> roles) : this(cooldown, fullCopy)
        {
            _ignoredRoles = roles.ToHashSet();
        }

        public override string Name => "Перевоплощение";

        public override string Desc => $"Ты можешь скопировать внешнее строение и поведение мертвого существа{(_fullCopy ? ", вместе с его способностями и характеристиками" : string.Empty)}";

        public override void OnEnabled(Player player)
        {
            base.OnEnabled(player);

            player.SendConsoleMessage($"Ты не сможешь перевоплотиться в трупы: {string.Join(", ", _ignoredRoles.Select(role => role.Translate()))}.", "red");
        }

        public override void OnDisabled(Player player)
        {
            base.OnDisabled(player);

            if (_copiedSubclasses.TryGetValue(player, out var previous))
            {
                previous?.Update(player, false);

                _copiedSubclasses.Remove(player);
            }

            if (player.TryGetSubclass(out var subclass))
            {
                foreach (var ability in subclass.Abilities)
                {
                    if (ability == null || ability is ReturnFaceAbility or ClothesAbility or AmogusAbility || subclass.Abilities.Any(a => a.GetType() == ability.GetType()))
                    {
                        continue;
                    }

                    subclass.Abilities.Remove(ability);

                    ability.OnDisabled(player);
                }

                foreach (var characteristic in subclass.Characteristics)
                {
                    if (characteristic == null || characteristic is InventoryCharacteristic or SpawnpointCharacteristic or RespawnCharacteristic)
                    {
                        continue;
                    }

                    characteristic.OnDisabled(player);
                }
            }
        }

        public override bool Activate(Player player, out object result)
        {
            if (!base.Activate(player, out result))
            {
                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (!Ragdoll.List.Any())
            {
                result = "Не удалось найти труп! (Возможно десинхронизация клиент-сервер)";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            var ragdoll = Ragdoll.List.Where(r => r != null && r.Transform != null && !_ignoredRoles.Contains(r.Role)).OrderBy(r => Vector3.Distance(r.Position, player.Position)).FirstOrDefault();

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

                    subclass.CreateInfo(player);
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

                    foreach (var characteristic in subclass.Characteristics)
                    {
                        if (characteristic == null || characteristic is InventoryCharacteristic or SpawnpointCharacteristic or RespawnCharacteristic)
                        {
                            continue;
                        }

                        characteristic.OnEnabled(player);
                    }

                    _copiedSubclasses.Add(player, subclass);
                }
            }

            AddUse(player, DateTime.Now, true, result);

            return true;
        }
    }
}
