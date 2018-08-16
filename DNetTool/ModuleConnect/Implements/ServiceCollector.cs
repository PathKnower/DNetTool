using System;
using System.Collections.Generic;
using System.Text;
using DNet_DataContracts.Maintance;
using ModuleConnect.Interfaces;

namespace ModuleConnect.Implements
{
    public class ServiceCollector : IServiceCollector
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
                    result += "gwmi win32_Processor".PowerShell(); //get full CPU info
                    break;
                default: //Linux, MacOS
                    result += "lscpu".Bash();
                    break;
            }
            return result;
        }

        /// <summary>
        /// Get Total installed RAM and 
        /// </summary>
        /// <returns></returns>
        public string GetFullRAMInfo()
        {
            string result = "";
            if (currentPlatform == PlatformID.Win32NT)
            {
                result += "systeminfo | Select-String 'Total Physical Memory:'".PowerShell(); //all memory in mb, replace 'Total Physical Memory:' and 'MB' 
                result += "gwmi Win32_OperatingSystem | fl *free*".PowerShell(); //FreePhysicalMemory in kb
            }
            else
            {
                result += "free -m | grep Mem".Bash(); //total/free memory, parse "Mem:" and split by " " 
            }

            return result;
        }

        public MachineInfo GetMachineInfo()
        {
            throw new NotImplementedException();
        }
    }
}
