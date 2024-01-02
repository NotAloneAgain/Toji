using Exiled.API.Features;
using Exiled.API.Features.Doors;
using System;
using Toji.Classes.API.Features.Abilities;

namespace Toji.Classes.Subclasses.Abilities.Active
{
    public class HackAbility(uint cooldown) : CooldownAbility(cooldown)
    {
        public override string Name => "Взлом дверей";

        public override string Desc => "Ты можешь взламывать двери";

        public override bool Activate(Player player, out object result)
        {
            if (!base.Activate(player, out result))
            {
                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            var door = player.GetDoorFromView(5);

            if (player.IsCuffed)
            {
                result = "Ты не можешь взломать дверь, будучи связанным.";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (door == null)
            {
                result = "Не удалось получить дверь (максимальная дистанция 5м).";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (!door.Is(out BreakableDoor breakable) || breakable.IsDestroyed || door.IsElevator || door.IsGate)
            {
                result = "Эту дверь нельзя взломать!";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (door.IsLocked && !breakable.AllowsScp106)
            {
                result = "Блокировка от системы безопасности не позволяет взломать дверь!";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            door.IsOpen = true;
            door.Unlock();

            result = "Ты успешно взломал эту дверь!";

            AddUse(player, DateTime.Now, true, result);

            return true;
        }
    }
}
