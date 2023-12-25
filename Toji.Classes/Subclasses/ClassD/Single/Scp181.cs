using System.Collections.Generic;
using Toji.Classes.API.Enums;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Relations;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Passive;

namespace Toji.Classes.Subclasses.ClassD.Single
{
    public class Scp181 : DSingleSubclass, IHintSubclass, ICustomHintSubclass, IRandomSubclass
    {
        public override string Name => "SCP-181";

        public override string Desc => "Везение твой конек, являеться ли нарушение У.С. SCP-Объектов везением? Скоро узнаем";

        public override List<BaseRelation> Relations { get; } = new List<BaseRelation>(1)
        {
            new SubclassRelation(RelationType.Not, GetInstance(typeof(Scp073)))
        };

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(3)
        {
            new DodgeAbility(89, 21, 0.77f),
            new SurviveAbility(5),
            new DoorErrorAbility(4)
        };

        public string HintText => string.Empty;

        public string HintColor => "#009A63";

        public float HintDuration => 15;

        public int Chance => 6;
    }
}
