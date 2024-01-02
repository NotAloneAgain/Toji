using UnityEngine;

namespace Toji.Classes.API.Features.Abilities
{
    public abstract class ChanceAbility(int chance) : PassiveAbility
    {
        public int Chance { get; } = chance;

        public bool GetRandom() => GetRandom(Chance);

        public bool GetRandom(int chance) => IsEnabled && Random.Range(0, 100) < chance;
    }
}
