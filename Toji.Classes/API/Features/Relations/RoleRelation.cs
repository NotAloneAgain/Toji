using Exiled.API.Features;
using PlayerRoles;
using System;
using System.Linq;
using Toji.Classes.API.Enums;
using Toji.Global;

namespace Toji.Classes.API.Features.Relations
{
    public class RoleRelation(RelationType type, RoleTypeId target) : PlayerRelation<RoleTypeId>(type, target)
    {
        public override string Desc => $"Для получения подкласса должен быть {Target.Translate()}";

        public override Func<RoleTypeId, bool> CheckAllPlayers => (RoleTypeId role) => Player.List.Any(ply => ply.Role.Type == role);

        public override bool Check() => Type switch
        {
            RelationType.Required => CheckAllPlayers(Target),
            RelationType.Not => !CheckAllPlayers(Target),
            _ => false,
        };
    }
}
