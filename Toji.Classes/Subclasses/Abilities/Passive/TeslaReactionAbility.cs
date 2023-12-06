using Exiled.Events.EventArgs.Player;
using Exiled.Events.Handlers;
using Toji.Classes.API.Features.Abilities;

namespace Toji.Classes.Subclasses.Abilities.Passive
{
    public class TeslaReactionAbility : PassiveAbility
    {
        private static TeslaReactionAbility _isTriggerable;
        private static TeslaReactionAbility _isIgnored;

        private bool _isTrigger;

        private TeslaReactionAbility(bool trigger) : base()
        {
            _isTrigger = trigger;
        }

        public static TeslaReactionAbility IsTriggerable => _isTriggerable ??= new (true);

        public static TeslaReactionAbility IsIgnored => _isIgnored ??= new (false);

        public override string Name => "Особая реакция Тесла-Ворот";

        public override string Desc => $"Тесла-Ворота {(_isTrigger ? "всегда" : "никогда не")} реагируют на тебя";

        public override void Subscribe() => Player.TriggeringTesla -= OnTriggeringTesla;

        public override void Unsubscribe() => Player.TriggeringTesla -= OnTriggeringTesla;

        private void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            ev.IsTriggerable = _isTrigger;
            ev.IsAllowed = _isTrigger;
        }
    }
}
