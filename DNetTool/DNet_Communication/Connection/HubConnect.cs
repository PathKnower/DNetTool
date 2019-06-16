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
using System.Timers;

namespace DNet_Communication.Connection
{
    public class HubConnect : IConnect
    {
        protected readonly ILogger<HubConnect> _logger;
        protected string _connectURI;
        protected IMachineInfoCollectorService _serviceCollector;
        protected bool _disposed;

        protected Timer _moduleLoadNotificationTimer;

        protected string _moduleType = string.Empty;
        protected string _hubGuid = string.Empty;

        protected HubConnection _hubConnection;


        public HubConnect(ILogger<HubConnect> logger, IMachineInfoCollectorService serviceCollector)
        {
            _logger = logger;
            _serviceCollector = serviceCollector;
        }

        protected virtual async Task Initialize(string connectionUri)
        {
            _connectURI = connectionUri;

            CheckConnectionURI(ref _connectURI);

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_connectURI)
                .Build();

            _hubConnection.Closed += HubConnection_Closed;

            _hubConnection.On<string, string>("OnRegister", OnRegister);

            _hubConnection.On<DNet_DataContracts.Processing.Task>("TaskRecieve", OnTaskRecieve);
            _hubConnection.On<DNet_DataContracts.Processing.Task>("ResultRecieve", OnResultRecieve);

            _hubConnection.On("CollectMachineInfo", GetMachineInfo);
            _hubConnection.On("UpdateMachineLoad", GetMachineLoad);

            _moduleLoadNotificationTimer = new Timer(TimeSpan.FromSeconds(5).TotalMilliseconds)
            {
                AutoReset = true
            };
            _moduleLoadNotificationTimer.Elapsed += _moduleLoadNotificationTimer_Elapsed;
        }



        #region RegisterModule logic

        public virtual async Task Register(string moduleType)
        {
            try
            {
                _moduleType = moduleType;
                await _hubConnection.InvokeAsync("RegisterModule", _moduleType);
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"Connect: Error occure the register module. Module type = {_moduleType}");
            }
        }

        protected virtual async void OnRegister(string result, string hubGuid)
        {
            if (result == "Ok")
            {
                _hubGuid = hubGuid;
                SuccessfullRegister?.Invoke(_hubGuid);
                _moduleLoadNotificationTimer.Start();
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

        protected virtual async void GetMachineInfo() => await CollectMachineInfo();

        protected virtual async void GetMachineLoad()
        {
            _moduleLoadNotificationTimer.Stop(); 
            await UpdateMachineLoad(); //refresh timer after manual call 
            _moduleLoadNotificationTimer.Start();
        }

        protected virtual async void OnTaskRecieve(DNet_DataContracts.Processing.Task task)
        {
            _logger.LogInformation("Recieve new task. Task ID: \'{0}\' ", task.Id);
            TaskRecieve?.Invoke(task); //TODO: if there is no subscribers, create a new one or send a callback
        }


        protected virtual async void OnResultRecieve(DNet_DataContracts.Processing.Task task)
        {
            _logger.LogInformation("Recieve result of task. Task ID: \'{0}\' ", task.Id);
            ResultRecieve?.Invoke(task);
        }



        #endregion

        #region Inteface Implementation

        public event ConnectionHandler SuccessfullRegister;
        public event ConnectionHandler ConnectionRestored;
        public event ConnectionHandler Disconnect;
        public event TaskTransmitHandler TaskRecieve;
        public event TaskTransmitHandler ResultRecieve;

        public bool IsConnected { get { return _hubConnection?.State == HubConnectionState.Connected;} }

        public string ConnectionId { get { return _hubConnection?.ConnectionId; } }

        public string ModuleType { get { return _moduleType; } }

        public virtual async Task<bool> Connect(string connectionUri, string moduleType)
        {
            if (_hubConnection == null)
                await Initialize(connectionUri);

            await _hubConnection.StartAsync();

            await Register(moduleType);

            return IsConnected;
        }

        public virtual async Task<bool> SendToHub(string methodName, object arg)
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

        public async Task HubRequest(string methodName)
        {
            if (!IsConnected)
            {
                //Handle connection
                return;
            }

            _logger.LogDebug($"{GetCallerName()} send to \'{methodName}\'");
            await _hubConnection.SendAsync(methodName);
        }


        public virtual async Task CollectMachineInfo()
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


        public virtual async Task UpdateMachineLoad()
        {
            try
            {
                MachineLoad load = await _serviceCollector.GetMachineLoad();
                await _hubConnection.InvokeAsync("RecieveModuleActivity", load);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Connect: Error occure collect machine load.");
            }
        }



        protected virtual async Task HubConnection_Closed(Exception arg)
        {
            Disconnect?.Invoke(_hubGuid);
            _moduleLoadNotificationTimer.Stop();
        }

        #endregion

        #region Helpers

        protected async void _moduleLoadNotificationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //_logger.LogDebug("Load Notidication Timer Elapsed!");
            await UpdateMachineLoad();
        }

        protected virtual void CheckConnectionURI(ref string uri)
        {
            uri = uri.Replace(" ", ""); //remove all spaces

            if(!uri.Replace("http://", "").Contains(":")) //means that uri doesn't have port, use default port
            {
                uri += ":39286/mainhub";
            }
            //if uri has ports, do nothing
        }

        protected static string GetCallerName([CallerMemberName]string name = "")
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
