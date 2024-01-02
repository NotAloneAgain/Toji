using System;
using Toji.Commands.API.Enums;

namespace Toji.Commands.API.Features
{
    public class CommandUse(DateTime time, CommandResultType result)
    {
        public DateTime Time { get; set; } = time;

        public CommandResultType Result { get; set; } = result;
    }
}
