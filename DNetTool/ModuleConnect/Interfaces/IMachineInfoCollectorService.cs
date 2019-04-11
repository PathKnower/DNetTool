using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DNet_DataContracts.Maintance;

namespace ModuleConnect.Interfaces
{
    public interface IMachineInfoCollectorService
    {
        string GetFullCPUInfo();

        string GetFullRAMInfo();

        long GetTotalRAM();

        long GetFreeRAM();

        int GetCPUClock();

        CPUArchitectures GetCPUArchitecture();

        Task<MachineInfo> GetMachineInfo();
    }
}
