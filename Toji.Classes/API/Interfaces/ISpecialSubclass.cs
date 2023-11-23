using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.API.Interfaces
{
    public interface ISpecialSubclass
    {
        List<string> Groups { get; }

        List<string> Users { get; }
    }
}
