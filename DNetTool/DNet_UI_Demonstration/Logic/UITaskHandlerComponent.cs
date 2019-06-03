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
            _taskHandlerService.ResultRecieve += _taskHandlerService_ResultRecieve;
        }

        private void _taskHandlerService_ResultRecieve(DNet_DataContracts.Processing.Task task)
        {
            if(task.IsFinished)
            {
                Console.WriteLine($"Task with ID: \'{task.Id}\' successfully finished");
            }

            SomeThing = (string)task.Context.Result;
            //throw new NotImplementedException();
        }

        private void _taskHandlerService_TaskRecieve(DNet_DataContracts.Processing.Task task)
        {
            SomeThing = task.Id;

        }

        internal async void SendTask()
        {
            DNet_DataContracts.Processing.Task task = new DNet_DataContracts.Processing.Task
            {
                Id = Guid.NewGuid().ToString(),
                //ExecutionEndTime = DateTime.MinValue,
                //ExecutionStartTime = DateTime.MinValue,
                //QueuedPushTime = DateTime.MinValue
            };

            SearchTaskContext context = new SearchTaskContext
            {
                Type = TaskType.Search,
                ModuleType = "DB, DB_Postgres",
                SearchAlias = "user"
            };
            task.Context = context;

            await _taskHandlerService.SendTask(task);
        }

    }
}
