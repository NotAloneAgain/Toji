using Exiled.API.Features;

namespace Toji.Classes.API.Features
{
    public abstract class BaseCharacteristic : BaseAbility
    {
        public override string Desc => GetDesc();

        public abstract string GetDesc(Player player = null);
    }
}
