namespace Toji.Malfunctions.API.Features
{
    public abstract class ObjectMalfunction<TObject> : BaseMalfunction where TObject : class
    {
        public TObject Value { get; private protected set; }

        public override void Activate(int duration) => Value = SelectObject();

        public abstract TObject SelectObject();
    }
}
