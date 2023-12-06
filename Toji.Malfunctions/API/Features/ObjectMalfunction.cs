namespace Toji.Malfunctions.API.Features
{
    public abstract class ObjectMalfunction<TObject> : BaseMalfunction where TObject : class
    {
        public TObject Object { get; private set; }

        public override void Activate(int duration)
        {
            Object = SelectObject();

            base.Activate(duration);
        }

        public override void Deactivate()
        {
            base.Deactivate();

            Object = null;
        }

        protected abstract TObject SelectObject();
    }
}
