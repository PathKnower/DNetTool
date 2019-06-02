using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DNet_DataContracts.Processing;
using Microsoft.AspNetCore.Components;


namespace DNet_UI_Demonstration.Logic
{
    public class UITaskHandlerComponent : ComponentBase
    {
        [Inject]
        internal IUITaskHandlerService _taskHandlerService { get; set; }

        internal string HubGUID;

        internal string SomeThing;

        public UITaskHandlerComponent()
        {
            
        }

        protected override void OnInit()
        {
            base.OnInit();

            _taskHandlerService.TaskRecieve += _taskHandlerService_TaskRecieve;

        }

        private void _taskHandlerService_TaskRecieve(DNet_DataContracts.Processing.Task task)
        {
            SomeThing = task.Id;

        }

        internal async void SendTask()
        {
            DNet_DataContracts.Processing.Task task = new DNet_DataContracts.Processing.Task
            {
                Id = Guid.NewGuid().ToString()
            };

            TaskContext context = new TaskContext
            {
                Type = TaskType.DataRequest
                
            };
            task.Context = context;

            await _taskHandlerService.SendTask(task);
        }

    }
}
