using System;
using System.Collections.Generic;
using System.Text;

namespace ModuleConnect
{
    class ServiceCollector
    {
        readonly PlatformID currentPlatform;

        public ServiceCollector()
        {
            currentPlatform = Environment.OSVersion.Platform;
        }

        /// <summary>
        /// Get full cpu info. return format depence on OS
        /// </summary>
        /// <returns></returns>
        public string GetFullCPUInfo()
        {
            string result = "";
            switch (currentPlatform)
            {
                case PlatformID.Win32NT: //Windows
                    result += "gwmi win32_Processor".PowerShell(); //get
                    break;
                default: //Linux, MacOS
                    result += "lscpu".Bash();
                    break;
            }
            return result;
        }
    }
}
