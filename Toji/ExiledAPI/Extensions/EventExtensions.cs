using Exiled.Events.EventArgs.Interfaces;

namespace Toji.ExiledAPI.Extensions
{
    public static class EventExtensions
    {
        public static bool IsValid(this IPlayerEvent ev, bool checkAlive = true) => ev.Player != null && !ev.Player.IsHost && !ev.Player.IsNPC && ev.Player.IsConnected && (!checkAlive || ev.Player.IsAlive);

        public static bool IsAttackerValid(this IAttackerEvent ev, bool checkAlive = true) => ev.Attacker != null && !ev.Attacker.IsHost && !ev.Attacker.IsNPC && ev.Attacker.IsConnected && (!checkAlive || ev.Attacker.IsAlive);

        public static bool IsFullyValid(this IAttackerEvent ev, bool checkAlive = true) => ev.IsValid(checkAlive) && ev.IsAttackerValid(checkAlive);

        public static bool IsNotSelfDamage(this IAttackerEvent ev, bool checkAlive = true) => ev.IsFullyValid(checkAlive) && ev.Attacker.UserId != ev.Player.UserId;
    }
}
