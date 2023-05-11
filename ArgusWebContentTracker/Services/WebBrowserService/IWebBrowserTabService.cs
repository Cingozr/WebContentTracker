using ArgusWebContentTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgusWebContentTracker.Services
{
    public interface IWebBrowserTabService
    {
        Task<List<WebTabModel>> GetTabsAsync(int processId);
    }
}
