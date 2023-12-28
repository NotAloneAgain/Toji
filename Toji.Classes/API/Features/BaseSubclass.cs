using Exiled.API.Extensions;
using Exiled.API.Features;
using PlayerRoles;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Toji.Classes.API.Enums;
using Toji.Classes.API.Extensions;
using Toji.Classes.API.Features.Relations;
using Toji.Classes.API.Features.SpawnRules;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Characteristics;
using Toji.Global;
using UnityEngine;

namespace Toji.Classes.API.Features
{
    public abstract class BaseSubclass : System.IDisposable
    {
        private static readonly SortedSet<BaseSubclass> _subclasses;
        private static readonly HashSet<string> _players;
        private bool _subscribed;

        static BaseSubclass()
        {
            _subclasses = new SortedSet<BaseSubclass>(new PriorityComparer());
            _players = new(Server.MaxPlayerCount);
        }

        public BaseSubclass()
        {
            _subclasses.Add(this);
        }

        public static IReadOnlyCollection<BaseSubclass> ReadOnlyCollection => _subclasses;

        public static BaseSubclass GetInstance(System.Type type) => ReadOnlyCollection.FirstOrDefault(sub => sub.GetType() == type);

        public static TSubclass GetInstance<TSubclass>(System.Type type) where TSubclass : BaseSubclass => GetInstance(type) as TSubclass;

        public static BaseSubclass Get(Player player) => player == null || !Contains(player) ? null : ReadOnlyCollection.FirstOrDefault(sub => sub.Has(player));

        public static BaseSubclass Get(Ragdoll ragdoll) => ragdoll == null ? null : ReadOnlyCollection.FirstOrDefault(sub => sub.Owners.Contains(ragdoll));

        public static BaseSubclass Get(string str) => string.IsNullOrEmpty(str) ? null : ReadOnlyCollection.FirstOrDefault(sub => sub.Name.Equals(str, System.StringComparison.OrdinalIgnoreCase));

        public static TSubclass Get<TSubclass>(in Player player) where TSubclass : BaseSubclass => player == null ? null : Get(player) as TSubclass;

        public static TSubclass Get<TSubclass>(Ragdoll ragdoll) where TSubclass : BaseSubclass => ragdoll == null ? null : Get(ragdoll) as TSubclass;

        public static TSubclass Get<TSubclass>(in string str) where TSubclass : BaseSubclass => string.IsNullOrEmpty(str) ? null : Get(str) as TSubclass;

        public static IEnumerable<BaseSubclass> Get(RoleTypeId role) => ReadOnlyCollection.Where(sub => sub.SpawnRules.Model == role || sub.SpawnRules is TeamSpawnRules team && team.Check(role));

        public static IEnumerable<TSubclass> Get<TSubclass>(RoleTypeId role) where TSubclass : BaseSubclass => Get(role).Select(sub => sub as TSubclass);

        public static bool TryGet(in Player player, out BaseSubclass subclass)
        {
            subclass = Get(player);

            return subclass != null;
        }

        public static bool TryGet(in Ragdoll ragdoll, out BaseSubclass subclass)
        {
            subclass = Get(ragdoll);

            return subclass != null;
        }

        public static bool TryGet(in string str, out BaseSubclass subclass)
        {
            subclass = Get(str);

            return subclass != null;
        }

        public static bool TryGet<TSubclass>(in Player player, out TSubclass subclass) where TSubclass : BaseSubclass
        {
            subclass = Get<TSubclass>(player);

            return subclass != null;
        }

        public static bool TryGet<TSubclass>(in Ragdoll ragdoll, out TSubclass subclass) where TSubclass : BaseSubclass
        {
            subclass = Get<TSubclass>(ragdoll);

            return subclass != null;
        }

        public static bool TryGet<TSubclass>(in string str, out TSubclass subclass) where TSubclass : BaseSubclass
        {
            subclass = Get<TSubclass>(str);

            return subclass != null;
        }

