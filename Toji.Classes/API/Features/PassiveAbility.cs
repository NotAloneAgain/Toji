using Toji.Classes.API.Interfaces;

namespace Toji.Classes.API.Features
{
    public abstract class PassiveAbility : BaseAbility, ISubscribable
    {
        public abstract void Subscribe();

        public abstract void Unsubscribe();
    }
}
