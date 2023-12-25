using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Server;
using PlayerRoles;
using System.Linq;
using Toji.AutoFF.API.Enums;
using Toji.Classes.API.Extensions;
using UnityEngine;

#pragma warning disable IDE0060

namespace Toji.AutoFF.Handlers
{
    internal sealed class RoundHandlers
    {
        private EndRoundAction _roundAction;

        public void OnRoundStarted() => Server.FriendlyFire = false;

        public void OnRestartingRound() => Server.FriendlyFire = false;

        public void OnEndingRound(EndingRoundEventArgs ev)
        {
            if (!ev.IsRoundEnded || !ev.IsAllowed)
            {
                return;
            }

            var array = System.Enum.GetValues(typeof(EndRoundAction)).ToArray<EndRoundAction>();

            _roundAction = array[Random.Range(0, array.Length)];

            OpenDoors();
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            Server.FriendlyFire = true;

            if (_roundAction.ToString().Contains("Force") && Player.List.Count > 35)
            {
                return;
            }

            switch (_roundAction)
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
                            }, RoleSpawnFlags.UseSpawnpoint);

                            player.DisableAllEffects();
                        }

                        break;
                    }
                case EndRoundAction.ForceAllToFlamingos:
                    {
                        foreach (var player in Player.List)
                        {
                            player.Role.Set(Random.Range(0, 2) switch
                            {
                                1 => RoleTypeId.AlphaFlamingo,
                                _ => RoleTypeId.Flamingo
                            }, RoleSpawnFlags.UseSpawnpoint);

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
                            player.Role.Set(RoleTypeId.Tutorial, RoleSpawnFlags.AssignInventory);

                            player.RandomTeleport(typeof(Door));

                            player.EnableEffect(EffectType.MovementBoost, 255, 255);
                        }

                        break;
                    }
            }
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
                door.IsOpen = !door.IsGate && !door.IsCheckpoint;
            }
        }

        private void TeleportPlayers(ZoneType zone, ItemType item = ItemType.None)
        {
            var rooms = Room.Get(zone);

            foreach (Player player in Player.List)
            {
                if (player == null || player.IsHost || player.IsDead || player.IsNPC || !player.IsConnected)
                {
                    continue;
                }

                player.ClearInventory();

                if (item != ItemType.None)
                {
                    player.CurrentItem = player.AddItem(item);
                }

                player.Teleport(rooms.GetRandomValue().GetSafePosition() + Vector3.up);
            }
        }
    }
}
