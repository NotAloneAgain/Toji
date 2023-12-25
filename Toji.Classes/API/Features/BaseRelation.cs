using Toji.Classes.API.Enums;

namespace Toji.Classes.API.Features
{
    public abstract class BaseRelation
    {
        public BaseRelation(RelationType type) => Type = type;

        public abstract string Desc { get; }

        public RelationType Type { get; }

        public abstract bool Check();
    }
}
