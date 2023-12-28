using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Scp106;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.SpawnRules;
using Toji.Classes.API.Features.Subclasses;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Passive;
using Toji.Classes.Subclasses.Abilities.Ticks;
using Toji.ExiledAPI.Extensions;

namespace Toji.Classes.Subclasses.Scp575
{
    public class Scp575Subclass : SingleSubclass, IHintSubclass, IRoleInfo, IRandomSubclass
    {
        public override string Name => "SCP-575";

        public override BaseSpawnRules SpawnRules { get; } = new TeamSpawnRules(Team.SCPs, RoleTypeId.Scp106);

        public override string Desc => "Прячется в ночи и ищет свою добычу, которая не сможет отогнать от себя мрак";

        public override List<string> Tags { get; } = new List<string>(1) { "Shadow" };

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(6)
        {
            new ShadowAbility(),
            new InvisibleAbility(),
            new FirstShadowRazeAbility(60),
            new SecondShadowRazeAbility(200),
            new ThirdShadowRazeAbility(360),
            new RequiemAbility(440),
        };

        public string RoleInfo => Name;

        public int Chance => 10;

        protected internal override void LazySubscribe()
        {
            base.LazySubscribe();

            Exiled.Events.Handlers.Scp106.Attacking += OnAttacking;
            Exiled.Events.Handlers.Map.GeneratorActivating += OnActivatingGenerator;
        }

        protected internal override void LazyUnsubscribe()
        {
            Exiled.Events.Handlers.Map.GeneratorActivating -= OnActivatingGenerator;
            Exiled.Events.Handlers.Scp106.Attacking -= OnAttacking;

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
                Revoke(Player);

                Player.Kill(DamageType.Bleeding);
            }
        }

        public void OnAttacking(AttackingEventArgs ev)
        {
            if (!ev.IsValid() || !Has(ev.Player) || !ev.IsAllowed)
            {
                return;
            }

            ev.IsAllowed = false;
            ev.Player.ShowHitMarker();
            ev.Target.Hurt(2);
            ev.Scp106.Attack?.SendCooldown(0.5f);
        }
    }
}
