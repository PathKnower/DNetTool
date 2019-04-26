using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using DNet_DataContracts;
using DNet_DataContracts.Maintance;

namespace DNet_Communication.Connection
{
    public delegate void ConnectionHandle(string uri);

    public interface IConnect
    {
        event ConnectionHandle SuccessfullRegister;
        event ConnectionHandle ConnectionRestored;
        event ConnectionHandle Disconnect;

        bool IsConnected { get; }

        /// <summary>
        /// Connect to the module 
        /// </summary>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        Task<bool> Connect(string connectionUri, ModuleTypes moduleType);

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
