using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using DNet_DataContracts;
using DNet_DataContracts.Maintance;

namespace ModuleConnect.Interfaces
{
    public interface IConnect
    {
        /// <summary>
        /// Connect to the Managed module
        /// </summary>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        Task Connect(string connectionUri, ModuleTypes moduleType);

        /// <summary>
        /// Collect full machine info, calculate Processing points
        /// </summary>
        /// <returns></returns>
        Task CollectMachineInfo();
        
        /// <summary>
        /// Collect actual machine load information
        /// </summary>
        /// <returns></returns>
        Task UpdateMachineLoad();
    }
}
