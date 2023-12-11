using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
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
        public void OnDestroying(DestroyingEventArgs ev)
        {
            if (!ev.IsValid() || !ev.Player.IsScp)
            {
                return;
            }

            var role = ev.Player.Role.Type;
            var subclass = ev.Player.GetSubclass();

            var player = SelectPlayer(GetRolesQueue(RoleTypeId.ClassD, RoleTypeId.Scientist, RoleTypeId.FacilityGuard, RoleTypeId.NtfPrivate, RoleTypeId.ChaosRifleman));

            if (player == null)
            {
                return;
            }

            player.DropItems();

            Timing.RunCoroutine(_Replace(player, ev.Player));

            if (subclass != null)
            {
                if (subclass.Has(ev.Player))
                {
                    subclass.Revoke(ev.Player);
                }

                subclass.DelayedAssign(player);
            }
        }

        private Queue<RoleTypeId> GetRolesQueue(params RoleTypeId[] roles) => new(roles);

        private IEnumerator<float> _Replace(Player player, Player target)
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

            yield return Timing.WaitForSeconds(0.0005f);

            player.Position = position;
            player.Rotation = rotation;
            player.Health = health;
        }

        private Player SelectPlayer(Queue<RoleTypeId> roles)
        {
            Player player = Server.Host;

            var queue = roles.Copy();

            do
            {
                var role = queue.Dequeue();

                player = FindPlayer(role);
            } while (queue.Count > 0 && player is null or { IsHost: true });

            if (player == null)
            {
                do
                {
                    var role = roles.Dequeue();

                    player = FindPlayer(role, false);
                } while (roles.Count > 0 && player is null or { IsHost: true });
            }

            return player;
        }

        private Player FindPlayer(RoleTypeId role, bool checkSubclass = true)
        {
            var players = Player.List.Where(x => x.Role.Type == role);

            if (players.Any() && Player.List.Count(ply => ply.Role.Side == role.GetSide()) > 1)
            {
                if (role == RoleTypeId.Spectator || !checkSubclass)
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
