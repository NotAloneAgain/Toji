using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Active;
using Toji.Classes.Subclasses.Characteristics;
using Toji.ExiledAPI.Extensions;

namespace Toji.Classes.Subclasses.Guards.Single
{
    public class Imposter : GuardSingleSubclass, IHintSubclass, IRandomSubclass, IHurtController, IDamageController
    {
        public override bool ShowInfo => false;

        public override string Name => "Предатель";

        public override string Desc => "Агент под кодовым именем 'Штирлиц' среди охраны комплекса, готовый к выводу лояльного D-Персонала";

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(2)
        {
            new AmogusAbility(180, RoleTypeId.ChaosRifleman, RoleTypeId.FacilityGuard, ItemType.GunAK),
            new ReturnFaceAbility(180),
        };

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(2)
        {
            new HealthCharacteristic(125),
            new EffectsCharacteristic(EffectType.Asphyxiated)
        };

        public int Chance => 10;

        public void OnDamage(HurtingEventArgs ev)
        {
            ev.IsAllowed = ev.Player.Role.Side != Side.ChaosInsurgency;
        }

        public void OnHurt(HurtingEventArgs ev)
        {
            if (!ev.IsNotSelfDamage() || ev.Player.Role.Type != RoleTypeId.FacilityGuard || ev.Attacker.Role.Side != Side.ChaosInsurgency)
            {
                return;
            }

            ev.IsAllowed = false;

            ev.Player.Role.Set(RoleTypeId.ChaosRifleman, SpawnReason.Revived, RoleSpawnFlags.None);

            ev.Player.DisableAllEffects();

            Revoke(ev.Player);
        }
    }
}
