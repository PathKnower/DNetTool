﻿using System;
using System.Collections.Generic;
using System.Text;
using DNet_DataContracts.Maintance;
using ModuleConnect.Interfaces;

namespace ModuleConnect.Implements
{
    public class ServiceCollector : IServiceCollector
    {
        readonly PlatformID currentPlatform;
        MachineInfo machineInfo;

        public ServiceCollector()
        {
            currentPlatform = Environment.OSVersion.Platform;
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
                result += "free -m | grep Mem".Bash(); //total/used/free memory, parse "Mem:" and split by " " 
            }

            return result;
        }
        #endregion

        /// <summary>
        /// Get all physical RAM
        /// </summary>
        /// <returns></returns>
        public long GetTotalRAM()
        {
            string freeMemory;
            if (currentPlatform == PlatformID.Win32NT)
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
            if(currentPlatform == PlatformID.Win32NT)
            {
                freeMemory = "gwmi Win32_OperatingSystem | fl *FreePhysicalMemory*".PowerShell().Split(":")[1];
            }
            else
                freeMemory = "free -m | grep Mem".Bash().Replace("Mem:", "").Split(' ', StringSplitOptions.RemoveEmptyEntries)[2];

            if (long.TryParse(freeMemory, out long result))
                return currentPlatform == PlatformID.Win32NT? result/1024 : result;
            return -1;
        }

        /// <summary>
        /// Get standart CPU Clock in MHz
        /// </summary>
        /// <returns></returns>
        public int GetCPUClock()
        {
            string cpuInfo;
            if (currentPlatform == PlatformID.Win32NT)
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

        public CPUArchitectures GetCPUArchitecture()
        {



            return CPUArchitectures.x86;
        }

        public MachineInfo GetMachineInfo()
        {
            if (machineInfo == null)
                machineInfo = new MachineInfo();

            machineInfo.AllMemory = GetTotalRAM();
            machineInfo.AvailableMemory = GetFreeRAM();
            machineInfo.CPUClock = GetCPUClock();

            return machineInfo;
        }
    }
}