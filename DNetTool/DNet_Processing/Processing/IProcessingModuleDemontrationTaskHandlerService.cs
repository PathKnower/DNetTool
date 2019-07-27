using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DNet_Communication.Connection;
using DNet_Communication.Maintance;

namespace DNet_Processing.Processing
{
    interface IProcessingModuleDemontrationTaskHandlerService : IBaseTaskHandlerService
    {
        void Initialize();

    }
}
