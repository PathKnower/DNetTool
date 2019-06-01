using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using DNet_DataContracts;

namespace DNet_Communication.Maintance
{
    public delegate void TaskHandler();

    public delegate void TaskTransmitHandler(DNet_DataContracts.Processing.Task task);

    public interface IBaseTaskHandlerService
    {

        event TaskTransmitHandler TaskRecieve;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <param name="requiredModule"></param>
        /// <returns></returns>
        Task SendTask(DNet_DataContracts.Processing.Task task);
    }
}
