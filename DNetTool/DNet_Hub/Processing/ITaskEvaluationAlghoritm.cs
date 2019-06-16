using DNet_Hub.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DNet_Hub.Processing
{
    public interface ITaskEvaluationAlghoritm
    {
        Task EvaluateTask(DNet_DataContracts.Processing.Task task, List<ModuleHubWrapper> modules);
    }
}
