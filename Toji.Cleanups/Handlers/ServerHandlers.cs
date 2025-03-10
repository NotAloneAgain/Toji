﻿using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using MEC;
using Mirror;
using System.Collections.Generic;
using System.Linq;
using Toji.Cleanups.API.Enums;
using Toji.Cleanups.API.Features;

namespace Toji.Cleanups.Handlers
{
    internal sealed class ServerHandlers
    {
        private List<CoroutineHandle> _coroutines = new (4);
        private GameStage _stage;

        public void OnRoundStarted()
        {
            _coroutines.Clear();

            _coroutines.Add(Timing.RunCoroutine(_UpdateStage().CancelWith(() => Round.IsEnded)));
            _coroutines.Add(Timing.RunCoroutine(_UpdateStates()));
            _coroutines.Add(Timing.RunCoroutine(_CleanupItems()));
            _coroutines.Add(Timing.RunCoroutine(_CleanupRagdolls()));
        }

        public void OnRestartingRound()
        {
            _stage = GameStage.Early;

            Timing.KillCoroutines(_coroutines.ToArray());

            ObjectsStateController.Reset();
        }

        private IEnumerator<float> _UpdateStage()
        {
            _stage = GameStage.Early;

            yield return Timing.WaitForSeconds(364);

            _stage = GameStage.Mid;

            yield return Timing.WaitForSeconds(424);

            _stage = GameStage.Late;

            yield return Timing.WaitForSeconds(424);

            _stage = GameStage.HyperLate;
        }

        private IEnumerator<float> _UpdateStates()
        {
            while (Round.InProgress)
            {
                yield return Timing.WaitForSeconds(10);

                var pickups = Pickup.List.Where(pickup => pickup is not null and { IsSpawned: true }).ToList();
                var ragdolls = Ragdoll.List.Where(ragdoll => ragdoll is not null and { GameObject: not null, IsExpired: true }).ToList();
                var players = Player.List.Where(ply => ply is not null and { IsNPC: false, IsHost: false, IsConnected: true, IsAlive: true }).ToList();

                foreach (var pickup in pickups)
                {
                    ObjectsStateController.TryChangeState(players, pickup);
                }

                foreach (var ragdoll in ragdolls)
                {
                    ObjectsStateController.TryChangeState(players, ragdoll);
                }
            }
        }

        private IEnumerator<float> _CleanupItems()
        {
            while (Round.InProgress)
            {
                var pickups = Pickup.List.Where(pickup => pickup is not null and { IsSpawned: true, IsLocked: false }).ToList();
                var players = Player.List.Where(ply => ply is not null and { IsNPC: false, IsHost: false, IsConnected: true, IsAlive: true }).ToList();

                var cleanup = BaseCleanup.Get<ItemCleanup>(CleanupType.Items, _stage);

                float cooldown = 120;

                if (cleanup != null)
                {
                    Log.Info($"Invoked {cleanup.GetType().Name}");

                    cleanup.Cleanup(pickups, players, out cooldown);
                }

                yield return Timing.WaitForSeconds(cooldown);
            }

            foreach (var pickup in Pickup.List)
            {
                try
                {
                    pickup.Destroy();
                }
                catch { }
            }
        }

        private IEnumerator<float> _CleanupRagdolls()
        {
            while (Round.InProgress)
            {
                var ragdolls = Ragdoll.List.Where(ragdoll => ragdoll != null).ToList();
                var players = Player.List.Where(ply => ply is not null and { IsNPC: false, IsHost: false, IsConnected: true }).ToList();

                var cleanup = BaseCleanup.Get<RagdollCleanup>(CleanupType.Ragdolls, _stage);

                float cooldown = 120;

                if (cleanup != null)
                {
                    Log.Info($"Invoked {cleanup.GetType().Name}");

                    cleanup.Cleanup(ragdolls, players, out cooldown);
                }

                yield return Timing.WaitForSeconds(cooldown);
            }

            foreach (var ragdoll in Ragdoll.List)
            {
                try
                {
                    NetworkServer.Destroy(ragdoll.GameObject);
                }
                catch { }

                try
                {
                    ragdoll.Destroy();
                }
                catch { }
            }
        }
    }
}
