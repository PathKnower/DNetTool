﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DNet_Hub.Processing
{
    public interface ILoadBalancerService
    {
        /// <summary>
        /// Select module to send the task
        /// </summary>
        /// <returns></returns>
        Task SelectTargetModule();

    }
}
