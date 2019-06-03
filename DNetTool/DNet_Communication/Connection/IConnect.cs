using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using DNet_Communication.Maintance;

using DNet_DataContracts;
using DNet_DataContracts.Maintance;

namespace DNet_Communication.Connection
{

    public interface IConnect : IDisposable
    {
        event ConnectionHandler SuccessfullRegister;
        event ConnectionHandler ConnectionRestored;
        event ConnectionHandler Disconnect;

        event TaskTransmitHandler TaskRecieve;
        event TaskTransmitHandler ResultRecieve;

        bool IsConnected { get; }

        string ConnectionId { get; }

        string ModuleType { get; }

        /// <summary>
        /// Connect to the module 
        /// </summary>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        Task<bool> Connect(string connectionUri, string moduleType);

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
        /// Unificated method to transmit data to the Hub
        /// </summary>
        /// <param name="methodName">Handler name on Hub</param>
        /// <param name="args">Argument</param>
        /// <returns></returns>
        Task<bool> SendToHub(string methodName, object arg);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //Task<string[]> HubSearching(string adressPool);
    }
}
