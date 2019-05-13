using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using DNet_DataContracts;
using DNet_DataContracts.Maintance;

namespace DNet_Communication.Connection
{

    public interface IConnect : IDisposable
    {
        event ConnectionHandler SuccessfullRegister;
        event ConnectionHandler ConnectionRestored;
        event ConnectionHandler Disconnect;

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //Task<string[]> HubSearching(string adressPool);
    }
}
