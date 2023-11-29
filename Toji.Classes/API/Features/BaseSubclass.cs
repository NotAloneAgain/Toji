using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Map;
using Hints;
using PlayerRoles;
using PluginAPI.Commands;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Toji.Classes.API.Extensions;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Characteristics;
using Toji.NwPluginAPI.API.Extensions;
using UnityEngine;

namespace Toji.Classes.API.Features
{
    public abstract class BaseSubclass : System.IDisposable
    {
        private static readonly Dictionary<RoleTypeId, List<BaseSubclass>> _roleToSubclasses;
        private static readonly List<BaseSubclass> _subclasses;
        private bool _subscribed;

        static BaseSubclass()
        {
            _subclasses = new List<BaseSubclass>(100);
            _roleToSubclasses = new Dictionary<RoleTypeId, List<BaseSubclass>>(50);
        }

        public BaseSubclass()
        {
            _subclasses.Add(this);

            if (!_roleToSubclasses.ContainsKey(Role))
            {
                _roleToSubclasses.Add(Role, new List<BaseSubclass> { this });

                return;
            }

            _roleToSubclasses[Role].Add(this);
        }

        public static IReadOnlyDictionary<RoleTypeId, IReadOnlyCollection<BaseSubclass>> RoleToSubclasses => _roleToSubclasses.ToDictionary(pair => pair.Key, pair => (IReadOnlyCollection<BaseSubclass>)pair.Value.AsReadOnly());

        public static IReadOnlyCollection<BaseSubclass> ReadOnlyCollection => _subclasses.AsReadOnly();

        public static BaseSubclass Get(Player player) => player == null ? null : ReadOnlyCollection.FirstOrDefault(sub => sub.Has(player));

        public static TSubclass Get<TSubclass>(in Player player) where TSubclass : BaseSubclass => player == null ? null : Get(player) as TSubclass;

        public static IEnumerable<BaseSubclass> Get(RoleTypeId role) => RoleToSubclasses.ContainsKey(role) ? RoleToSubclasses[role] : null;

        public static IEnumerable<TSubclass> Get<TSubclass>(RoleTypeId role) where TSubclass : BaseSubclass => Get(role).Select(sub => sub as TSubclass);

        public static bool TryGet(in Player player, out BaseSubclass subclass)
        {
            subclass = Get(player);

            return subclass != null;
        }

        public static bool TryGet<TSubclass>(in Player player, out TSubclass subclass) where TSubclass : BaseSubclass
        {
            subclass = Get<TSubclass>(player);

            return subclass != null;
        }

        public static bool TryGet(RoleTypeId role, out IEnumerable<BaseSubclass> subclasses)
        {
            subclasses = Get(role);

            return subclasses != null && subclasses.Any();
        }

        public static bool TryGet<TSubclass>(RoleTypeId role, out IEnumerable<TSubclass> subclasses) where TSubclass : BaseSubclass
        {
            subclasses = Get<TSubclass>(role);

            return subclasses != null && subclasses.Any();
        }

        public static bool HasAny(Player player) => ReadOnlyCollection.Any(subclass => subclass.Has(player));

        public bool IsGroup => this.IsGroupSubclass();

        public bool IsSingle => this.IsSingleSubclass();

        public abstract string Name { get; }

        public abstract string Desc { get; }

        public abstract RoleTypeId Role { get; }

        public virtual List<BaseAbility> Abilities { get; } = new List<BaseAbility>(0);

        public virtual List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(0);

        public virtual bool ShowInfo { get; } = true;

        public virtual bool Has(in Player player) => player == null;

        public virtual bool Can(in Player player) => player != null && CheckLimited(player) && CheckNeeds() && CheckRandom() && CheckDate();

        public bool DelayedAssign(in Player player, float delay = 0.0005f)
        {
            System.Delegate action = Assign;

            return action.CallDelayedWithResult<bool>(delay, player);
        }

        public virtual void UpdatePlayer(in Player player)
        {
            if (ShowInfo)
            {
                CreateInfo(player);
            }

            if (Abilities.Any())
            {
                foreach (var ability in Abilities)
                {
                    ability.OnEnabled(player);
                }
            }

            if (Characteristics.Any())
            {
                foreach (var characteristic in Characteristics)
                {
                    characteristic.OnEnabled(player);
                }
            }
        }

