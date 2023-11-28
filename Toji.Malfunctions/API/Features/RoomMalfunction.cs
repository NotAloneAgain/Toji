using Exiled.API.Extensions;
using Exiled.API.Features;

namespace Toji.Malfunctions.API.Features
{
    public abstract class RoomMalfunction : ObjectMalfunction<Room>
    {
        protected override Room SelectObject() => Room.List.GetRandomValue();
    }
}
