using DNet_Hub.Communication;
using DNet_Hub.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DNet_Hub.Processing
{
    public class LoadBalancerService : ILoadBalancerService
    {
        ITaskEvaluationAlghoritm _taskEvaluationAlghoritm;

        public LoadBalancerService(ITaskEvaluationAlghoritm taskEvaluationAlghoritm)
        {
            _taskEvaluationAlghoritm = taskEvaluationAlghoritm;
        }

        public async Task SelectTargetModule(DNet_DataContracts.Processing.Task task, MainHub hubInstance)
        {
            var modules = MainHub.Modules
                .Where(x => task.ModuleType.Contains(x.TargedModule.ModuleType)) //determine comparible module types
                .OrderByDescending(x => x.TargedModule.ModulesHostSpecs.PerformancePoint).ToList(); //sort by performance points

            //var oldModuleInfo = modules.Where(x => (DateTime.Now - x.LastLoadUpdate).TotalSeconds > 30); //detect old info modules

            //await hubInstance.RequestLoadUpdate(oldModuleInfo); //Collect all load from potential modules

            //await _taskEvaluationAlghoritm.EvaluateTask(task, modules);

            //foreach(var oldModule in oldModuleInfo)
            //{
            //    oldModule.WaitTillGetNewLoadInfo();
            //}

            //modules = modules.OrderBy(x => x.Load.CPULoad).ToList();

            var module = modules.First();
            task.Executor = module.TargedModule;
            await hubInstance.SendTask(module.ConnectionId, task);
        }

    }
}
