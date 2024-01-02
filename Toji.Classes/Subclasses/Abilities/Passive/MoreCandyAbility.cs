using Exiled.Events.EventArgs.Scp330;
using Exiled.Events.Handlers;
using Toji.Classes.API.Features.Abilities;
using Toji.ExiledAPI.Extensions;

namespace Toji.Classes.Subclasses.Abilities.Passive
{
    public class MoreCandyAbility(int maxUsages) : PassiveAbility
    {
        public override string Name => "Больше конфет?";

        public override string Desc => $"Ты можешь взять до {maxUsages + 1} конфет SCP-330";

        public override void Subscribe() => Scp330.InteractingScp330 += OnInteractingScp330;

        public override void Unsubscribe() => Scp330.InteractingScp330 -= OnInteractingScp330;

        private void OnInteractingScp330(InteractingScp330EventArgs ev)
        {
            if (!IsEnabled || !ev.IsAllowed || !ev.IsValid() || !Has(ev.Player))
            {
                return;
            }

            ev.ShouldSever = ev.UsageCount > maxUsages;

            if (ev.ShouldSever)
            {
                ev.IsAllowed = false;
            }
        }
    }
}
