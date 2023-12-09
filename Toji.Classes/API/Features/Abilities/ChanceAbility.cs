using UnityEngine;

namespace Toji.Classes.API.Features.Abilities
{
    public abstract class ChanceAbility : PassiveAbility
    {
        public ChanceAbility(int chance) => Chance = chance;

        public int Chance { get; }

        public bool GetRandom() => GetRandom(Chance);

        public bool GetRandom(int chance) => IsEnabled && Random.Range(0, 100) < chance;
    }
}
