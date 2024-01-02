namespace Toji.Classes.API.Features.Characteristics
{
    public abstract class Characteristic<TValue>(TValue value) : BaseCharacteristic()
    {
        public TValue Value { get; protected internal set; } = value;
    }
}
