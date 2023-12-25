using Toji.Classes.API.Enums;

namespace Toji.Classes.API.Features
{
    public abstract class Relation<TTarget> : BaseRelation
    {
        protected Relation(RelationType type, TTarget target) : base(type)
        {
            Target = target;
        }

        public TTarget Target { get; }
    }
}
