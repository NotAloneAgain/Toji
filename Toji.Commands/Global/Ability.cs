using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;
using Toji.Classes.API.Extensions;
using Toji.Classes.API.Features.Abilities;
using Toji.Commands.API.Enums;
using Toji.Commands.API.Features;
using Toji.Global;

namespace Toji.Commands.Global
{
    public sealed class Ability : CommandBase
    {
        public override string Command { get; set; } = "ability";

        public override string Description { get; set; } = "Команда для использования способности.";

        public override Dictionary<int, string> Syntax { get; set; } = new Dictionary<int, string>()
        {
            { 0, string.Empty },
            { 1, "[Номер]" }
        };

        public override List<CommandType> Types { get; set; } = new List<CommandType>(1) { CommandType.PlayerConsole };

        public override CommandPermission Permission { get; set; } = new(false);

        public override bool ParseSyntax(List<string> input, int count, out List<object> output)
        {
            output = new List<object>();

            if (count != 1)
            {
                output.Add(0);

                return true;
            }

            if (!int.TryParse(input[0], out var number))
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

            if (!player.TryGetSubclass(out var subclass) || subclass == null)
            {
                response = "Ты не имеешь подкласса!";

                return CommandResultType.Fail;
            }

            var activeAbilities = subclass.Abilities.Where(x => x is ActiveAbility active).Select(x => x as ActiveAbility);

            if (!activeAbilities.Any() || activeAbilities.All(x => x == null))
            {
                response = "У твоего подкласса нет активных способностей!";

                return CommandResultType.Fail;
            }

            if (activeAbilities.All(x => !x.AllowConsole))
            {
                response = "Нет способностей, которые могут активироваться из консоли!";

                return CommandResultType.Fail;
            }

            int number = (int)arguments[0];

            if (activeAbilities.Count() < number || number < 0)
            {
                response = $"У твоего подкласса нет способности под цифрой {number}!";

                return CommandResultType.Fail;
            }

            var ability = activeAbilities.ElementAtOrDefault(number);

            if (ability == null)
            {
                response = $"У твоего подкласса нет способности под цифрой {number}!";

                return CommandResultType.Fail;
            }

            var success = ability.Activate(player, out var result);

            if (!success)
            {
                if (result is int value)
                {
                    response = $"Способность на перезарядке ещё {value.GetSecondsString()}.";
                }
                else if (result is string str)
                {
                    response = str;
                }
                else
                {
                    response = "Способность не удалось активировать.";
                }

                return CommandResultType.Fail;
            }

            response = "Активация способности выполнена!";

            return CommandResultType.Success;
        }
    }
}
