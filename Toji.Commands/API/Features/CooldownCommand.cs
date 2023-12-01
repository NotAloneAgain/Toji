using Exiled.API.Features;
using System;
using System.Collections.Generic;
using Toji.Commands.API.Enums;

namespace Toji.Commands.API.Features
{
    public abstract class CooldownCommand : CommandBase
    {
        public abstract int Cooldown { get; set; }

        public override CommandResultType Handle(List<object> arguments, Player player, out string response)
        {
            response = string.Empty;

            CommandUse use = History.GetLastSuccessfulUse(player);

            if (use == null)
            {
                return CommandResultType.Success;
            }

            TimeSpan time = DateTime.Now - use.Time;

            if (time.TotalSeconds < Cooldown)
            {
                response = $"Вам осталось ждать {(Cooldown - time.TotalSeconds).GetSecondsString()}.";

                return CommandResultType.Fail;
            }

            return CommandResultType.Success;
        }
    }
}
