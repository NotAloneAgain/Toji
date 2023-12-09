using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toji.Classes.API.Features.Abilities;

namespace Toji.Classes.Subclasses.Scp575
{
    public class ThirdShadowRazeAbility : CooldownAbility
    {
        public ThirdShadowRazeAbility(uint cooldown) : base(cooldown)
        {
        }

        public override string Name => "Shadowraze-дальний";

        public override string Desc => "Моментально атакуете всех людей в комплексе и выключаете везде свет.";

        public override bool Activate(Player player, out object result)
        {
            if (!base.Activate(player, out result))
            {
                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (Round.ElapsedTime.TotalMinutes > 5)
            {
                result = "Способность разблокируется через 5 минут после начала раунда!";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (player.CurrentRoom.Zone == ZoneType.Surface)
            {
                result = "Ты не можешь активировать ярость на Поверхности!";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            Map.TurnOffAllLights(70);

            foreach (var ply in Player.List)
            {
                if (ply.IsHost || ply.IsDead || ply.IsScp)
                {
                    continue;
                }

                ply.Hurt(player, 25, DamageType.Crushed, default);
            }

            AddUse(player, DateTime.Now, true, result);

            return true;
        }
    }
}
