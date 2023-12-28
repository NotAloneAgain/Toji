using Exiled.API.Features;
using Toji.Classes.API.Interfaces;

namespace Toji.Classes.API.Features.Subclasses
{
    public abstract class SingleSubclass : BaseSubclass, ISingle
    {
        public Player Player { get; set; }

        public override bool Add(Player player)
        {
            if (!base.Add(player) || Player == player)
            {
                return false;
            }

            Player = player;

            return true;
        }

        public override bool Remove(Player player)
        {
            if (!base.Remove(player) || Player != player || Player == null)
            {
                return false;
            }

            Player = null;

            return true;
        }

        public override bool Has(in Player player) => base.Has(player) && Player?.UserId == player.UserId;

        public override bool Can(in Player player) => base.Can(player) && Player == null;

        public sealed override bool Assign(in Player player) => Player == null && base.Assign(player);

        public sealed override bool Revoke(in Player player)
        {
            if (Player == null || !base.Revoke(player))
            {
                return false;
            }

            LazyUnsubscribe();

            return true;
        }
    }
}
