using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Interactables.Interobjects.DoorUtils;
using UnityEngine;

namespace Toji.Classes.Subclasses.Abilities
{
    public static class Extensions
    {
        public static Player GetFromView(this Player owner, float distance)
        {
            if (!Physics.Raycast(owner.Position, owner.Transform.forward, out RaycastHit hit, distance))
            {
                return null;
            }

            var target = Player.Get(hit.transform.GetComponentInParent<ReferenceHub>());

            return target;
        }

        public static Door GetDoorFromView(this Player owner, float distance)
        {
            if (!Physics.Raycast(owner.Position, owner.Transform.forward, out RaycastHit hit, distance))
            {
                return null;
            }

            var door = Door.Get(hit.transform.GetComponentInParent<DoorVariant>());

            return door;
        }
    }
}
