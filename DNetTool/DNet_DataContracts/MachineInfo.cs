using System;
using System.Collections.Generic;
using System.Text;


namespace DNet_DataContracts
{
    public class MachineInfo
    {
        /// <summary>
        /// Represent CPU Clock in MHz
        /// </summary>
        public int CPUClock { get; set; }

        /// <summary>
        /// Represent number of CPU Cores
        /// </summary>
        public int CPUCores { get; set; }

        /// <summary>
        /// Represent all memory in machine in MB
        /// </summary>
        public long AllMemory { get; set; }

        /// <summary>
        /// Represent available memory in machine in MB
        /// </summary>
        public long AvailableMemory { get; set; }

    }
}
