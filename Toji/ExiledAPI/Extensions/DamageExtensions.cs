using Exiled.API.Enums;

namespace Toji.ExiledAPI.Extensions
{
    public static class DamageExtensions
    {
        public static bool IsHumanDamage(this DamageType type)
        {
            return type switch
            {
                DamageType.Firearm or DamageType.Revolver or DamageType.Crossvec or
                DamageType.AK or DamageType.E11Sr or DamageType.Fsp9 or
                DamageType.Logicer or DamageType.Shotgun or DamageType.Com45 or
                DamageType.Com18 or DamageType.A7 or DamageType.Fsp9 => true,
                _ => false
            };
        }
    }
}
