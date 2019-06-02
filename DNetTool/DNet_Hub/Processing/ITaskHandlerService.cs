using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DNet_Hub.Processing
{
    public interface ITaskHandlerService
    {

        Task TaskEvaluate(DNet_DataContracts.Processing.Task task);

        
    }
}
