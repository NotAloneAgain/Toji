using Exiled.API.Features;
using Exiled.Loader;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;
using System;
using Toji.Classes.API.Features.Abilities;
using Toji.Global;

namespace Toji.Classes.Subclasses.Abilities.Active
{
    public class DisableAbility(uint cooldown) : CooldownAbility(cooldown)
    {
        public override string Name => "Отключение";

        public override string Desc => "Ты можешь временно отключить SCP-079 от систем управления комплексом, если он находится поблизости";

        public override bool Activate(Player player, out object result)
        {
            if (!base.Activate(player, out result))
            {
                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (player.IsCuffed)
            {
                result = "Ты не можешь отключить SCP-079, будучи связанным.";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            var time = Loader.Random.Next(5, 13);

            bool affected = false;

            foreach (var ply in Player.List)
            {
                if (ply.Role.Type != RoleTypeId.Scp079 || ply.Zone != player.Zone)
                {
                    continue;
                }

                var scp = ply.Role.Base as Scp079Role;

                if (scp == null || !scp.SubroutineModule.TryGetSubroutine(out Scp079LostSignalHandler lost))
                {
                    continue;
                }

                lost.ServerLoseSignal(time);

                affected = true;
            }

            result = affected switch
            {
                true => $"Ты успешно отключил его на {time.GetSecondsString()}!",
                false => "У тебя не вышло..."
            };

            AddUse(player, DateTime.Now, true, result);

            return true;
        }
    }
}
