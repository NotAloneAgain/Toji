using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toji.Classes.API.Features.Inventory;
using Toji.Classes.API.Features;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Characteristics;
using Exiled.Events.EventArgs.Player;
using Toji.ExiledAPI.Extensions;
using Exiled.API.Enums;
using Toji.Classes.Subclasses.Abilities.Active;

namespace Toji.Classes.Subclasses.ClassD.Single
{
    public class Scp073 : DSingleSubclass, IHintSubclass, ICustomHintSubclass, IRandomSubclass, IHurtController
    {
        public override bool ShowInfo => true;

        public override string Name => "SCP-073";

        public override string Desc => "У тебя очень крепкое тело и неплохая регенерация";

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            new KnockAbility(90, 5.11f)
        };

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(1)
        {
            new HurtMultiplayerCharacteristic(0.6f)
        };

        public string HintText => string.Empty;

        public string HintColor => "#009A63";

        public float HintDuration => 15;

        public int Chance => 5;

        public void OnHurt(HurtingEventArgs ev)
        {
            if (!ev.IsNotSelfDamage() || ev.Attacker.IsGodModeEnabled || ev.DamageHandler.Type == DamageType.PocketDimension)
            {
                return;
            }

            var amount = ev.Attacker.IsScp ? 50 : ev.Amount * 0.346f;

            if (ev.Attacker.Health - amount > 0)
            {
                ev.Attacker.Hurt(ev.Attacker, amount, ev.DamageHandler.Type, default);
            }
            else
            {
                ev.Attacker.Kill(ev.DamageHandler.Type);
            }
        }
    }
}
