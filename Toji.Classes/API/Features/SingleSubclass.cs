using Exiled.API.Features;
using Toji.Classes.API.Interfaces;

namespace Toji.Classes.API.Features
{
    public abstract class SingleSubclass : BaseSubclass, ISingle
    {
        public Player Player { get; set; }
    }
}
