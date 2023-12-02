using Exiled.API.Features;
using Toji.Classes.API.Interfaces;

namespace Toji.Classes.API.Features
{
    public abstract class SingleSubclass : BaseSubclass, ISingle
    {
        public Player Player { get; set; }

        public override bool Has(in Player player) => base.Has(player) && Player?.UserId == player.UserId;

        public override bool Can(in Player player) => base.Can(player) && Player == null;

        public override bool Assign(in Player player)
        {
            if (!base.Assign(player))
            {
                return false;
            }

            Player = player;

            return true;
        }

        public override bool Revoke(in Player player)
        {
            if (!base.Revoke(player))
            {
                return false;
            }

            Player = null;

            LazyUnsubscribe();

            return true;
        }
    }
}
