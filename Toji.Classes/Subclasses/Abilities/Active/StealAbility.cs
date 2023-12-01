using Exiled.API.Features;
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

        private bool _held;
        private int _failure;
        private int _detected;
        private float _distance;
        private HashSet<ItemType> _banned;

        public StealAbility(uint cooldown, bool held, int failure, int detected, float distance) : base(cooldown)
        {
            _held = held;
            _failure = failure;
            _detected = detected;
            _distance = distance;
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
        }

        public StealAbility(uint cooldown, bool held, int failure, int detected, float distance, params ItemType[] items) : this(cooldown, held, failure, detected, distance)
        {
            _banned = items.ToHashSet();
        }

        public StealAbility(uint cooldown, bool held, int failure, int detected, float distance, IEnumerable<ItemType> items) : this(cooldown, held, failure, detected, distance, items.ToArray()) { }

        public override string Name => "Воровство";

        public override string Desc => $"Ты можешь попытаться {(_held ? "отобрать" : "украсть")} случайный предмет у игрока, на которого смотрите.";

        public override void OnEnabled(Player player)
        {
            base.OnEnabled(player);

            player.SendConsoleMessage($"Вы не сможете украсть: {string.Join(", ", _banned.Select(x => x.ToString()))}", "red");
        }

        public override bool Activate(Player player, out object result)
        {
            result = null!;

            var target = player.GetFromView(_distance);

            if (target == null || target.IsHost || target.IsNPC || target.IsInventoryEmpty || target.IsTutorial)
            {
                return false;
            }

            if (target.Role.Side == player.Role.Side && target.Role.Team != Team.ClassD && player.Role.Team != Team.ClassD)
            {
                return false;
            }

            IEnumerable<ItemType> items = target.Items.Where(item => item != null && (_held || item != target.CurrentItem) && !_banned.Contains(item.Type)).Select(x => x.Type).Distinct();

            if (items.Count() == 0)
            {
                return false;
            }

            ItemType targetItem = items.ElementAt(Random.Range(0, items.Count()));

            if (Random.Range(0, 100) <= _failure)
            {
                target.ShowHint(string.Format(StealFailed, player.CustomName), 6);

                return false;
            }

            if (!base.Activate(player, out result))
            {
                return false;
            }

            player.AddItem(targetItem);

            target.RemoveItem(target.Items.First(item => item.Type == targetItem));

            if (Random.Range(0, 100) <= _detected)
            {
                target.ShowHint(string.Format(StealFailed, player.CustomName), 6);
            }

            return true;
        }
    }
}
