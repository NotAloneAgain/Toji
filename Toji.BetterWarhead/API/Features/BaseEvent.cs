using Exiled.API.Features;

namespace Toji.BetterWarhead.API.Features
{
    public abstract class BaseEvent
    {
        public abstract int Chance { get; }

        public abstract string Text { get; }

        public void Start()
        {
            Map.Broadcast(12, $"<color=#780000><b>Внимание всем!\n{Text}</b></color>");

            Activate();
        }

        private protected abstract void Activate();
    }
}
