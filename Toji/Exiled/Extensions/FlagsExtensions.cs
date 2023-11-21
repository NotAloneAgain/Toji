using Interactables.Interobjects.DoorUtils;

namespace Toji.ExiledAPI.Extensions
{
    public static class FlagsExtensions
    {
        public static bool HasFlagFast(this KeycardPermissions perm, KeycardPermissions perm2) => (perm & perm2) == perm2;
    }
}
