using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DNet_Communication.Connection;
using DNet_DataContracts;
using DNet_DataContracts.Processing;

using Microsoft.Extensions.Logging;

using Task = System.Threading.Tasks.Task;

namespace DNet_Communication.Maintance
{
    /// <summary>
    /// Base implemetation of TaskHandlerService
    /// </summary>
    public class BaseTaskHandlerService : ITaskHandlerService
    {
        protected IConnect _connectionInstance;

        protected List<DNet_DataContracts.Processing.Task> _currentTasks;

        protected Queue<CombinedTask> _taskQueue;

        public BaseTaskHandlerService(IConnect connectionInstance)
        {
            _connectionInstance = connectionInstance;
            _taskQueue = new Queue<CombinedTask>();
            _currentTasks = new List<DNet_DataContracts.Processing.Task>();

            // _connectionInstance.TaskRecieve += TaskRecieved;
            TaskRecieve += TaskRecieved;
            // _connectionInstance.TaskRecieve += TaskRecieve;
        }


        #region Interface impl

        public event TaskTransmitHandler TaskRecieve;

        protected async void TaskRecieved(DNet_DataContracts.Processing.Task task)
        {
            _currentTasks.Add(task);
        }

        public async Task SendTask(DNet_DataContracts.Processing.Task task)
        {
            await _connectionInstance.SendToHub("TaskReciever", task);
        }

        #endregion

    }
}
