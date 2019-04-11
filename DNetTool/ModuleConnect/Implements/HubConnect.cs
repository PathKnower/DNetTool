using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using ModuleConnect.Interfaces;

using DNet_DataContracts;
using DNet_DataContracts.Maintance;

namespace ModuleConnect.Implements
{
    public class HubConnect : IConnect
    {
        public delegate void ConnectionHandle(string uri);
        public event ConnectionHandle SuccessfullRegister;
        public event ConnectionHandle FailedRegister;

        ILogger<HubConnect> _logger;
        private string _connectUri;
        private IMachineInfoCollectorService _serviceCollector;

        HubConnection hubConnection;

        public HubConnect(ILogger<HubConnect> logger, IMachineInfoCollectorService serviceCollector)
        {
            _logger = logger;
            _serviceCollector = serviceCollector;
        }

        void Initialize(string connectionUri)
        {
            _connectUri = connectionUri;

            hubConnection = new HubConnectionBuilder()
                .WithUrl(_connectUri)
                .Build();

            hubConnection.On<string>("OnRegister", OnRegister);

            hubConnection.On("CollectMachineInfo", GetMachineInfo);
            hubConnection.On("UpdateMachineLoad", GetMachineLoad);

            hubConnection.StartAsync();
        }



        #region RegisterModule logic
        public async Task Register(ModuleTypes moduleType)
        {
            try
            {
                await hubConnection.InvokeAsync("RegisterModule", moduleType);
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"Connect: Error occure the register module. Module type = {moduleType}");
            }
        }

        private async void OnRegister(string result)
        {
            if (result == "Ok")
            {
                SuccessfullRegister(_connectUri);
                return;
            }
            else
            {
                FailedRegister(_connectUri);
                return;
            }
        }

        #endregion

        #region Server handlers

        private async void GetMachineInfo() => await CollectMachineInfo();

        private async void GetMachineLoad() => await UpdateMachineLoad();

        #endregion

        #region Inteface Implementation

        public async Task Connect(string connectionUri, ModuleTypes moduleType)
        {
            if (hubConnection == null)
                Initialize(connectionUri);

            await Register(moduleType);
        }

        public async Task CollectMachineInfo()
        {
            try
            {
                MachineInfo machineInfo = await _serviceCollector.GetMachineInfo();
                await hubConnection.InvokeAsync("RecieveMachineInfo", machineInfo);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Connect: Error occure collect machine info.");
            }
        }


        public async Task UpdateMachineLoad()
        {
            try
            {
                //TODO: work with this
            }
            catch(Exception e)
            {

            }
        }

        #endregion
    }
}
