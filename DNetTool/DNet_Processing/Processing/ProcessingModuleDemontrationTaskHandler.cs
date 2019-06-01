using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DNet_Communication.Connection;
using DNet_Communication.Maintance;

namespace DNet_Processing.Processing
{
    public class ProcessingModuleDemontrationTaskHandler : BaseTaskHandlerService, IProcessingModuleDemontrationTaskHandler
    {
        public ProcessingModuleDemontrationTaskHandler(IConnect connectionInstance) : 
            base (connectionInstance)
        {
            TaskRecieve += ProcessingModuleDemontrationTaskHandler_TaskRecieve;
        }

        private void ProcessingModuleDemontrationTaskHandler_TaskRecieve(DNet_DataContracts.Processing.Task task)
        {
            Console.WriteLine(task.Id);
        }
    }
}
