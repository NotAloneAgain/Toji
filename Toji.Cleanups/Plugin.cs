using Exiled.Events.Handlers;
using System;
using Toji.Cleanups.API.Features;
using Toji.Cleanups.Handlers;
using Toji.ExiledAPI.Configs;
using Toji.Global;

namespace Toji.Cleanups
{
    public sealed class Plugin : Exiled.API.Features.Plugin<DefaultConfig>
    {
        private ServerHandlers _server;
        private PlayerHandlers _player;
        private MapHandlers _map;

        public override string Name => "Toji.Cleanups";

        public override string Prefix => Name.ToPrefix();

        public override string Author => "NotAloneAgain";

        public override Version Version { get; } = new(1, 0, 0);

        public override void OnEnabled()
        {
            _map = new();
            _player = new();
            _server = new();

            CreateCleanups();

            Map.PlacingBulletHole += _map.OnPlacingBulletHole;

            Player.SpawningRagdoll += _player.OnSpawningRagdoll;

            Server.RestartingRound += _server.OnRestartingRound;
            Server.RoundStarted += _server.OnRoundStarted;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Server.RoundStarted -= _server.OnRoundStarted;
            Server.RestartingRound -= _server.OnRestartingRound;

            Player.SpawningRagdoll -= _player.OnSpawningRagdoll;

            Map.PlacingBulletHole -= _map.OnPlacingBulletHole;

            DestroyCleanups();

            _server = null;
            _player = null;
            _map = null;

            base.OnDisabled();
        }

        public override void OnReloaded() { }

        public override void OnRegisteringCommands() { }

        public override void OnUnregisteringCommands() { }

        private void CreateCleanups()
        {
            Type cleanupType = typeof(BaseCleanup);

            foreach (Type type in Assembly.GetTypes())
            {
                if (!type.IsClass || type.IsAbstract || !type.IsSubclassOf(cleanupType))
                {
                    continue;
                }

                BaseCleanup cleanup = Activator.CreateInstance(type) as BaseCleanup;

                if (cleanup != null)
                {
                    Exiled.API.Features.Log.Info($"Registered {cleanup.Type} cleanup for {cleanup.Stage} game: {cleanup.GetType().Name}");
                }
            }
        }

        private void DestroyCleanups()
        {
            foreach (var cleanup in BaseCleanup.ReadOnlyCollection)
            {
                cleanup.Dispose();
            }
        }
    }
}
