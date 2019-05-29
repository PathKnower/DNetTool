using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

using DNet_DataContracts;
using DNet_DataContracts.Maintance;

using DNet_Communication.Maintance;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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

            _hubConnection.On<string, string>("OnRegister", OnRegister);

            _hubConnection.On<DNet_DataContracts.Processing.Task>("TaskRecieve", OnTaskRecieve);

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

        private async void OnRegister(string result, string hubGuid)
        {
            if (result == "Ok")
            {
                SuccessfullRegister?.Invoke(hubGuid);
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

        private async void OnTaskRecieve(DNet_DataContracts.Processing.Task task)
        {
            TaskRecieve?.Invoke(task); //TODO: if there is no subscribers, create a new one or send a callback
        }

        #endregion

        #region Inteface Implementation

        public event ConnectionHandler SuccessfullRegister;
        public event ConnectionHandler ConnectionRestored;
        public event ConnectionHandler Disconnect;
        public event TaskTransmitHandler TaskRecieve;

        public bool IsConnected { get { return _hubConnection?.State == HubConnectionState.Connected;} }


        public async Task<bool> Connect(string connectionUri, ModuleTypes moduleType)
        {
            if (_hubConnection == null)
                await Initialize(connectionUri);

            await Register(moduleType);

            return true;
        }




        public async Task<bool> SendToHub(string methodName, object arg)
        {
            if(!IsConnected)
            {
                //Handle connection
                return false;
            }

            _logger.LogDebug($"{GetCallerName()} send to \'{methodName}\'");
            await _hubConnection.InvokeAsync(methodName, arg);

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

        private static string GetCallerName([CallerMemberName]string name = "")
        {
            return name + " Action:";
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
