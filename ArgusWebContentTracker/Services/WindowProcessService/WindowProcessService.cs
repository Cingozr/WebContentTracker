using System;
using System.Runtime.InteropServices;

namespace ArgusWebContentTracker.Services
{
    public class WindowProcessService : IWindowProcessService
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        public int GetActiveWindowProcessId => _getActiveWindowProcessId();

        private int _getActiveWindowProcessId()
        {
            IntPtr hWnd = GetForegroundWindow();
            if (hWnd == IntPtr.Zero)
            {
                throw new InvalidOperationException("Failed to get active window handle");
            }

            uint processId;
            GetWindowThreadProcessId(hWnd, out processId);

            if (processId == 0)
            {
                throw new InvalidOperationException("Failed to get active window process ID");
            }

            return (int)processId;
        }
    }
}
