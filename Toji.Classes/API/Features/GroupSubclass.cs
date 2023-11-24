using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;
using Toji.Classes.API.Interfaces;

namespace Toji.Classes.API.Features
{
    public abstract class GroupSubclass : BaseSubclass, IGroup
    {
        public GroupSubclass() => Players = new(100);

        public HashSet<Player> Players { get; }

        public override bool Has(in Player player) => base.Has(player) && Players.Contains(player);

        public override bool Can(in Player player) => base.Can(player) && (this is not ILimitableGroup group || group.Players.Count < group.Max);

        public override bool Assign(in Player player)
        {
            if (!base.Assign(player))
            {
                return false;
            }

            Players.Add(player);

            return true;
        }

        public override bool Revoke(in Player player)
        {
            if (!base.Revoke(player))
            {
                return false;
            }

            Players.Remove(player);

            if (Players.Any(ply => ply != null))
            {
                return false;
            }

            Unsubscribe();

            return true;
        }
    }
}
