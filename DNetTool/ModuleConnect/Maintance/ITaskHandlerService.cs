using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using DNet_DataContracts;

namespace DNet_Communication.Maintance
{
    public interface ITaskHandlerService
    {



        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <param name="requiredModule"></param>
        /// <returns></returns>
        Task SendTask(DNet_DataContracts.Processing.Task task, ModuleTypes requiredModule);
    }
}
