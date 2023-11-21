using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Server;
using PlayerRoles;
using System.Linq;
using Toji.AutoFF.API.Enums;
using UnityEngine;

#pragma warning disable IDE0060 // Удалите неиспользуемый параметр

namespace Toji.AutoFF.Handlers
{
    internal sealed class RoundHandlers
    {
        public void OnRoundStarted()
        {
            Server.FriendlyFire = false;
        }

        public void OnRestartingRound()
        {
            Server.FriendlyFire = false;
        }

        public void OnEndingRound(EndingRoundEventArgs ev)
        {
            if (!ev.IsRoundEnded || !ev.IsAllowed)
            {
                return;
            }

            var array = System.Enum.GetValues(typeof(EndRoundAction)).ToArray<EndRoundAction>();

            var action = array[Random.Range(0, array.Length)];

            if (action.ToString().Contains("Force") && Player.List.Count > 35)
            {
                return;
            }

            OpenDoors();

            switch (action)
            {
                case EndRoundAction.ForceAllToScps:
                    {
                        foreach (var player in Player.List)
                        {
                            player.Role.Set(Random.Range(0, 5) switch
                            {
                                4 => RoleTypeId.Scp173,
                                3 => RoleTypeId.Scp096,
                                2 => RoleTypeId.Scp049,
                                1 => RoleTypeId.Scp939,
                                _ => RoleTypeId.Scp3114
                            });

                            player.EnableEffect(EffectType.MovementBoost, 255, 255);
                        }

                        break;
                    }
                case EndRoundAction.ForceAllToNtf:
                    {
                        foreach (var player in Player.List)
                        {
                            player.Role.Set(Random.Range(0, 4) switch
                            {
                                3 => RoleTypeId.NtfPrivate,
                                2 => RoleTypeId.NtfSergeant,
                                1 => RoleTypeId.NtfSpecialist,
                                _ => RoleTypeId.NtfCaptain
                            });

                            player.DisableAllEffects();
                        }

                        break;
                    }
                case EndRoundAction.ForceAllToChaos:
                    {
                        foreach (var player in Player.List)
                        {
                            player.Role.Set(Random.Range(0, 4) switch
                            {
                                3 => RoleTypeId.ChaosRifleman,
                                2 => RoleTypeId.ChaosMarauder,
                                1 => RoleTypeId.ChaosConscript,
                                _ => RoleTypeId.ChaosRepressor
                            });

                            player.DisableAllEffects();
                        }

                        break;
                    }
                case EndRoundAction.TeleportAndHorse:
                    {
                        TeleportPlayers(Random.Range(0, 3) switch
                        {
                            2 => ZoneType.HeavyContainment,
                            1 => ZoneType.HeavyContainment,
                            _ => ZoneType.Entrance
                        }, ItemType.Jailbird);

                        break;
                    }
                case EndRoundAction.TeleportAndLazer:
                    {
                        TeleportPlayers(Random.Range(0, 3) switch
                        {
                            2 => ZoneType.HeavyContainment,
                            1 => ZoneType.HeavyContainment,
                            _ => ZoneType.Entrance
                        }, ItemType.ParticleDisruptor);

                        break;
                    }
                case EndRoundAction.TeleportAndCum:
                    {
                        TeleportPlayers(ZoneType.Entrance, ItemType.GunCom45);

                        break;
                    }
                case EndRoundAction.TeleportAndRun:
                    {
                        foreach (var player in Player.List)
                        {
                            player.Role.Set(RoleTypeId.Tutorial);

                            player.EnableEffect(EffectType.MovementBoost, 255, 255);
                        }

                        break;
                    }
            }
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            Server.FriendlyFire = true;
        }

        private void OpenDoors()
        {
            foreach (var door in Door.List)
            {
                if (door == null || door.Is<BreakableDoor>(out var breakable) && breakable.IsDestroyed)
                {
                    continue;
                }

                door.ChangeLock(DoorLockType.AdminCommand);
                door.IsOpen = true;
            }
        }

        private void TeleportPlayers(ZoneType zone, ItemType item)
        {
            var rooms = Room.List.Where(room => room != null && room.Zone == zone).ToList();

            foreach (Player player in Player.List)
            {
                if (player == null || player.IsHost || player.IsDead || player.IsNPC || !player.IsConnected)
                {
                    continue;
                }

                player.ClearInventory();

                player.CurrentItem = player.AddItem(item);

                player.Teleport(GetSafeTeleportPosition(rooms[Random.Range(0, rooms.Count)]) + Vector3.up);
            }
        }

        private static Vector3 GetSafeTeleportPosition(Room room)
        {
            if (IsSafe(room.Type))
            {
                return room.Position;
            }

            var doors = room.Doors.ToList();

            if (doors.Count == 0)
            {
                return room.Position;
            }

            return doors[Random.Range(0, doors.Count)].Position;
        }

        private static bool IsSafe(RoomType room) => room is RoomType.Lcz173 or RoomType.HczTesla or RoomType.HczTestRoom;
    }
}
