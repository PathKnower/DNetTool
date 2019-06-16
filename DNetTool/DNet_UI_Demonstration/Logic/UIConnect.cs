using DNet_Communication.Connection;
using DNet_Communication.Maintance;
using DNet_UI_Demonstration.Data;

using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DNet_UI_Demonstration.Logic
{
    public class UIConnect : HubConnect, IUIConnect
    {

        public UIConnect(ILogger<HubConnect> logger, IMachineInfoCollectorService serviceCollector) : base(logger, serviceCollector)
        {
        }

        public event UIModuleDemonstarion ModuleInfoRecieved;

        protected override async Task Initialize(string connectionUri)
        {
            await base.Initialize(connectionUri);

            _hubConnection.On<ModuleWrapper[]>("RecieveModuleInfo", RecieveModuleInfo);
        }

        public override async Task<bool> Connect(string connectionUri, string moduleType)
        {
            if (_hubConnection == null)
                await this.Initialize(connectionUri); //used for create new handlers

            return await base.Connect(connectionUri, moduleType);
        }

        protected virtual async void RecieveModuleInfo(ModuleWrapper[] modules)
        {
            ModuleInfoRecieved?.Invoke(modules);
        }
    }
}