        public static bool TryGet(RoleTypeId role, out IEnumerable<BaseSubclass> subclasses)
        {
            subclasses = Get(role);

            return subclasses != null && subclasses.Any(sub => sub != null);
        }

        public static bool TryGet<TSubclass>(RoleTypeId role, out IEnumerable<TSubclass> subclasses) where TSubclass : BaseSubclass
        {
            subclasses = Get<TSubclass>(role);

            return subclasses != null && subclasses.Any();
        }

        public static bool Contains(Player player) => _players.Contains(player.UserId);

        public static void Clear() => _players.Clear();

        public bool IsGroup => this.IsGroupSubclass();

        public bool IsSingle => this.IsSingleSubclass();

        public string ConsoleMessage => BuildConsoleMessage(null);

        public HashSet<Ragdoll> Owners { get; } = new HashSet<Ragdoll>(100);

        public abstract string Name { get; }

        public abstract string Desc { get; }

        public abstract BaseSpawnRules SpawnRules { get; }

        public virtual List<string> Tags { get; } = new List<string>(0);

        public virtual List<BaseAbility> Abilities { get; } = new List<BaseAbility>(0);

        public virtual List<BaseRelation> Relations { get; } = new List<BaseRelation>(0);

        public virtual List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(0);

        public virtual bool ShowInfo { get; } = true;

        public virtual bool Has(in Player player) => player != null && !player.IsHost && Contains(player);

        public virtual bool Can(in Player player) => player != null && !player.IsHost && !Contains(player) && CheckLimited(player) && CheckRelations() && CheckRandom() && CheckDate();

        public virtual bool Add(Player player) => _players.Add(player.UserId);

        public virtual bool Remove(Player player) => _players.Remove(player.UserId);

        public bool DelayedAssign(in Player player, float delay = 0.0005f)
        {
            Add(player);

            System.Delegate action = Assign;

            return action.CallDelayedWithResult<bool>(delay, player);
        }

        public virtual bool Assign(in Player player)
        {
            if (TryGet(player, out var sub) && sub != this || !player.IsAlive)
            {
                Remove(player);

                return false;
            }

            Add(player);

            player.SendConsoleMessage(BuildConsoleMessage(player), "yellow");

            ShowHint(player);

            Update(player, true);

            if (!_subscribed)
            {
                LazySubscribe();
            }

            return true;
        }

        public virtual bool Revoke(in Player player)
        {
            if (!Contains(player) || TryGet(player, out var sub) && sub != this)
            {
                return false;
            }

            Remove(player);

            Update(player, false);

            return true;
        }

        public void Update(in Player player, bool isAdded, bool withAbility = true)
        {
            if (isAdded)
            {
                OnAdded(player, withAbility);
            }
            else
            {
                OnRemoved(player);
            }
        }

        public void ShowHint(Player player)
        {
            if (this is IHintSubclass)
            {
                ICustomHintSubclass customHint = this as ICustomHintSubclass;

                string color = string.IsNullOrEmpty(customHint?.HintColor) ? SpawnRules.Model.GetColor().ToHex() : customHint.HintColor;
                string text = string.IsNullOrEmpty(customHint?.HintText) ? $"<line-height=95%><size=95%><voffset=-18em><color={color}>Ты - {Name}!\n{Desc}.</color></size></voffset>" : customHint.HintText;

                player.ShowHint(text, 10);
            }
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

            _subclasses.Remove(this);
        }

        protected internal virtual void OnEscaped(in Player player)
        {
            if (ShowInfo)
            {
                CreateInfo(player);
            }

            if (Characteristics.Any())
            {
                foreach (var characteristic in Characteristics)
                {
                    if (characteristic is InventoryCharacteristic or SpawnpointCharacteristic)
                    {
                        continue;
                    }

                    characteristic.OnEnabled(player);
                }
            }
        }

        protected internal virtual void LazySubscribe()
        {
            _subscribed = true;
        }

        protected internal virtual void LazyUnsubscribe()
        {
            _subscribed = false;
        }

