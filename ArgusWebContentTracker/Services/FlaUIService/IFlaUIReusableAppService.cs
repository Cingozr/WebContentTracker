using FlaUI.Core;
using FlaUI.UIA3;
using System;

namespace ArgusWebContentTracker.Services
{
    public interface IFlaUIReusableAppService : IDisposable
    {
        void SetProcessId(int processId);
        Application GetAppInstance();
        UIA3Automation GetAutomation();
    }
}
