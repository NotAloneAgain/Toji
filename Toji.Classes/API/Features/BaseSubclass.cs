using Exiled.API.Extensions;
using Exiled.API.Features;
using PlayerRoles;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Toji.Classes.API.Extensions;
using Toji.Classes.API.Interfaces;
using Toji.Global;
using UnityEngine;

namespace Toji.Classes.API.Features
{
    public abstract class BaseSubclass : System.IDisposable
    {
        private static readonly Dictionary<RoleTypeId, List<BaseSubclass>> _roleToSubclasses;
        private static readonly SortedSet<BaseSubclass> _subclasses;
        private bool _subscribed;

        static BaseSubclass()
        {
            _subclasses = new SortedSet<BaseSubclass>(new PriorityComparer());
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

        public static IReadOnlyCollection<BaseSubclass> ReadOnlyCollection => _subclasses;

        public static BaseSubclass Get(Player player) => player == null ? null : ReadOnlyCollection.FirstOrDefault(sub => sub.Has(player));

        public static BaseSubclass Get(Ragdoll ragdoll) => ragdoll == null ? null : ReadOnlyCollection.FirstOrDefault(sub => sub.Owners.Contains(ragdoll));

        public static BaseSubclass Get(string str) => string.IsNullOrEmpty(str) ? null : ReadOnlyCollection.FirstOrDefault(sub => sub.Name.Equals(str, System.StringComparison.OrdinalIgnoreCase));

        public static TSubclass Get<TSubclass>(in Player player) where TSubclass : BaseSubclass => player == null ? null : Get(player) as TSubclass;

        public static TSubclass Get<TSubclass>(Ragdoll ragdoll) where TSubclass : BaseSubclass => ragdoll == null ? null : Get(ragdoll) as TSubclass;

        public static TSubclass Get<TSubclass>(in string str) where TSubclass : BaseSubclass => string.IsNullOrEmpty(str) ? null : Get(str) as TSubclass;

        public static IEnumerable<BaseSubclass> Get(RoleTypeId role) => RoleToSubclasses.ContainsKey(role) ? RoleToSubclasses[role] : null;

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

        public string ConsoleMessage => BuildConsoleMessage(null);

        public HashSet<Ragdoll> Owners { get; } = new HashSet<Ragdoll>(100);

        public abstract string Name { get; }

        public abstract string Desc { get; }

        public abstract RoleTypeId Role { get; }

        public virtual List<string> Tags { get; } = new List<string>(0);

        public virtual List<BaseAbility> Abilities { get; } = new List<BaseAbility>(0);

        public virtual List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(0);

        public virtual bool ShowInfo { get; } = true;

        public virtual bool Has(in Player player) => player != null && !player.IsHost && player.IsAlive;

        public virtual bool Can(in Player player) => player != null && !player.IsHost && player.IsAlive && !HasAny(player) && CheckLimited(player) && CheckNeeds() && CheckRandom() && CheckDate();

        public bool DelayedAssign(in Player player, float delay = 0.0005f)
        {
            System.Delegate action = Assign;

            return action.CallDelayedWithResult<bool>(delay, player);
        }

        public virtual bool Assign(in Player player)
        {
            if (TryGet(player, out _))
            {
                return false;
            }

            player.SendConsoleMessage(BuildConsoleMessage(player), "yellow");

            SendSpawnCassie();

            SendWarningBroadcast();

            Teleport(player);

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
            if (!Has(player))
            {
                return false;
            }

            SendDeathCassie();

            SendDeathBroadcast();

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

        public virtual void LazySubscribe()
        {
            _subscribed = true;
        }

        public virtual void LazyUnsubscribe()
        {
            _subscribed = false;
        }

        public void SendSpawnCassie()
        {
            if (this is ICassieSubclass cassie && !string.IsNullOrEmpty(cassie.SpawnAnnouncement))
            {
                Cassie.MessageTranslated(cassie.SpawnAnnouncement, cassie.SpawnSubtitles);
            }
        }

        public void SendDeathCassie()
        {
            if (this is ICassieSubclass cassie && !string.IsNullOrEmpty(cassie.DeathAnnouncement))
            {
                Cassie.MessageTranslated(cassie.DeathAnnouncement, cassie.DeathSubtitles);
            }
        }

        public void SendWarningBroadcast()
        {
            if (this is IBroadcastSubclass broadcast && !string.IsNullOrEmpty(broadcast.WarningText))
            {
                Map.Broadcast(10, broadcast.WarningText);
            }
        }

        public void SendDeathBroadcast()
        {
            if (this is IBroadcastSubclass broadcast && !string.IsNullOrEmpty(broadcast.DeathText))
            {
                Map.Broadcast(10, broadcast.DeathText);
            }
        }

        public void Teleport(in Player player)
        {
            if (this is ISpawnpointSubclass spawnpoint)
            {
                player.Position = spawnpoint.Spawnpoint.Position;
            }
        }

        public void ShowHint(Player player)
        {
            if (this is IHintSubclass)
            {
                ICustomHintSubclass customHint = this as ICustomHintSubclass;

                string color = string.IsNullOrEmpty(customHint?.HintColor) ? Role.GetColor().ToHex() : customHint.HintColor;
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

            _roleToSubclasses[Role].Remove(this);

            _subclasses.Remove(this);
        }

        internal protected void OnEscaped(in Player player) => Update(player, true);

        protected virtual void OnAdded(in Player player, bool withAbility)
        {
            if (ShowInfo)
            {
                CreateInfo(player);
            }

            if (Abilities.Any() && withAbility)
            {
                player.SendConsoleMessage("Активация некоторых способностей происходит с помощью команды .ability", "green");
                player.SendConsoleMessage("Если активируемых способностей много пиши .ability [Номер]", "green");
                player.SendConsoleMessage("Помни что отсчет способностей начинается с 0.", "green");

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

            if (Abilities.Any())
            {
                builder.Append("\n\t\tСпособности:");

                foreach (var ability in Abilities)
                {
                    builder.Append($"\n\t\t\t{ability.Type} | {ability.Name}: {ability.Desc}.");
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
