using Exiled.API.Features;
using PlayerRoles;

namespace Toji.BetterCuff.API
{
    public static class Extensions
    {
        public static Faction GetFaction(this Player player) => player == null ? Faction.Unclassified : player.Role.Team.GetFaction();

        public static Faction GetEnemyFaction(this Player player) => player.GetFaction() switch
        {
            Faction.FoundationEnemy => Faction.FoundationStaff,
            Faction.FoundationStaff => Faction.FoundationEnemy,
            Faction.SCP => Faction.FoundationStaff,
            Faction.Unclassified => Faction.SCP,
            _ => throw new System.NotImplementedException(),
        };
    }
}
