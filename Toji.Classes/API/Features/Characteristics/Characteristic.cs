namespace Toji.Classes.API.Features.Characteristics
{
    public abstract class Characteristic<TValue> : BaseCharacteristic
    {
        public Characteristic(TValue value) : base()
        {
            Value = value;
        }

        public TValue Value { get; protected internal set; }
    }
}
