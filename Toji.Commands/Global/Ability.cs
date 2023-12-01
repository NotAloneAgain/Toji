using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;
using Toji.Classes.API.Extensions;
using Toji.Classes.API.Features.Abilities;
using Toji.Commands.API;
using Toji.Commands.API.Enums;
using Toji.Commands.API.Features;
using Toji.Global;

namespace Toji.Commands.Global
{
    public sealed class Ability : CommandBase
    {
        public override string Command { get; set; } = "force";

        public override string Description { get; set; } = "Команда для смены своего SCP-Объекта.";

        public override Dictionary<int, string> Syntax { get; set; } = new Dictionary<int, string>()
        {
            { 1, "[Номер]" }
        };

        public override List<CommandType> Types { get; set; } = new List<CommandType>(1) { CommandType.PlayerConsole };

        public override CommandPermission Permission { get; set; } = new()
        {
            IsLimited = true,
        };

        public override bool ParseSyntax(List<string> input, int count, out List<object> output)
        {
            output = new List<object>();

            if (count != 1)
            {
                output.Add(0);

                return true;
            }


            if (!byte.TryParse(input[0], out var number))
            {
                return false;
            }

            output.Add(number);

            return true;
        }

        public override CommandResultType Handle(List<object> arguments, Player player, out string response)
        {
            response = string.Empty;

            if (arguments == null || !arguments.Any())
            {
                return CommandResultType.Fail;
            }

            if (!player.TryGetSubclass(out var subclass))
            {
                response = "Ты не имеешь подкласса!";

                return CommandResultType.Fail;
            }

            var activeAbilities = subclass.Abilities.Where(x => x is ActiveAbility).Select(x => x as ActiveAbility);

            if (!activeAbilities.Any() || activeAbilities.All(x => x == null))
            {
                response = "У твоего подкласса нет активных способностей!";

                return CommandResultType.Fail;
            }

            int number = (byte)arguments[0];

            if (activeAbilities.Count() < number)
            {
                response = $"У твоего подкласса нет способности под цифрой {number}!";

                return CommandResultType.Fail;
            }

            var ability = activeAbilities.ElementAt(number);

            var success = ability.Activate(player, out var result);

            if (!success)
            {
                if (result is int value)
                {
                    response = $"Способность на перезарядке ещё {value.GetSecondsString()}.";
                }
                else
                {
                    response = "Способность не удалось активировать.";
                }

                return CommandResultType.Fail;
            }

            return CommandResultType.Success;
        }

        public override bool CheckPermissions(Player player)
        {
            return base.CheckPermissions(player) || player.IsScp && Swap.AllowedScps.Contains(player.Role);
        }
    }
}
