using System;
using System.Collections.Generic;
using System.Text;
using Threading = System.Threading.Tasks;


namespace DNet_DataContracts.Processing
{
    public class TaskContext
    {
        public delegate void TaskEvents();
        public event TaskEvents TaskComlete;

        public string ModuleType { get; set; }

        public TaskType Type { get; set; }

        public long ApproximateResultTypeMemoryConsumption { get; set; }


        /// <summary>
        /// Payload of the action. 
        /// </summary>
        public object Payload { get; set; }

        /// <summary>
        /// For demonstrate will use shared library.
        /// TODO: Use Json/XML reparsing on tartget modules
        /// </summary>
        public object Result { get; set; }
        
    }
}
