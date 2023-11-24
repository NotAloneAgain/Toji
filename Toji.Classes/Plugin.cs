using Exiled.Events.Handlers;
using System;
using Toji.Classes.API.Features;
using Toji.Classes.Configs;
using Toji.Classes.Handlers;

namespace Toji.Classes
{
    public sealed class Plugin : Exiled.API.Features.Plugin<Config>
    {
        private PlayerHandlers _handlers;

        public override string Name => "Toji.Classes";

        public override string Prefix => "Toji.Classes";

        public override string Author => "NotAloneAgain";

        public override Version Version { get; } = new(1, 0, 0);

        public override void OnEnabled()
        {
            _handlers = new();

            Player.TriggeringTesla += _handlers.OnTriggeringTesla;
            Player.ChangingRole += _handlers.OnChangingRole;
            Player.Hurting += _handlers.OnHurting;

            CreateSubclasses();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            DestroySubclasses();

            Player.Hurting -= _handlers.OnHurting;
            Player.ChangingRole -= _handlers.OnChangingRole;
            Player.TriggeringTesla -= _handlers.OnTriggeringTesla;

            _handlers = null;

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

                subclass.Subscribe();
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
