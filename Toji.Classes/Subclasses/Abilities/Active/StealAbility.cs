using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using Toji.Classes.API.Features.Abilities;
using UnityEngine;

namespace Toji.Classes.Subclasses.Abilities.Active
{
    public class StealAbility : CooldownAbility
    {
        private const string StealFailed = "<line-height=95%><size=95%><voffset=-20em><color=#BC5D58>Вы услышали как что-то шуршит в ваших карманах... {0} выглядит подозрительным...</color></size></voffset>";

        private HashSet<ItemType> _banned;
        private float _distance;
        private int _detected;
        private int _failure;
        private bool _held;

        public StealAbility(uint cooldown, bool held, int failure, int detected, float distance) : base(cooldown)
        {
            _banned = new HashSet<ItemType>(13)
            {
                 ItemType.ParticleDisruptor,
                 ItemType.MicroHID,
                 ItemType.GunLogicer,
                 ItemType.ArmorCombat,
                 ItemType.ArmorHeavy,
                 ItemType.ArmorLight,
                 ItemType.GunShotgun,
                 ItemType.GunCom45,
                 ItemType.GunFSP9,
                 ItemType.GunE11SR,
                 ItemType.GunCrossvec,
                 ItemType.SCP244a,
                 ItemType.SCP244b,
            };
            _detected = detected;
            _distance = distance;
            _failure = failure;
            _held = held;
        }

        public StealAbility(uint cooldown, bool held, int failure, int detected, float distance, IEnumerable<ItemType> items) : this(cooldown, held, failure, detected, distance)
        {
            _banned = items.ToHashSet();
        }

        public override string Name => "Воровство";

        public override string Desc => $"Ты можешь попытаться {(_held ? "отобрать" : "украсть")} случайный предмет у игрока, на которого смотришь";

        public override void OnEnabled(Player player)
        {
            base.OnEnabled(player);

            player.SendConsoleMessage($"Ты не сможешь {(_held ? "отобрать" : "украсть")} предметы: {string.Join(", ", _banned.Select(x => x.ToString()))}", "red");
        }

        public override bool Activate(Player player, out object result)
        {
            if (!base.Activate(player, out result))
            {
                AddUse(player, System.DateTime.Now, false, result);

                return false;
            }

            if (player.IsCuffed)
            {
                result = $"Ты не можешь {(_held ? "отбирать" : "воровать")} предметы, будучи связанным.";

                AddUse(player, System.DateTime.Now, false, result);

                return false;
            }

            var target = player.GetFromView(_distance);

            if (target == null || target.IsHost || target.IsNPC || target.IsTutorial)
            {
                result = "Не удалось получить цель!";

                AddUse(player, System.DateTime.Now, false, result);

                return false;
            }

            if (target.IsInventoryEmpty)
            {
                result = "Тебя встретили только пустые карманы!";

                AddUse(player, System.DateTime.Now, false, result);

                return false;
            }

            if (target.Role.Side == player.Role.Side && target.Role.Team != Team.ClassD && player.Role.Team != Team.ClassD)
            {
                result = "Ты вступил в эту организацию и не можешь воровать у союзников!";

                AddUse(player, System.DateTime.Now, false, result);

                return false;
            }

            IEnumerable<ItemType> items = target.Items.Where(item => item != null && (_held || item != target.CurrentItem) && !_banned.Contains(item.Type)).Select(x => x.Type).Distinct();

            if (items.Count() == 0)
            {
                result = "Не найдено подходящих для воровства предметов!";

                AddUse(player, System.DateTime.Now, false, result);

                return false;
            }

            AddUse(player, System.DateTime.Now, true, result);

            ItemType targetItem = items.ElementAt(Random.Range(0, items.Count()));

            if (Random.Range(0, 100) <= _failure)
            {
                result = $"Тебе не удалось украсть {targetItem}...";

                target.ShowHint(string.Format(StealFailed, player.CustomName), 6);

                return false;
            }

            if (player.IsInventoryFull)
            {
                Pickup.CreateAndSpawn(targetItem, player.Position, player.Rotation, target);
            }
            else
            {
                player.AddItem(targetItem);
            }

            target.RemoveItem(target.Items.First(item => item.Type == targetItem));

            if (Random.Range(0, 100) <= _detected)
            {
                target.ShowHint(string.Format(StealFailed, player.CustomName), 6);

                result = $"Ты успешно украл {targetItem}, но кажется он что-то понял...";
            }
            else
            {
                result = $"Ты успешно украл {targetItem}!";
            }

            return true;
        }
    }
}
