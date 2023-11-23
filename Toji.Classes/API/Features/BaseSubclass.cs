using Exiled.API.Features;
using PlayerRoles;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Toji.Classes.API.Extensions;
using Toji.Classes.API.Interfaces;
using Toji.NwPluginAPI.API.Extensions;
using UnityEngine;

namespace Toji.Classes.API.Features
{
    public abstract class BaseSubclass
    {
        private static readonly List<BaseSubclass> _subclasses;
        private bool _subscribed;

        static BaseSubclass()
        {
            _subclasses = new List<BaseSubclass>(100);
        }

        public static IReadOnlyDictionary<RoleTypeId, BaseSubclass> RoleToSubclasses => ReadOnlyCollection.ToDictionary(key => key.Role, value => value);

        public static IReadOnlyCollection<BaseSubclass> ReadOnlyCollection => _subclasses.AsReadOnly();

        public static BaseSubclass Get(Player player) => ReadOnlyCollection.FirstOrDefault(sub => sub.Has(player));

        public static TSubclass Get<TSubclass>(Player player) where TSubclass : BaseSubclass => Get(player) as TSubclass;

        public static bool TryGet(Player player, out BaseSubclass subclass)
        {
            subclass = Get(player);

            return subclass != null;
        }

        public static bool TryGet<TSubclass>(Player player, out TSubclass subclass) where TSubclass : BaseSubclass
        {
            subclass = Get<TSubclass>(player);

            return subclass != null;
        }

        public static bool HasAny(Player player) => ReadOnlyCollection.Any(subclass => subclass.Has(player));

        public abstract string Name { get; }

        public abstract string Desc { get; }

        public abstract RoleTypeId Role { get; }

        public virtual bool ShowInfo { get; } = true;

        public bool IsGroup => this.IsGroupSubclass();

        public bool IsSingle => this.IsSingleSubclass();

        public virtual bool Has(Player player)
        {
            if (player == null)
            {
                return false;
            }

            if (this is IGroup group)
            {
                return group.Players.Contains(player);
            }

            if (this is ISingle single)
            {
                return single.Player.UserId == player.UserId;
            }

            return false;
        }

        public virtual bool Can(Player player) => player != null && CheckType() && CheckLimited(player) && CheckRandom() && CheckDate();

        public virtual void Assign(Player player)
        {
            if (TryGet(player, out var subclass))
            {
                return;
            }

            player.SendConsoleMessage(BuildConsoleMessage(), "yellow");

            if (ShowInfo)
            {
                CreateInfo(player);
            }

            if (this is ISizeSubclass size)
            {
                player.Scale = size.Size;
            }

            if (this is IHasInventory inventory)
            {
                foreach (var slot in inventory.Slots)
                {
                    var item = slot.GetRandomItem();

                    if (item == ItemType.None)
                    {
                        continue;
                    }

                    player.AddItem(item);
                }
            }

            if (this is ICustomHealthSubclass health)
            {
                player.MaxHealth = health.Health;
                player.Health = health.Health;
            }

            if (this is ICassieSubclass cassie)
            {
                Cassie.MessageTranslated(cassie.SpawnAnnouncement, cassie.SpawnSubtitles);
            }

            if (this is IBroadcastSubclass broadcast)
            {
                Map.Broadcast(10, broadcast.WarningText);
            }

            if (this is IHintSubclass hint)
            {
                player.ShowHint(hint.HintText, hint.HintDuration);
            }

            if (!_subscribed)
            {
                Subscribe();
            }
        }

        public virtual void Revoke(Player player)
        {
            if (!Has(player))
            {
                return;
            }

            if (ShowInfo)
            {
                DestroyInfo(player);
            }

            if (this is ISizeSubclass)
            {
                player.Scale = Vector3.one;
            }

            if (this is ICassieSubclass cassie)
            {
                Cassie.MessageTranslated(cassie.DeathAnnouncement, cassie.DeathSubtitles);
            }

            if (this is IBroadcastSubclass broadcast)
            {
                Map.Broadcast(10, broadcast.DeathText);
            }

            if (_subscribed)
            {
                if (this is ISingle single)
                {
                    single.Player = null;
                }
                else if (this is IGroup group)
                {
                    group.Players.Remove(player);

                    if (group.Players.Count != 0)
                    {
                        return;
                    }
                }

                _subscribed = false;

                Unsubscribe();
            }
        }

