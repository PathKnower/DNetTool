using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DNet_DataContracts.Processing;
using DNet_Hub.Communication;

namespace DNet_Hub.Processing
{
    public class TaskEvaluationAlghoritm : ITaskEvaluationAlghoritm
    {
        public async System.Threading.Tasks.Task EvaluateTask(DNet_DataContracts.Processing.Task task, List<ModuleHubWrapper> modules)
        {

            switch(task.Type)
            {
                case TaskType.Search:
                    await SearchTaskContextMiddleware(task);
                    break;

                case TaskType.Calculate:
                    task.PerformancePoints = (int)task.CalculateContext.ApproximateResultTypeMemoryConsumption;
                    break;

                case TaskType.DataRequest:
                    task.PerformancePoints = (int)task.DataRequestContext.ApproximateResultTypeMemoryConsumption;
                    break;

                default:
                    break;
            }



            await Helper.SortByMinDifference(task.PerformancePoints, modules);
        }

        public async System.Threading.Tasks.Task SearchTaskContextMiddleware(DNet_DataContracts.Processing.Task task)
        {
            task.PerformancePoints = (int)task.SearchContext.ApproximateResultTypeMemoryConsumption;

            if(task.SearchContext.SearchArea.ToLower().Contains("sql")) //Detecting direct sql querry
            {
                task.PerformancePoints += (int)(70 * (task.SearchContext.SearchExpression.Length / 100.0)); //add 70% of lenght

                task.PerformancePoints += await Helper.AccountingBrackets(task.SearchContext.SearchExpression);
            }
            else
            {
                task.PerformancePoints += task.SearchContext.SearchExpression.Length;

                task.PerformancePoints += await Helper.AccountingBrackets(task.SearchContext.SearchExpression);
            }

        }

    }

    class Helper
    {
        public async static Task<int> AccountingBrackets(string searchExpression)
        {
            int additionalPP = 0;

            return additionalPP;
        }

        public async static System.Threading.Tasks.Task SortByMinDifference(int Pp, List<ModuleHubWrapper> modules)
        {

        }
    }
}
