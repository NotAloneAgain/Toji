using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System.Linq;
using Toji.Global;

namespace Toji.ScpSwap.Handlers
{
    internal sealed class SwapHandlers
    {
        private readonly float _duration;
        private readonly string _text;

        internal SwapHandlers(float duration, string text)
        {
            _duration = duration;
            _text = text;
        }

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Reason is not SpawnReason.RoundStart and not SpawnReason.LateJoin and not SpawnReason.Respawn)
            {
                return;
            }

            if (ev.NewRole.GetTeam() != Team.SCPs || ev.NewRole == RoleTypeId.Scp0492)
            {
                return;
            }

            if (Player.List.Count(ply => ply.Role.Team == Team.SCPs) == 6 && ev.NewRole is not RoleTypeId.Scp939 and not RoleTypeId.Scp3114)
            {
                ev.NewRole = RoleTypeId.Scp939;
            }

            if (Round.ElapsedTime.TotalSeconds > Swap.SwapDuration)
            {
                return;
            }

            ev.Player.ShowHint(string.Format(_text, Swap.SwapDuration), _duration);

            Swap.StartScps[ev.NewRole]++;
        }

        public void OnDestroying(DestroyingEventArgs ev)
        {
            if (!Swap.AllowedScps.Contains(ev.Player.Role.Type))
            {
                return;
            }

            Swap.StartScps[ev.Player.Role]--;
        }

        public void Reset()
        {
            foreach (RoleTypeId role in Swap.StartScps.Keys.ToHashSet())
            {
                Swap.StartScps[role] = 0;
            }
        }
    }
}
