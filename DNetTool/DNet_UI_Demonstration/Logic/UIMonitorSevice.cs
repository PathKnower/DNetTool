using DNet_Communication.Connection;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace DNet_UI_Demonstration.Logic
{
    public class UIMonitorSevice : ComponentBase
    {
        [Inject]
        internal IConnect _connectionInstance { get; set; }

        [Parameter]
        internal Data.ModuleWrapper[] Modules { get; set; }

        bool moduleVisibility = false;
        internal string hideStyle = "display:none;";

        private Timer timer;

        public UIMonitorSevice() { }

        internal async void BeginMonitor()
        {
            await _connectionInstance.HubRequest("ShareModuleInfo");
            timer.Start();
        }

        internal async void RequestInfo()
        {
            await _connectionInstance.HubRequest("ShareModuleInfo");
        }

        protected override void OnInit()
        {
            base.OnInit();

            (_connectionInstance as UIConnect).ModuleInfoRecieved += _connectionInstance_ModuleInfoRecieved;
            timer = new Timer
            {
                AutoReset = true,
                Interval = TimeSpan.FromSeconds(5).TotalMilliseconds
            };

            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            RequestInfo();
        }

        private void _connectionInstance_ModuleInfoRecieved(Data.ModuleWrapper[] modules)
        {
            Modules = null;
            Modules = modules;
            Invoke(StateHasChanged);
        }

    }
}
