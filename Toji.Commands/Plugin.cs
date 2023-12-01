﻿using System;
using System.Collections.Generic;
using Toji.Commands.API.Features;
using Toji.Commands.Configs;

namespace Toji.Commands
{
    public sealed class Plugin : Exiled.API.Features.Plugin<Config>
    {
        private Type _commandType;
        private List<CommandBase> _commands;

        public Plugin()
        {
            _commandType = typeof(CommandBase);
            _commands = new List<CommandBase>(25);
        }

        public override string Name => "Toji.Commands";

        public override string Prefix => "Toji.Commands";

        public override string Author => "NotAloneAgain";

        public override Version Version { get; } = new(1, 0, 0);

        public override void OnReloaded() { }

        public override void OnRegisteringCommands()
        {
            foreach (Type type in Assembly.GetTypes())
            {
                if (!type.IsClass || type.IsAbstract || !type.IsSubclassOf(_commandType))
                {
                    continue;
                }

                CommandBase command = Activator.CreateInstance(type) as CommandBase;

                command.Subscribe();

                _commands.Add(command);
            }
        }

        public override void OnUnregisteringCommands()
        {
            foreach (CommandBase command in _commands)
            {
                command.Unsubscribe();
            }

            _commands.Clear();
        }
    }
}
