using Hints;

namespace Toji.Hud.API.Features
{
    public static class Constants
    {
        public const string HintSkeleton = "<voffset=8.5em><line-height=95%><size=95%>[Top][AfterTop][Center][AfterCenter]";
        public const int CenterLinesPadding = 9;
        public const int TopLinesPadding = 16;
        public const float UpdateTime = 0.5f;

        public static HintEffect[] Effects { get; } = [HintEffectPresets.PulseAlpha(0.68f, 1, 1)];
    }
}
