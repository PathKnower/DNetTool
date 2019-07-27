using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DNet_Communication.Connection;
using DNet_Communication.Maintance;

namespace DNet_Processing.Processing
{
    public class ProcessingModuleDemontrationTaskHandlerService : BaseTaskHandlerService, IProcessingModuleDemontrationTaskHandlerService
    {
        public ProcessingModuleDemontrationTaskHandlerService(IConnect connectionInstance) : 
            base (connectionInstance)
        {
            
        }

        public void Initialize()
        {
            _connectionInstance.TaskRecieve += ProcessingModuleDemontrationTaskHandler_TaskRecieve;
        }

        private void ProcessingModuleDemontrationTaskHandler_TaskRecieve(DNet_DataContracts.Processing.Task task)
        {
            task.SearchContext.Result = "Processing result";

            task.IsFinished = true;

            _connectionInstance.SendToHub("TaskResult", task);
            //task.TestAction.Invoke();
        }
    }
}
