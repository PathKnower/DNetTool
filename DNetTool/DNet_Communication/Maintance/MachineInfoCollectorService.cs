using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using DNet_DataContracts.Maintance;
using Microsoft.Extensions.Logging;


namespace DNet_Communication.Maintance
{
    public class MachineInfoCollectorService : IMachineInfoCollectorService
    {
        private readonly PlatformID _currentPlatform;
        private MachineSpecifications _machineInfo;
        ILogger<MachineInfoCollectorService> _logger;

        private bool _disposed;

        public MachineInfoCollectorService(ILogger<MachineInfoCollectorService> logger)
        {
            _currentPlatform = Environment.OSVersion.Platform;
            _logger = logger;
        }

        #region Diagnostics function

        //TODO: Rework to get more info

        /// <summary>
        /// Get full cpu info. return format depence on OS
        /// </summary>      
        /// <returns></returns>
        public string GetFullCPUInfo()
        {
            string result = "";
            switch (_currentPlatform)
            {
                case PlatformID.Win32NT: //Windows
                    result += "gwmi win32_Processor".PowerShell(); //get full CPU info
                    break;
                default: //Linux, MacOS
                    result += "lscpu".Bash();
                    break;
            }

            _logger.LogInformation("GetFullCPUInfo: Get all CPU Info:\n" + result);
            return result;
        }

        /// <summary>
        /// Get Total installed RAM and 
        /// </summary>
        /// <returns></returns>
        public string GetFullRAMInfo()
        {
            string result = "";
            if (_currentPlatform == PlatformID.Win32NT)
            {
                result += "systeminfo | Select-String 'Total Physical Memory:'".PowerShell(); //all memory in mb, replace 'Total Physical Memory:' and 'MB' 
                result += "gwmi Win32_OperatingSystem | fl *free*".PowerShell(); //FreePhysicalMemory in kb
            }
            else
            {
                result += "free -m | grep Mem".Bash(); //total/used/free memory, parse "Mem:" and split by " " 
            }

            _logger.LogInformation("GetFullRAMInfo: Get all RAM Info:\n" + result);
            return result;
        }

        /// <summary>
        /// Get all physical RAM in MB
        /// </summary>
        /// <returns></returns>
        public long GetTotalRAM()
        {
            string freeMemory;
            if (_currentPlatform == PlatformID.Win32NT)
            {
                freeMemory = "systeminfo | Select-String 'Total Physical Memory:'".PowerShell()
                    .Replace("Total Physical Memory:", "").Replace("MB", "").Replace(",", "");
            }
            else
                freeMemory = "free -m | grep Mem".Bash().Replace("Mem:", "").Split(' ', StringSplitOptions.RemoveEmptyEntries)[0];

            if (long.TryParse(freeMemory, out long result))
                return result;
            return -1;
        }

        /// <summary>
        /// Get available RAM in MB
        /// </summary>
        public long GetFreeRAM()
        {
            string freeMemory;
            if(_currentPlatform == PlatformID.Win32NT)
            {
                freeMemory = "gwmi Win32_OperatingSystem | fl *FreePhysicalMemory*".PowerShell().Split(":")[1];
            }
            else
                freeMemory = "free -m | grep Mem".Bash().Replace("Mem:", "").Split(' ', StringSplitOptions.RemoveEmptyEntries)[2];

            if (long.TryParse(freeMemory, out long result))
                return _currentPlatform == PlatformID.Win32NT? result/1024 : result;
            return -1;
        }

        /// <summary>
        /// Get standart CPU Clock in MHz
        /// </summary>
        /// <returns></returns>
        public int GetCPUClock()
        {
            string cpuInfo;
            if (_currentPlatform == PlatformID.Win32NT)
            {
                cpuInfo = "gwmi win32_Processor | fl *MaxClockSpeed* ".PowerShell();  
            }
            else
                cpuInfo = "lscpu | grep 'CPU MHz'".Bash();

            cpuInfo = cpuInfo.Split(':')[1];

            if (int.TryParse(cpuInfo, out int result))
                return result;
            return -1;
        }

        /// <summary>
        /// Get CPU Architecture
        /// </summary>
        /// <returns></returns>
        public CPUArchitectures GetCPUArchitecture()
        {
            string cpuArchitecure;
            if (_currentPlatform == PlatformID.Win32NT)
            {
                cpuArchitecure = "$ENV:Processor_Architecture".PowerShell();
            }
            else
                cpuArchitecure = "lscpu | grep Architecture".Bash();

            if (cpuArchitecure.Contains("AMD64") || cpuArchitecure.Contains("x86_64"))
                return CPUArchitectures.x86_64;

            return CPUArchitectures.x86;
        }

        /// <summary>
        /// Get CPU load
        /// </summary>
        /// <returns></returns>
        public double GetCPULoad()
        {
            string cpuload;
            if (_currentPlatform == PlatformID.Win32NT)
            {
                cpuload = "gwmi win32_processor | select LoadPercentage  |fl".PowerShell();
                cpuload = cpuload.Split(':')[1];
            }
            else
            {
                cpuload = "grep \'cpu \' /proc/stat | awk \'{usage=($2+$4)*100/($2+$4+$5)} END {print usage}\'".Bash();
            }

            if (double.TryParse(cpuload, out double result))
                return result;
            return -1;
        }

        #endregion


        /// <summary>
        /// Collect all necessary information about machine and calculate Performance Points
        /// </summary>
        /// <returns></returns>
        public async Task<MachineSpecifications> GetMachineInfo()
        {
            if (_machineInfo == null)
                _machineInfo = new MachineSpecifications();

            _machineInfo.AllMemory = GetTotalRAM();

            _machineInfo.CPUClock = GetCPUClock();
            _machineInfo.CPUCores = Environment.ProcessorCount;
            _machineInfo.CPUArchitecture = GetCPUArchitecture();

            _machineInfo.CalculatePerformancePoints();

            return _machineInfo;
        }

        public async Task<MachineLoad> GetMachineLoad()
        {
            MachineLoad load = new MachineLoad
            {
                CPULoad = GetCPULoad(),
                MemoryLoad = 100.0 - (GetFreeRAM()/
                (_machineInfo.AllMemory / 100.0))
            };

            load.MemoryLoad = Math.Round(load.MemoryLoad, 1);

            return load;
        }

        #region Disposing

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            InternalDispose();
            GC.SuppressFinalize(this);
        }

        private void InternalDispose()
        {
            // nothing to do
        }


        #endregion
    }
}
