using System;
using Toji.Classes.API.Enums;

namespace Toji.Classes.API.Features.Relations
{
    public abstract class PlayerRelation<TTarget> : Relation<TTarget>
    {
        public PlayerRelation(RelationType type, TTarget target) : base(type, target) { }

        public abstract Func<TTarget, bool> CheckAllPlayers { get; }
    }
}
