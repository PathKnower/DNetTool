using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

using DNet_DataContracts;
using DNet_DataContracts.Maintance;

using DNet_Communication.Maintance;
using System.Collections.Generic;

namespace DNet_Communication.Connection
{
    public class HubConnect : IConnect
    {
        private readonly ILogger<HubConnect> _logger;
        private string _connectURI;
        private IMachineInfoCollectorService _serviceCollector;
        private bool _disposed;

        HubConnection _hubConnection;

        public HubConnect(ILogger<HubConnect> logger, IMachineInfoCollectorService serviceCollector)
        {
            _logger = logger;
            _serviceCollector = serviceCollector;
        }

        async Task Initialize(string connectionUri)
        {
            _connectURI = connectionUri;

            CheckConnectionURI(ref _connectURI);

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_connectURI)
                .Build();

            _hubConnection.Closed += HubConnection_Closed;

            _hubConnection.On<string>("OnRegister", OnRegister);

            _hubConnection.On("CollectMachineInfo", GetMachineInfo);
            _hubConnection.On("UpdateMachineLoad", GetMachineLoad);

            await _hubConnection.StartAsync();
        }

        private async Task HubConnection_Closed(Exception arg)
        {
            Disconnect?.Invoke(_connectURI);
        }

        #region RegisterModule logic
        public async Task Register(ModuleTypes moduleType)
        {
            try
            {
                await _hubConnection.InvokeAsync("RegisterModule", moduleType);
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
                SuccessfullRegister?.Invoke(_connectURI);
                return;
            }
            else
            {
                //FailedRegister(_connectUri); //Work on this cases
                return;
            }
        }

        #endregion

        #region Server handlers

        private async void GetMachineInfo() => await CollectMachineInfo();

        private async void GetMachineLoad() => await UpdateMachineLoad();

        #endregion

        #region Inteface Implementation

        public event ConnectionHandle SuccessfullRegister;
        public event ConnectionHandle ConnectionRestored;
        public event ConnectionHandle Disconnect;

        public bool IsConnected { get { return _hubConnection.State == HubConnectionState.Connected;} }


        public async Task<bool> Connect(string connectionUri, ModuleTypes moduleType)
        {
            if (_hubConnection == null)
                await Initialize(connectionUri);

            await Register(moduleType);

            return true;
        }

        public async Task CollectMachineInfo()
        {
            try
            {
                MachineSpecifications machineInfo = await _serviceCollector.GetMachineInfo();
                await _hubConnection.InvokeAsync("RecieveMachineInfo", machineInfo);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adressPool"></param>
        /// <returns></returns>
        //public Task<string[]> HubSearching(string adressPool)
        //{
            


        //    return null;
        //}

        #endregion

        #region Helpers

        private void CheckConnectionURI(ref string uri)
        {
            uri = uri.Replace(" ", ""); //remove all spaces

            if(!uri.Replace("http://", "").Contains(":")) //means that uri doesn't have port, use default port
            {
                uri += ":39286/mainhub";
            }
            //if uri has ports, do nothing
        }


        #endregion

        #region Disposing

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            InternalDispose();
            GC.SuppressFinalize(this);
        }

        private void InternalDispose()
        {
            _hubConnection.DisposeAsync();
            _serviceCollector.Dispose();
        }

        #endregion
    }
}