        public virtual bool Assign(in Player player)
        {
            if (TryGet(player, out _))
            {
                return false;
            }

            player.SendConsoleMessage(BuildConsoleMessage(player), "yellow");

            UpdatePlayer(player);

            if (this is ICassieSubclass cassie)
            {
                Cassie.MessageTranslated(cassie.SpawnAnnouncement, cassie.SpawnSubtitles);
            }

            if (this is IBroadcastSubclass broadcast)
            {
                Map.Broadcast(10, broadcast.WarningText);
            }

            if (this is IHintSubclass)
            {
                ICustomHintSubclass customHint = this as ICustomHintSubclass;

                string color = string.IsNullOrEmpty(customHint?.HintColor) ? Role.GetColor().ToHex() : customHint.HintColor;
                string text = string.IsNullOrEmpty(customHint?.HintText) ? $"<line-height=95%><size=95%><voffset=-18em><color={color}>Ты - {Name}!\n{Desc}.</color></size></voffset>" : customHint.HintText;

                player.ShowHint(text, 10);
            }

            if (!_subscribed)
            {
                LazySubscribe();
            }

            return true;
        }

        public virtual bool Revoke(in Player player)
        {
            if (!Has(player))
            {
                return false;
            }

            if (ShowInfo)
            {
                DestroyInfo(player);
            }

            if (Abilities.Any())
            {
                foreach (var ability in Abilities)
                {
                    ability.OnEnabled(player);
                }
            }

            if (Characteristics.Any())
            {
                foreach (var characteristic in Characteristics)
                {
                    characteristic.OnDisabled(player);
                }
            }

            if (this is ICassieSubclass cassie)
            {
                Cassie.MessageTranslated(cassie.DeathAnnouncement, cassie.DeathSubtitles);
            }

            if (this is IBroadcastSubclass broadcast)
            {
                Map.Broadcast(10, broadcast.DeathText);
            }

            if (!_subscribed)
            {
                return false;
            }

            _subscribed = false;

            return true;
        }

        public void Dispose()
        {
            foreach (var player in Player.List.Where(ply => Has(ply)))
            {
                Revoke(player);
            }

            if (_subscribed)
            {
                LazyUnsubscribe();
            }

            if (this is ISubscribable sub)
            {
                sub.Unsubscribe();
            }

            _roleToSubclasses[Role].Remove(this);

            _subclasses.Remove(this);
        }

        public virtual void LazySubscribe() { }

        public virtual void LazyUnsubscribe() { }

        internal protected void OnEscaped(in Player player) => UpdatePlayer(player);

        protected void CreateInfo(in Player ply)
        {
            ply.CustomInfo = $"{ply.CustomName}{(string.IsNullOrEmpty(ply.CustomInfo) ? string.Empty : $"\n{ply.CustomInfo}")}\n{GetRoleInfo(ply)}";
            ply.InfoArea &= ~(PlayerInfoArea.Role | PlayerInfoArea.Nickname);
        }

        protected void DestroyInfo(in Player ply)
        {
            ply.CustomInfo = ply.CustomInfo.Replace(ply.CustomName, string.Empty).Replace("\n", string.Empty).Replace(GetRoleInfo(ply), string.Empty);
            ply.InfoArea |= PlayerInfoArea.Role | PlayerInfoArea.Nickname;
        }

        protected string GetRoleInfo(in Player ply) => ply.IsScp ? $"{ply.Role.Type.Translate()} - {Name}" : Name;

        private string BuildConsoleMessage(in Player player)
        {
            StringBuilder builder = new($"Ты получил подкласс:\n\t\tНазвание: {Name}.\n\t\tОписание: {Desc}.");

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
                builder.Append($"\n\t\tШанс: {random.Chance}%.");
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

            if (Abilities.Any())
            {
                builder.Append("\n\t\tСпособности:");

                foreach (var ability in Abilities)
                {
                    builder.Append($"\n\t\t\t{ability.Name}: {ability.Desc}.");
                }
            }

            if (Characteristics.Any())
            {
                builder.Append("\n\t\tХарактеристики:");

                foreach (var characteristic in Characteristics)
                {
                    builder.Append($"\n\t\t\t{characteristic.Name}: {characteristic.GetDesc(player)}.");
                }
            }

            if (this is ICommandsSubclass commands)
            {
                builder.Append("\n\t\tКоманды:");

                foreach (var command in commands.Commands)
                {
                    builder.Append($"\n\t\t\t{command.Key}: {command.Value}.");
                }
            }

            return builder.ToString();
        }

        private bool CheckLimited(in Player player)
        {
            if (this is ILimitedSubclass limit && !limit.Groups.Contains(player.GroupName) && !limit.Users.Contains(player.UserId))
            {
                return false;
            }

            return true;
        }

        private bool CheckNeeds()
        {
            if (this is INeedRole needRole && Player.List.All(ply => ply.Role.Type != needRole.NeedRole))
            {
                return false;
            }

            if (this is INeedSubclass needSubclass)
            {
                var subclass = ReadOnlyCollection.FirstOrDefault(sub => sub.GetType() == needSubclass.Needed);

                if (subclass != null && Player.List.All(ply => !subclass.Has(ply)))
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckRandom()
        {
            if (this is IRandomSubclass random && Random.Range(0, 100) < random.Chance)
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
