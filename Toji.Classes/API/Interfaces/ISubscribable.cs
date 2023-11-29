using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toji.Classes.API.Interfaces
{
    public interface ISubscribable
    {
        void Subscribe();

        void Unsubscribe();
    }
}
