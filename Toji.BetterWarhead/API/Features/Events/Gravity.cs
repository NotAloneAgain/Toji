using UnityEngine;

namespace Toji.BetterWarhead.API.Features.Events
{
    public class Gravity : BaseEvent
    {
        public override int Chance => 49;

        public override string Text => "Взрыв создал гравитационную аномалию, не выбрасывайте предметы!";

        private protected override void Activate() => Physics.gravity = new Vector3(GetRandomValue() - 0.5f, GetRandomValue(), GetRandomValue() - 0.5f);

        private float GetRandomValue() => Random.Range(-10, 16);
    }
}
