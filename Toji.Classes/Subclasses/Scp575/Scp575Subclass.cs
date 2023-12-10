using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using Toji.Classes.API.Features;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Passive;
using Toji.Classes.Subclasses.Abilities.Ticks;

namespace Toji.Classes.Subclasses.Scp575
{
    public class Scp575Subclass : SingleSubclass, IHintSubclass, IRoleInfo, IRandomSubclass
    {
        public override bool ShowInfo => true;

        public override string Name => "SCP-575";

        public override RoleTypeId Role => RoleTypeId.Scp106;

        public override string Desc => "Прячется в ночи и ищет свою добычу, которая не сможет отогнать от себя мрак";

        public override List<string> Tags { get; } = new List<string>(1) { "Shadow" };

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(6)
        {
            new ShadowAbility(),
            new InvisibleAbility(),
            new FirstShadowRazeAbility(70),
            new SecondShadowRazeAbility(210),
            new ThirdShadowRazeAbility(370),
            new RequiemAbility(480),
        };

        public string RoleInfo => Name;

        public int Chance => 10;

        protected internal override void LazySubscribe()
        {
            base.LazySubscribe();

            Exiled.Events.Handlers.Map.GeneratorActivating += OnActivatingGenerator;
        }

        protected internal override void LazyUnsubscribe()
        {
            Exiled.Events.Handlers.Map.GeneratorActivating -= OnActivatingGenerator;

            base.LazyUnsubscribe();
        }

        private void OnActivatingGenerator(GeneratorActivatingEventArgs ev)
        {
            if (!ev.IsAllowed || Player == null)
            {
                return;
            }

            int activated = Generator.List.Count(x => x.IsEngaged);

            if (activated == Generator.List.Count)
            {
                Player.Kill(DamageType.Bleeding);

                Revoke(Player);
            }
        }
    }
}
