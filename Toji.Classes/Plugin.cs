using Exiled.API.Enums;
using Exiled.Events.Handlers;
using HarmonyLib;
using PlayerRoles.Ragdolls;
using System;
using Toji.Classes.API.Features;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Configs;
using Toji.Classes.Handlers;
using Toji.Global;

namespace Toji.Classes
{
    public sealed class Plugin : Exiled.API.Features.Plugin<Config>
    {
        private const string HarmonyId = "zenin";

        private RagdollHandlers _ragdoll;
        private ServerHandlers _server;
        private PlayerHandlers _player;
        private Harmony _harmony;

        public override string Name => "Toji.Classes";

        public override string Prefix => Name.ToPrefix();

        public override string Author => "NotAloneAgain";

        public override PluginPriority Priority => PluginPriority.Low;

        public override Version Version { get; } = new(1, 0, 0);

        public override void OnEnabled()
        {
            _harmony = new(HarmonyId);

            _ragdoll = new();
            _server = new();
            _player = new();

            RagdollManager.OnRagdollRemoved += _ragdoll.OnRagdollRemoved;
            Player.SpawningRagdoll += _ragdoll.OnSpawningRagdoll;
            Player.SpawnedRagdoll += _ragdoll.OnSpawnedRagdoll;

            Server.RestartingRound += _server.OnRoundRestarting;

            Player.ChangingRole += _player.OnChangingRole;
            Player.Destroying += _player.OnDestroying;
            Player.Hurting += _player.OnHurting;

            CreateSubclasses();

            _harmony.PatchAll();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            _harmony.UnpatchAll(HarmonyId);

            DestroySubclasses();

            Player.Hurting -= _player.OnHurting;
            Player.Destroying -= _player.OnDestroying;
            Player.ChangingRole -= _player.OnChangingRole;

            Server.RestartingRound -= _server.OnRoundRestarting;

            Player.SpawnedRagdoll -= _ragdoll.OnSpawnedRagdoll;
            Player.SpawningRagdoll -= _ragdoll.OnSpawningRagdoll;
            RagdollManager.OnRagdollRemoved -= _ragdoll.OnRagdollRemoved;

            _player = null;
            _server = null;
            _ragdoll = null;

            _harmony = null;

            base.OnDisabled();
        }

        public override void OnReloaded() { }

        public override void OnRegisteringCommands() { }

        public override void OnUnregisteringCommands() { }

        private void CreateSubclasses()
        {
            Type subclassType = typeof(BaseSubclass);

            foreach (Type type in Assembly.GetTypes())
            {
                if (!type.IsClass || type.IsAbstract || !type.IsSubclassOf(subclassType))
                {
                    continue;
                }

                BaseSubclass subclass = Activator.CreateInstance(type) as BaseSubclass;

                if (subclass is ISubscribable sub)
                {
                    sub.Subscribe();
                }
            }
        }

        private void DestroySubclasses()
        {
            foreach (var subclass in BaseSubclass.ReadOnlyCollection)
            {
                subclass.Dispose();
            }
        }
    }
}
