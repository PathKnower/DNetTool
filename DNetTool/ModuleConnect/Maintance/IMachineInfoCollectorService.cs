using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DNet_DataContracts.Maintance;

namespace DNet_Communication.Maintance
{
    public interface IMachineInfoCollectorService
    {
        string GetFullCPUInfo();

        string GetFullRAMInfo();

        long GetTotalRAM();

        long GetFreeRAM();

        int GetCPUClock();

        CPUArchitectures GetCPUArchitecture();

        Task<MachineSpecifications> GetMachineInfo();
    }
}
