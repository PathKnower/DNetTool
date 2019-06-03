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
        /// Time when task is begin executing by Module
        /// </summary>
        //public DateTime ExecutionStartTime { get; set; }

        /// <summary>
        /// Time when task ends execution by any reason
        /// </summary>
        //public DateTime ExecutionEndTime { get; set; }

        /// <summary>
        /// Time when task was pushed to the queue
        /// </summary>
        //public DateTime QueuedPushTime { get; set; }

        /// <summary>
        /// Recommended performance points for this tasks
        /// </summary>
        public int PerformancePoints;

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

        public string ModuleType { get; set; }

        /// <summary>
        /// Task context
        /// </summary>
        public SearchTaskContext SearchContext { get; set; }

        public ProcessingTaskContext CalculateContext { get; set; }

        public DataRequestTaskContext DataRequestContext { get; set; }

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
