using System;
using Toji.Classes.API.Enums;

namespace Toji.Classes.API.Features.Relations
{
    public class ChanceRelation(RelationType type, int target) : Relation<int>(type, target)
    {
        public override string Desc => "";

        public void Activate(BaseSubclass subclass)
        {
            throw new NotImplementedException();
        }

        public override bool Check()
        {
            throw new NotImplementedException();
        }
    }
}
