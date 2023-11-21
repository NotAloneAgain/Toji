using Exiled.API.Features;

namespace Toji.BetterWarhead.API.Features.Events
{
    public class BlackOut : BaseEvent
    {
        public override int Chance => 45;

        public override string Text => "Взрыв комплекса затронул аварийные генераторы поверхности, на починку неисправности уйдет много времени!";

        private protected override void Activate()
        {
            Map.TurnOffAllLights(600);
        }
    }
}