        protected virtual void OnAdded(in Player player, bool withAbility)
        {
            if (ShowInfo)
            {
                CreateInfo(player);
            }

            if (Abilities.Any() && withAbility)
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

        protected virtual void OnRemoved(in Player player)
        {
            if (ShowInfo)
            {
                DestroyInfo(player);
            }

            if (Abilities.Any())
            {
                foreach (var ability in Abilities)
                {
                    ability.OnDisabled(player);
                }
            }

            if (Characteristics.Any())
            {
                foreach (var characteristic in Characteristics)
                {
                    characteristic.OnDisabled(player);
                }
            }
        }

        internal protected void CreateInfo(in Player ply)
        {
            ply.CustomInfo = $"{ply.CustomName}{(string.IsNullOrEmpty(ply.CustomInfo) ? string.Empty : $"\n{ply.CustomInfo}")}\n{GetRoleInfo(ply)}";
            ply.InfoArea &= ~(PlayerInfoArea.Role | PlayerInfoArea.Nickname);
        }

        internal protected void DestroyInfo(in Player ply)
        {
            ply.CustomInfo = ply.CustomInfo.Replace(ply.CustomName, string.Empty).Replace("\n", string.Empty).Replace(GetRoleInfo(ply), string.Empty);
            ply.InfoArea |= PlayerInfoArea.Role | PlayerInfoArea.Nickname;
        }

        protected string GetRoleInfo(in Player ply)
        {
            if (this is IRoleInfo info)
            {
                return info.RoleInfo;
            }

            return ply.Role.Team switch
            {
                Team.SCPs => $"{ply.Role.Type.Translate()} - {Name}",
                Team.FoundationForces => ply.Role.Type == RoleTypeId.FacilityGuard ? $"Охранник Комплекса - {Name}" : $"Девятихвостая Лиса - {Name}",
                Team.ChaosInsurgency => $"Повстанец Хаоса - {Name}",
                _ => Name,
            };
        }

        private string BuildConsoleMessage(in Player player)
        {
            StringBuilder builder = new($"{(player == null ? string.Empty : "Ты получил подкласс:")}\n\t\tНазвание: {Name}.\n\t\tОписание: {Desc}.");

            if (Tags.Any())
            {
                builder.Append($"\n\t\tТеги: {string.Join(", ", Tags)}.");
            }

            if (this is ILimitableGroup limit)
            {
                builder.Append($"\n\t\tТип: Групповой с лимитом {limit.Max}.");
            }
            else if (IsGroup)
            {
                builder.Append("\n\t\tТип: Групповой, безлимитный.");
            }
            else if (IsSingle)
            {
                builder.Append("\n\t\tТип: Одиночный.");
            }
            else
            {
                builder.Append("\n\t\tТип: Базовый.");
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

            if (Characteristics.Any())
            {
                builder.Append("\n\t\tХарактеристики:");

                foreach (var characteristic in Characteristics)
                {
                    builder.Append($"\n\t\t\t{characteristic.Name}: {characteristic.GetDesc(player)}.");
                }
            }

            if (Relations.Any())
            {
                builder.Append("\n\t\tОтношения:");

                foreach (var relation in Relations)
                {
                    builder.Append($"\n\t\t\t{(relation.Type == RelationType.Required ? "Требуется" : "Не должно быть")}: {relation.Desc}.");
                }
            }

            if (Abilities.Any())
            {
                builder.Append("\n\t\tСпособности:");

                foreach (var ability in Abilities)
                {
                    builder.Append($"\n\t\t\t{ability.Type} | {ability.Name}: {ability.Desc}.");
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

        private bool CheckRelations()
        {
            if (!Relations.Any())
            {
                return true;
            }

            foreach (var relation in Relations)
            {
                if (relation.Check())
                {
                    continue;
                }

                return false;
            }

            return true;
        }

        private bool CheckRandom()
        {
            if (this is IRandomSubclass random && Random.Range(0, 100) > random.Chance)
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
