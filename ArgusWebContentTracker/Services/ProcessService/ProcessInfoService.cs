using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ArgusWebContentTracker.Services.ProcessService
{
    public class ProcessInfoService : IProcessInfoService
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(int processAccess, bool bInheritHandle, int processId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("psapi.dll", SetLastError = true)]
        private static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, StringBuilder lpFilename, int nSize);

        private const int PROCESS_QUERY_INFORMATION = 0x0400;
        private const int PROCESS_VM_READ = 0x0010;

        public string GetExeNameFromProcessId(int processId)
        {
            IntPtr processHandle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, false, processId);

            if (processHandle == IntPtr.Zero)
            {
                throw new InvalidOperationException("Failed to open process with ID: " + processId);
            }

            try
            {
                StringBuilder exeName = new StringBuilder(1024);
                uint result = GetModuleFileNameEx(processHandle, IntPtr.Zero, exeName, exeName.Capacity);

                if (result == 0)
                {
                    throw new InvalidOperationException("Failed to get the exe name for process with ID: " + processId);
                }

                return exeName.ToString();
            }
            finally
            {
                CloseHandle(processHandle);
            }
        }
    }
}
