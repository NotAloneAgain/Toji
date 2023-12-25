using Exiled.API.Features;
using System;
using System.Linq;
using Toji.Classes.API.Enums;
using Toji.Classes.API.Extensions;

namespace Toji.Classes.API.Features.Relations
{
    public class SubclassRelation : PlayerRelation<BaseSubclass>
    {
        public SubclassRelation(RelationType type, BaseSubclass target) : base(type, target) { }

        public override string Desc => $"Для получения подкласса должен быть {Target.Name}";

        public override Func<BaseSubclass, bool> CheckAllPlayers => (BaseSubclass subclass) => Player.List.Any(ply => ply.TryGetSubclass(out var sub) && sub == subclass);

        public override bool Check() => Type switch
        {
            RelationType.Required => CheckAllPlayers(Target),
            RelationType.Not => !CheckAllPlayers(Target),
            _ => false,
        };
    }
}
