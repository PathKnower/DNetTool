using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DNet_DataContracts.Processing
{
    public class ProcessingTaskContext : TaskContext
    {



        IFormFile[] Files { get; set; }

        //TODO: define this
    }
}
