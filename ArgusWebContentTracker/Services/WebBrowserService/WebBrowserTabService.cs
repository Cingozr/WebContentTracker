using ArgusWebContentTracker.Models;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgusWebContentTracker.Services
{

    public class WebBrowserTabService : IWebBrowserTabService
    {
        private readonly IFlaUIReusableAppService _flaUIReusableApp;
        private List<WebTabModel> _webTabModels;
        public WebBrowserTabService(IFlaUIReusableAppService flaUIReusableApp)
        {
            _flaUIReusableApp = flaUIReusableApp;
            _webTabModels = new List<WebTabModel>();
        }

        public async Task<List<WebTabModel>> GetTabsAsync(int processId)
        {
            List<WebTabModel> tempWebTabModels = new List<WebTabModel>();

            await Task.Run(() =>
            {
                _flaUIReusableApp.SetProcessId(processId);
                var appInstance = _flaUIReusableApp.GetAppInstance();
                var automationInstance = _flaUIReusableApp.GetAutomation();
                var mainWindow = appInstance.GetMainWindow(automationInstance);
                var tabItems = mainWindow.FindAllDescendants(cf => cf.ByControlType(ControlType.TabItem));

                // TabItem'ları işleyin
                foreach (var tabItem in tabItems)
                {
                    if (!tabItem.IsOffscreen && tabItem.FrameworkAutomationElement.FrameworkId.ValueOrDefault == null)
                    {
                        tempWebTabModels.Add(new WebTabModel
                        {
                            IsSelected = tabItem.Patterns.SelectionItem.Pattern.IsSelected,
                            TabName = tabItem.Name
                        });
                    }
                }
            });

            if (_webTabModels.Any())
                _webTabModels.RemoveAll(x => !tempWebTabModels.Contains(x));
            else
                _webTabModels = tempWebTabModels;

            return _webTabModels;
        }

    }
}
