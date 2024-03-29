﻿using DNet_Communication.Connection;
using DNet_UI_Demonstration.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DNet_UI_Demonstration.Logic
{
    public delegate void UIModuleDemonstarion(ModuleWrapper[] modules);
    public delegate void UITaskDemonstration(DNet_DataContracts.Processing.Task[] tasks);

    public interface IUIConnect : IConnect
    {
        event UIModuleDemonstarion ModuleInfoRecieved;
        event UITaskDemonstration TaskInfoRecieved;
    }
}
