using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DNet_Communication.Connection;
using DNet_Communication.Maintance;
using DNet_DataContracts.Processing;

namespace DNet_UI_Demonstration.Logic
{
    public class UITaskHandlerService : BaseTaskHandlerService, IUITaskHandlerService
    {
        IUITypeEvaluateService _typeEvaluateService;
        public event TaskTransmitHandler ResultRecieve;

        public UITaskHandlerService(IConnect connectionInstance, IUITypeEvaluateService typeEvaluateService)
            : base(connectionInstance)
        {
            _connectionInstance.ResultRecieve += UITaskHandlerService_ResultRecieve;

            _typeEvaluateService = typeEvaluateService;
        }

        private void UITaskHandlerService_ResultRecieve(DNet_DataContracts.Processing.Task task)
        {
            ResultRecieve?.Invoke(task);
        }

        public override async System.Threading.Tasks.Task SendTask(DNet_DataContracts.Processing.Task task)
        {
            task = new DNet_DataContracts.Processing.Task
            {
                Id = Guid.NewGuid().ToString(),
                Type = TaskType.Search,
                ModuleType = "DB, DB_Postgres",
                //TestAction = new Action(() =>
                //{
                //    int x = 60, y = 10;

                //    Console.WriteLine(x + y);
                //    Console.WriteLine("Hello!");
                //})
            };

            SearchTaskContext context = new SearchTaskContext
            {
                SearchArea = "user",
                ApproximateResultTypeMemoryConsumption = _typeEvaluateService.TypeMemoryConsumptionDictionary["User"]
            };
            task.SearchContext = context;


            await base.SendTask(task);
        }

        private void SendCalculateTask()
        {
            DNet_DataContracts.Processing.Task task = new DNet_DataContracts.Processing.Task
            {

            };

            ProcessingTaskContext calculateTask = new ProcessingTaskContext
            {
                
            };
        }
    }
}
