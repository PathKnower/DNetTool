using System;
using System.Collections.Generic;
using System.Text;

using DNet_DataContracts;

namespace DNet_DataContracts.Processing
{
    /// <summary>
    /// Represent information about Task inside the system.
    /// </summary>
    public class Task
    {
        //TODO: add aborting events/functions

        /// <summary>
        /// Unique identificator
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Recommended performance points for this tasks
        /// </summary>
        public int PerformancePoints { get; set; }

        /// <summary>
        /// Task requester
        /// </summary>
        public Module RequestedBy { get; set; }

        /// <summary>
        /// Task executor
        /// </summary>
        public Module Executor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TaskType Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ModuleType { get; set; }

        /// <summary>
        /// Using JSON for further interpretation
        /// </summary>
        public string CustomContext { get; set; }

        /// <summary>
        /// Task context
        /// </summary>
        public SearchTaskContext SearchContext { get; set; }

        public ProcessingTaskContext CalculateContext { get; set; }

        public DataRequestTaskContext DataRequestContext { get; set; }


        public Action TestAction { get; set; }

        /// <summary>
        /// Boolean which represent that is task finished
        /// </summary>
        public bool IsFinished { get; set; }

        /// <summary>
        /// Boolean which represent that is task executing
        /// </summary>
        public bool IsExecuting { get; set; }

        /// <summary>
        /// Boolean which represent that is task was canceled
        /// </summary>
        public bool IsCanceled { get; set; }
    }
}
