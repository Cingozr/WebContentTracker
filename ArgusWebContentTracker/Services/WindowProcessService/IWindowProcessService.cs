using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgusWebContentTracker.Services
{
    public interface IWindowProcessService
    {
        int GetActiveWindowProcessId { get; }
    }
}
