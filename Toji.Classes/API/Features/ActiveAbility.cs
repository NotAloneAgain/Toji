using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toji.Classes.API.Interfaces;

namespace Toji.Classes.API.Features
{
    public abstract class ActiveAbility : BaseAbility, ISubscribable
    {
        public abstract void Subscribe();

        public abstract void Unsubscribe();

        public abstract void Activate(Player player);
    }
}
