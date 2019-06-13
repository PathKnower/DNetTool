using System;
using System.Collections.Generic;
using System.Text;
using Threading = System.Threading.Tasks;


namespace DNet_DataContracts.Processing
{
    public class TaskContext
    {

        public long ApproximateResultTypeMemoryConsumption { get; set; }


        /// <summary>
        /// For demonstrate will use shared library.
        /// TODO: Use Json/XML reparsing on tartget modules
        /// </summary>
        public string Result { get; set; }
        
    }
}
