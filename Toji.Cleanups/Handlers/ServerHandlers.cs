using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using MEC;
using System.Collections.Generic;
using System.Linq;
using Toji.Cleanups.API.Enums;
using Toji.Cleanups.API.Features;

namespace Toji.Cleanups.Handlers
{
    internal sealed class ServerHandlers
    {
        private List<CoroutineHandle> _coroutines = new (3);
        private GameStage _stage;

        public void OnRoundStarted()
        {
            _coroutines.Clear();

            _coroutines.Add(Timing.RunCoroutine(_UpdateStage()));
            _coroutines.Add(Timing.RunCoroutine(_CleanupItems()));
            _coroutines.Add(Timing.RunCoroutine(_CleanupRagdolls()));
        }

        public void OnRestartingRound()
        {
            Timing.KillCoroutines(_coroutines.ToArray());
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

        private IEnumerator<float> _CleanupItems()
        {
            while (Round.InProgress)
            {
                var pickups = Pickup.List.Where(pickup => pickup is not null and { IsSpawned: true, IsLocked: false }).ToList();
                var players = Player.List.Where(ply => ply is not null and { IsNPC: false, IsHost: false, IsConnected: true }).ToList();

                var cleanup = BaseCleanup.Get<ItemCleanup>(CleanupType.Items, _stage);

                float cooldown = 120;

                if (cleanup != null)
                {
                    Log.Info($"Invoked {cleanup.GetType().Name}");

                    cleanup.Cleanup(pickups, players, out cooldown);
                }

                yield return Timing.WaitForSeconds(cooldown);
            }
        }

        private IEnumerator<float> _CleanupRagdolls()
        {
            while (Round.InProgress)
            {
                var ragdolls = Ragdoll.List.Where(pickup => pickup is not null and { CanBeCleanedUp: true }).ToList();
                var players = Player.List.Where(ply => ply is not null and { IsNPC: false, IsHost: false, IsConnected: true }).ToList();

                var cleanup = BaseCleanup.Get<RagdollCleanup>(CleanupType.Ragdolls, _stage);

                float cooldown = 120;

                cleanup?.Cleanup(ragdolls, players, out cooldown);

                yield return Timing.WaitForSeconds(cooldown);
            }
        }
    }
}
