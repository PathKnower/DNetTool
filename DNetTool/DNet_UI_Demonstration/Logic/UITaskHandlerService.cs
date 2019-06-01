using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DNet_Communication.Connection;
using DNet_Communication.Maintance;

namespace DNet_UI_Demonstration.Logic
{
    public class UITaskHandlerService : BaseTaskHandlerService, IUITaskHandlerService
    {
        public UITaskHandlerService(IConnect connectionInstance)
            : base(connectionInstance)
        {

        }

    }
}
