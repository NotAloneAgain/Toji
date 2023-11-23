using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toji.Malfunctions.API.Interfaces
{
    public interface ICassieMalfunction
    {
        string WarningText { get; }

        string WarningSubtitles { get; }
    }
}
