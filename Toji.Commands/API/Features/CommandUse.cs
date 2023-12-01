using System;
using Toji.Commands.API.Enums;

namespace Toji.Commands.API.Features
{
    public class CommandUse
    {
        public CommandUse(DateTime time, CommandResultType result)
        {
            Time = time;
            Result = result;
        }

        public DateTime Time { get; set; }

        public CommandResultType Result { get; set; }
    }
}
