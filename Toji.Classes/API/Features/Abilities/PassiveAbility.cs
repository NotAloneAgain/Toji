using Toji.Classes.API.Interfaces;

namespace Toji.Classes.API.Features.Abilities
{
    public abstract class PassiveAbility : BaseAbility, ISubscribable
    {
        public abstract void Subscribe();

        public abstract void Unsubscribe();
    }
}
