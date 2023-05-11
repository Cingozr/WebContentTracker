using FlaUI.Core;
using FlaUI.UIA3;
using System;

namespace ArgusWebContentTracker.Services
{
    public class FlaUIReusableAppService : IFlaUIReusableAppService
    {
        private int _currentProcessId;
        private Application _appInstance;
        private UIA3Automation _automation;

        public FlaUIReusableAppService(int processId)
        {
            _automation = new UIA3Automation();
            SetProcessId(processId);
        }

        public void SetProcessId(int processId)
        {
            if (_currentProcessId != processId)
            {
                // Eski uygulama örneğini serbest bırak
                _appInstance?.Dispose();

                // Yeni uygulama örneği oluştur ve bağlan
                _appInstance = Application.Attach(processId);

                // Mevcut processId'yi güncelle
                _currentProcessId = processId;
            }
        }

        public Application GetAppInstance()
        {
            return _appInstance;
        }

        public UIA3Automation GetAutomation()
        {
            return _automation;
        }

        public void Dispose()
        {
            _appInstance?.Dispose();
            _automation?.Dispose();
        }
    }
}
