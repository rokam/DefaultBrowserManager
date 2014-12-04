using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DefaultBrowserManager.Util
{
    public class User32Util
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        /// <summary>
        /// Get Foreground Window Process
        /// </summary>
        /// <returns></returns>
        public static Process GetForegroundWindowProcess()
        {
            uint pid = 0;
            GetWindowThreadProcessId(GetForegroundWindow(), out pid);
            return Process.GetProcessById((int)pid);
        }
    }
}
