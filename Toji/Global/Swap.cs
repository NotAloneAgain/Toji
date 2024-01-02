using PlayerRoles;
using System.Collections.Generic;

namespace Toji.Global
{
    public static class Swap
    {
        public static bool Prevent { get; set; } = true;

        public static ushort SwapDuration { get; set; } = 90;

        public static List<RoleTypeId> AllowedScps { get; set; } = [

            RoleTypeId.Scp096,
            RoleTypeId.Scp049,
            RoleTypeId.Scp173,
            RoleTypeId.Scp939,
            RoleTypeId.Scp106,
            RoleTypeId.Scp079,
            RoleTypeId.Scp3114
        ];

        public static Dictionary<RoleTypeId, int> Slots { get; set; } = new(8)
        {
            { RoleTypeId.Scp096, 1 },
            { RoleTypeId.Scp049, 1 },
            { RoleTypeId.Scp173, 1 },
            { RoleTypeId.Scp939, 2 },
            { RoleTypeId.Scp106, 1 },
            { RoleTypeId.Scp079, 1 },
            { RoleTypeId.Scp3114, 1 }
        };

        public static Dictionary<RoleTypeId, int> StartScps { get; set; } = new Dictionary<RoleTypeId, int>(6)
        {
            { RoleTypeId.Scp096, 0 },
            { RoleTypeId.Scp049, 0 },
            { RoleTypeId.Scp173, 0 },
            { RoleTypeId.Scp939, 0 },
            { RoleTypeId.Scp106, 0 },
            { RoleTypeId.Scp079, 0 },
            { RoleTypeId.Scp3114, 0 }
        };
    }
}
