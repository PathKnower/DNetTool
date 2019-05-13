using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DNet_DataContracts;
using DNet_DataContracts.Processing;

namespace DNet_Communication.Maintance
{
    public class TaskHandlerService : ITaskHandlerService
    {
        public System.Threading.Tasks.Task SendTask(DNet_DataContracts.Processing.Task task, ModuleTypes requiredModule)
        {
            throw new NotImplementedException();
        }
    }
}
