namespace Toji.Classes.API.Interfaces
{
    public interface ICustomHintSubclass : IHintSubclass
    {
        string HintText { get; }

        string HintColor { get; }

        float HintDuration { get; }
    }
}
