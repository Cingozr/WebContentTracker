using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArgusWebContentTracker.Services.ProcessService
{
    public class ProcessMonitorService : IProcessMonitorService
    {

        private readonly IWindowProcessService _windowProcessService;
        private readonly IProcessInfoService _processInfoService;
        private readonly IWebBrowserTabService _webBrowserTabService;

        public ProcessMonitorService(IWindowProcessService windowProcessService, IProcessInfoService processInfoService, IWebBrowserTabService webBrowserTabService)
        {
            _windowProcessService = windowProcessService;
            _processInfoService = processInfoService;
            _webBrowserTabService = webBrowserTabService;
        }

        public async Task ListenAsync(BlockingCollection<string> _channel, CancellationToken _cancellationToken)
        {
            Console.WriteLine("ListenAsync...");
            try
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    string data = _channel.Take(_cancellationToken);
                    Console.WriteLine("Alınan veri: ");
                    Console.WriteLine(data);
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Dinleme iptal edildi.");
            }
        }

        public async Task StartMonitoringAsync(BlockingCollection<string> _channel, CancellationToken _cancellationToken)
        {
            Console.WriteLine("StartMonitoringAsync..."); 
            while (!_cancellationToken.IsCancellationRequested)
            {
                var currentPID = _windowProcessService.GetActiveWindowProcessId;
                var exeName = Path.GetFileName(_processInfoService.GetExeNameFromProcessId(currentPID));
                if (exeName == BrowserConstant.Chrome || exeName == BrowserConstant.Edge)
                {
                    var tabModels = await _webBrowserTabService.GetTabsAsync(currentPID);
                    var tabModelChannelFormat = JsonConvert.SerializeObject(tabModels, Formatting.Indented);
                    _channel.Add(string.Join(Environment.NewLine, tabModelChannelFormat));
                }

                // İzleme aralığını temsilen.
                Thread.Sleep(100);
            }
        }


    }
}
