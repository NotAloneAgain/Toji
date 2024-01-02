using Exiled.API.Features;
using System;

namespace Toji.Classes.API.Features.Abilities
{
    public class AbilityUse(Player player, DateTime time)
    {
        public AbilityUse(Player player, DateTime time, bool success, object result = null!) : this(player, time)
        {
            Result = result;
            IsSuccessful = success;
        }

        public Player Player { get; init; } = player;

        public DateTime Time { get; init; } = time;

        public object Result { get; init; }

        public bool IsSuccessful { get; init; }
    }
}
