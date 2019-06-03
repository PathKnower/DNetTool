﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DNet_Communication.Connection;
using DNet_Communication.Maintance;
using DNet_DataContracts.Processing;

namespace DNet_UI_Demonstration.Logic
{
    public class UITaskHandlerService : BaseTaskHandlerService, IUITaskHandlerService
    {
        IUITypeEvaluateService _typeEvaluateService;
        public event TaskTransmitHandler ResultRecieve;

        public UITaskHandlerService(IConnect connectionInstance, IUITypeEvaluateService typeEvaluateService)
            : base(connectionInstance)
        {
            _connectionInstance.ResultRecieve += UITaskHandlerService_ResultRecieve;
        }

        private void UITaskHandlerService_ResultRecieve(DNet_DataContracts.Processing.Task task)
        {
            ResultRecieve?.Invoke(task);
        }

        public override async System.Threading.Tasks.Task SendTask(DNet_DataContracts.Processing.Task task)
        {


            await base.SendTask(task);
        }
    }
}
