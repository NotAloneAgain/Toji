using System;
using Toji.Classes.API.Enums;

namespace Toji.Classes.API.Features.Relations
{
    public abstract class PlayerRelation<TTarget>(RelationType type, TTarget target) : Relation<TTarget>(type, target)
    {
        public abstract Func<TTarget, bool> CheckAllPlayers { get; }
    }
}
