using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using Toji.Classes.API.Extensions;
using Toji.Classes.API.Features;
using Toji.ExiledAPI.Extensions;
using Toji.Global;

namespace Toji.LeaveReplacer.Handlers
{
    internal sealed class PlayerHandlers
    {
        private IEnumerable<RoleTypeId> _roles;

        internal PlayerHandlers() => _roles = new List<RoleTypeId>(5)
        {
            RoleTypeId.Spectator,
            RoleTypeId.ClassD,
            RoleTypeId.Scientist,
            RoleTypeId.FacilityGuard,
            RoleTypeId.NtfPrivate,
            RoleTypeId.ChaosRifleman
        };

        public void OnDestroying(DestroyingEventArgs ev)
        {
            if (!ev.IsValid() || !ev.Player.IsScp)
            {
                return;
            }

            var subclass = ev.Player.GetSubclass();
            var player = SelectPlayer();

            if (player == null)
            {
                return;
            }

            player.DropItems();

            Timing.RunCoroutine(_Replace(player, ev.Player, subclass));
        }

        private Queue<RoleTypeId> GetRolesQueue() => QueuePool<RoleTypeId>.Pool.Get(_roles);

        private IEnumerator<float> _Replace(Player player, Player target, BaseSubclass subclass)
        {
            if (target.Role.Type == RoleTypeId.Scp079)
            {
                var role = target.Role.As<Scp079Role>();

                var experience = role.Experience;
                var camera = role.Camera;
                var energy = role.Energy;
                var level = role.Level;

                player.Role.Set(target.Role.Type, SpawnReason.LateJoin, RoleSpawnFlags.AssignInventory);
                target.Role.Set(RoleTypeId.Spectator, SpawnReason.ForceClass, RoleSpawnFlags.All);

                yield return Timing.WaitForSeconds(0.0005f);

                role = player.Role.As<Scp079Role>();

                role.Level = level;
                role.Energy = energy;
                role.Camera = camera;
                role.Experience = experience;

                yield break;
            }

            var position = target.Position;
            var rotation = target.Rotation;
            var health = target.Health;

            player.Role.Set(target.Role.Type, SpawnReason.LateJoin, RoleSpawnFlags.AssignInventory);
            target.Role.Set(RoleTypeId.Spectator, SpawnReason.ForceClass, RoleSpawnFlags.All);

            if (subclass != null)
            {
                if (subclass.Has(player))
                {
                    subclass.Revoke(player);
                }

                subclass.DelayedAssign(target);
            }

            yield return Timing.WaitForSeconds(0.0005f);

            player.Position = position;
            player.Rotation = rotation;
            player.Health = health;
        }

        private Player SelectPlayer()
        {
            var queue = GetRolesQueue();

            while (queue.Count > 0)
            {
                var role = queue.Dequeue();

                Player player = FindPlayer(role);

                if (player != null && !player.IsHost && !player.IsNPC)
                {
                    return player;
                }
            }

            QueuePool<RoleTypeId>.Pool.Return(queue);

            queue = GetRolesQueue();

            while (queue.Count > 0)
            {
                var role = queue.Dequeue();

                Player player = FindPlayer(role, false);

                if (player != null && !player.IsHost && !player.IsNPC)
                {
                    return player;
                }
            }

            QueuePool<RoleTypeId>.Pool.Return(queue);

            return null;
        }

        private Player FindPlayer(RoleTypeId role, bool checkSubclass = true)
        {
            var players = Player.List.Where(x => x.Role.Type == role);

            if (players.Any() && Player.List.Count(ply => ply.Role.Side == role.GetSide()) > 1)
            {
                if (role == RoleTypeId.Spectator && !checkSubclass)
                {
                    return players.GetRandomValue();
                }

                var withoutSubclass = players.Where(x => !BaseSubclass.Contains(x));

                if (withoutSubclass.Any())
                {
                    return withoutSubclass.GetRandomValue();
                }
            }

            return null;
        }
    }
}
