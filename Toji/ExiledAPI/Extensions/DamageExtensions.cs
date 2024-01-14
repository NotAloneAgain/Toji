using Exiled.API.Enums;

namespace Toji.ExiledAPI.Extensions
{
    public static class DamageExtensions
    {
        public static bool IsValid(this DamageType type) => type switch
        {
            DamageType.Firearm or DamageType.Scp or DamageType.Scp049 or DamageType.Scp0492
            or DamageType.Scp096 or DamageType.Scp173 or DamageType.Scp939 or DamageType.Scp106
            or DamageType.Crossvec or DamageType.Logicer or DamageType.Revolver or DamageType.Shotgun
            or DamageType.AK or DamageType.Com15 or DamageType.Com18 or DamageType.Fsp9 or DamageType.E11Sr
            or DamageType.ParticleDisruptor or DamageType.Com45 or DamageType.Jailbird or DamageType.Frmg0 or DamageType.A7
            or DamageType.Scp3114 or DamageType.Explosion => true,
            _ => false
        };

        public static bool IsStupid(this DamageType type) => type switch
        {
            DamageType.Unknown or DamageType.Falldown or DamageType.Warhead
            or DamageType.Decontamination or DamageType.Scp207 or DamageType.PocketDimension
            or DamageType.SeveredHands => true,
            _ => false,
        };
    }
}
