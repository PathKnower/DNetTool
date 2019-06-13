using System;
using System.Collections.Generic;
using System.Text;


namespace DNet_DataContracts.Maintance
{
    public class MachineSpecifications
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
        /// Full cpu power
        /// </summary>
        public long CPUPower { get; set; }

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
        public long PerformancePoint { get; set; }

        //TODO: Module load calculate

        /// <summary>
        /// Calculate CPU Power
        /// </summary>
        public void CalculateCPUPower()
        {
            long res = CPUClock * CPUCores;
            if (CPUArchitecture == CPUArchitectures.x86_64)
                res = (long)(res * 1.1);

            CPUPower = res;
        }

        /// <summary>
        /// Calculate Performance Points
        /// </summary>
        public void CalculatePerformancePoints()
        {
            CalculateCPUPower();

            double coefficent = CPUPower / AllMemory; //coefficent of CPU performancing 
            if (coefficent < 1 && CPUPower < 8000) //if CPU power less than 8000, then add some PP due the RAM 
            {
                coefficent = 100.0 - (CPUPower /(AllMemory / 100.0)); 
                PerformancePoint = CPUPower + (long)(AllMemory * (coefficent / 100.0));
            }
            else
                PerformancePoint = CPUPower + AllMemory;
        }
    }
}
