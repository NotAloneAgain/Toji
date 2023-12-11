using Exiled.API.Features;

namespace Toji.BetterWarhead.API.Features.Events
{
    public class BlackOut : BaseEvent
    {
        public override int Chance => 43;

        public override string Text => "Взрыв затронул аварийные генераторы поверхности, починка займет 10 минут!";

        private protected override void Activate()
        {
            Map.TurnOffAllLights(600);
        }
    }
}
