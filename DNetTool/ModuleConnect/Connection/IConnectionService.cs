using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using DNet_DataContracts;

namespace DNet_Communication.Connection
{
    public delegate void TaskHandler(); 

    public interface IConnectionService : IDisposable
    {


        void ScheduleConnectionInitialize(TimeSpan interval, ModuleTypes moduleType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <param name="requiredModule"></param>
        /// <returns></returns>
        Task SendTask(DNet_DataContracts.Processing.Task task, ModuleTypes requiredModule);
    }
}
