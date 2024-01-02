namespace Toji.Global
{
    public readonly struct Range(int max)
    {
        public Range(int min, int max) : this(max) => Min = min;

        public int Max { get; init; } = max;

        public int Min { get; init; }

        public bool InRange(int value) => value <= Max && value >= Min;
    }
}
