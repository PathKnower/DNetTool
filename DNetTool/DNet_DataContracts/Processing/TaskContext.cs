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


        public TaskType Type { get; set; }

        public Threading.Task Action { get; set; }

        /// <summary>
        /// Payload of the action. 
        /// </summary>
        public object Payload { get; set; }

        public object Result { get; set; }
        
    }
}
