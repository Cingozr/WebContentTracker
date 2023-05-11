using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgusWebContentTracker.Services.ProcessService
{
    public interface IProcessInfoService
    {
        string GetExeNameFromProcessId(int processId);
    }
}