        public virtual void Subscribe()
        {

        }

        public virtual void Unsubscribe()
        {

        }

        protected void CreateInfo(Player ply)
        {
            ply.CustomInfo = $"{ply.CustomName}{(string.IsNullOrEmpty(ply.CustomInfo) ? string.Empty : $"\n{ply.CustomInfo}")}\n{GetRoleInfo(ply)}";
            ply.InfoArea &= ~(PlayerInfoArea.Role | PlayerInfoArea.Nickname);
        }

        protected void DestroyInfo(Player ply)
        {
            ply.CustomInfo = ply.CustomInfo.Replace(ply.CustomName, string.Empty).Replace("\n", string.Empty).Replace(GetRoleInfo(ply), string.Empty);
            ply.InfoArea |= PlayerInfoArea.Role | PlayerInfoArea.Nickname;
        }

        protected string GetRoleInfo(Player ply) => ply.IsScp ? $"{ply.Role.Type.Translate()} - {Name}" : Name;

        private string BuildConsoleMessage()
        {
            StringBuilder builder = new($"Ты получил подкласс!\n\t\tНазвание: {Name}!\n\t\tОписание: {Desc}.");

            if (IsGroup)
            {
                if (this is ILimitableGroup limit)
                {
                    builder.Append($"\n\t\tТип: Групповой с лимитом {limit.Max}.");
                }
                else
                {
                    builder.Append("\n\t\tТип: Групповой, безлимитный.");
                }
            }
            else if (IsSingle)
            {
                builder.Append("\n\t\tТип: Одиночный.");
            }

            if (this is IRandomSubclass random)
            {
                builder.Append($"\n\t\tШанс: {random.Chance}.");
            }

            if (this is ILimitedSubclass)
            {
                builder.Append("\n\t\tДоступен только определенным игрокам.");
            }

            if (this is IMonthSubclass month)
            {
                var name = DateTimeFormatInfo.CurrentInfo.GetMonthName(month.Month).ToLower();

                name = name.Substring(0, name.Length - 1);

                builder.Append($"\n\t\tДоступный только в {name}е.");
            }

            if (this is ISeasonSubclass season)
            {
                var min = DateTimeFormatInfo.CurrentInfo.GetMonthName(season.Months.Min).ToLower();
                var max = DateTimeFormatInfo.CurrentInfo.GetMonthName(season.Months.Max).ToLower();

                min = min.Substring(0, min.Length - 1);

                builder.Append($"\n\t\tДоступный только с {min}я по {max}.");
            }

            if (this is ICommandsSubclass commands)
            {
                builder.Append("\n\t\tКоманды:");

                foreach (var command in commands.Commands)
                {
                    builder.Append($"\n\t\t\t{command.Key}: {command.Value}.");
                }
            }

            builder.Append("\n\t\t");

            return builder.ToString();
        }

        private bool CheckType()
        {
            if (IsGroup)
            {
                if (this is ILimitableGroup group)
                {
                    return group.Players.Count < group.Max;
                }

                return true;
            }

            if (this is ISingle single)
            {
                return single.Player == null;
            }

            return false;
        }

        private bool CheckLimited(Player player)
        {
            if (this is ILimitedSubclass limit && !limit.Groups.Contains(player.GroupName) && !limit.Users.Contains(player.UserId))
            {
                return false;
            }

            return true;
        }

        private bool CheckRandom()
        {
            if (this is IRandomSubclass random && Random.Range(0, 101) < random.Chance)
            {
                return false;
            }

            return true;
        }

        private bool CheckDate()
        {
            var date = System.DateTime.Now;

            if (this is IMonthSubclass month && date.Month != month.Month)
            {
                return false;
            }

            if (this is ISeasonSubclass season && !season.Months.InRange(date.Month))
            {
                return false;
            }

            return true;
        }
    }
}
