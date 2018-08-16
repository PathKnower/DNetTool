using System;
using System.Collections.Generic;
using System.Text;


namespace DNet_DataContracts.Maintance
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
        /// Represent CPU Architecture realtes to calculate PP as coefficent
        /// </summary>
        public CPUArchitectures CPUArchitecture { get; set; }

        /// <summary>
        /// Represent all memory in machine in MB
        /// </summary>
        public long AllMemory { get; set; }

        /// <summary>
        /// Represent available memory in machine in MB
        /// </summary>
        public long AvailableMemory { get; set; }

        /// <summary>
        /// Performance points of current machine
        /// </summary>
        public int PerformancePoint { get; set; }
        
        //Hard drive: Maybe, in future 
    }
}
