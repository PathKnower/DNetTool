using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using DNet_Communication.Connection;
using DNet_Communication.Maintance;
using DNet_DataContracts.Processing;
using Microsoft.AspNetCore.Components;


namespace DNet_UI_Demonstration.Logic
{
    public class UITaskHandlerComponent : ComponentBase
    {
        [Inject]
        internal IUITaskHandlerService _taskHandlerService { get; set; }

        [Inject]
        internal IConnect _connectionInstance { get; set; }

        [Inject]
        internal IUITypeEvaluateService uITypeEvaluateService { get; set; }

        [Parameter]
        internal DNet_DataContracts.Processing.Task[] Tasks { get; set; }

        internal string HubGUID = string.Empty;

        internal string SomeThing = string.Empty;

        private Timer timer;

        public UITaskHandlerComponent()
        { }

        protected override void OnInit()
        {
            base.OnInit();

            _taskHandlerService.ResultRecieve += _taskHandlerService_ResultRecieve;

            (_connectionInstance as UIConnect).TaskInfoRecieved += UITaskHandlerComponent_TaskInfoRecieved;

            timer = new Timer
            {
                AutoReset = true,
                Interval = TimeSpan.FromSeconds(5).TotalMilliseconds
            };
            timer.Elapsed += Timer_Elapsed;
        }

        private void UITaskHandlerComponent_TaskInfoRecieved(DNet_DataContracts.Processing.Task[] tasks)
        {
            Tasks = null;
            Tasks = tasks;
            Invoke(StateHasChanged);
        }

        internal async void BeginMonitor()
        {
            await _connectionInstance.HubRequest("ShareTaskInfo");
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            RequestTasks();
        }

        internal async void RequestTasks()
        {
            await _connectionInstance.HubRequest("ShareTaskInfo");
        }

        private void _taskHandlerService_ResultRecieve(DNet_DataContracts.Processing.Task task)
        {
            if(task.IsFinished)
            {
                Console.WriteLine($"Task with ID: \'{task.Id}\' successfully finished");
            }

            SomeThing = (string)task.SearchContext.Result;
            Invoke(StateHasChanged);

            //throw new NotImplementedException();
        }

        internal async void SendTask()
        {
            //DNet_DataContracts.Processing.Task task = new DNet_DataContracts.Processing.Task
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    Type = TaskType.Search,
            //    ModuleType = "DB, DB_Postgres"
            //};

            //SearchTaskContext context = new SearchTaskContext
            //{
            //    SearchAlias = "user"
            //};
            //task.SearchContext = context;

            var task = new DNet_DataContracts.Processing.Task
            {
                Id = Guid.NewGuid().ToString(),
                Type = TaskType.Search,
                ModuleType = "DB, DB_Postgres"
            };

            SearchTaskContext context = new SearchTaskContext
            {
                SearchArea = "user"
            };

            if(uITypeEvaluateService.TypeMemoryConsumptionDictionary.TryGetValue("User", out long temp))
            {
                context.ApproximateResultTypeMemoryConsumption = temp;
            }

            task.SearchContext = context;

            await _taskHandlerService.SendTask(null);
        }

    }
}
