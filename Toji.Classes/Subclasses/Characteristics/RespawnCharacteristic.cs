using Exiled.API.Features;
using Toji.Classes.API.Features.Characteristics;

namespace Toji.Classes.Subclasses.Characteristics
{
    public class RespawnCharacteristic(bool value) : Characteristic<bool>(value)
    {
        public override string Name => "Сохранение подкласса";

        public override string GetDesc(Player player = null) => Value ? "Ваш подкласс остается с вами даже после смерти и возрождения" : "Вы не будете сохранять подкласс после смерти (возможно, нерф).";
    }
}
