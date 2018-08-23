using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ModuleConnect.Interfaces
{
    interface IConnect
    {
        /// <summary>
        /// Connect to the Managed module
        /// </summary>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        Task Connect(string connectionUri, string moduleType);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task CollectMachineInfo();

        Task UpdateMachineLoad();
    }
}
