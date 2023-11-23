using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toji.Malfunctions.API.Features
{
    public abstract class ObjectMalfunction<TObject> : BaseMalfunction where TObject : class
    {
        public TObject Object { get; private set; }

        public override void Activate()
        {
            SelectObject();

            base.Activate();
        }

        public override void Deactivate()
        {
            base.Deactivate();

            Object = null;
        }

        public abstract void SelectObject();
    }
}
