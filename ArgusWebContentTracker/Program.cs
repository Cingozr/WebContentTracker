using ArgusWebContentTracker.Models;
using ArgusWebContentTracker.Services;
using ArgusWebContentTracker.Services.ProcessService;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;


namespace ArgusWebContentTracker
{
    public class Program
    {
        public static List<WebTabModel> WebTabModels = new List<WebTabModel>();


        public static async Task Main()
        {
            // Kanal ve İptal İşareti nesnelerini oluşturun
            BlockingCollection<string> channel = new BlockingCollection<string>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();


            IWindowProcessService windowProcessService = new WindowProcessService();
            IProcessInfoService processInfoService = new ProcessInfoService();
            IFlaUIReusableAppService iflaUIReusableAppService = new FlaUIReusableAppService(windowProcessService.GetActiveWindowProcessId);
            IWebBrowserTabService webBrowserTabService = new WebBrowserTabService(iflaUIReusableAppService);
            IProcessMonitorService processMonitorService = new ProcessMonitorService(windowProcessService, processInfoService, webBrowserTabService);

            Task taskStartMonitoringAsync = Task.Run(() => processMonitorService.StartMonitoringAsync(channel, cancellationTokenSource.Token));
            Task taskListenAsync = Task.Run(() => processMonitorService.ListenAsync(channel, cancellationTokenSource.Token));


            Console.WriteLine("Çıkmak için bir tuşa basın...");
            Console.ReadKey();
            cancellationTokenSource.Cancel();
            Task.WaitAll(taskStartMonitoringAsync, taskListenAsync);
        }


        static List<string> ActiveUrl(int processId)
        {
            List<string> activeUrls = new List<string>();
            var app = Application.Attach(processId);
            using (var automation = new UIA3Automation())
            {
                var mainWindow = app.GetMainWindow(automation);
                var cf = new ConditionFactory(new UIA3PropertyLibrary());
                var elements = mainWindow.FindAllDescendants(cf.ByName("Address and search bar"));
                foreach (var element in elements)
                    activeUrls.Add(element.Patterns.LegacyIAccessible.Pattern.Value);
            }

            return activeUrls;
        }


    }
}
