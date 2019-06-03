using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using DNet_DataContracts;
using DNet_DataContracts.Processing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Task = System.Threading.Tasks.Task;

namespace DNet_Communication.Connection
{
    public class ConnectionService : IConnectionService
    {
        private ILogger<ConnectionService> _logger;
        private IConfiguration _configuration;

        private IConnect _connectionInstance;
        private string _currentModuleType;

        private Timer _connectionTimer;


        public ConnectionService(IConfiguration configuration, IConnect connectionInstance, ILogger<ConnectionService> logger)
        {
            _configuration = configuration;
            _connectionInstance = connectionInstance;
            _logger = logger;

            EventSubscribe();
        }


        public void ScheduleConnectionInitialize(TimeSpan interval, string moduleType)
        {
            _connectionTimer = new Timer
            {
                AutoReset = false, //trigger only once 
                Interval = interval.TotalMilliseconds
            };

            _connectionTimer.Elapsed += _connectionTimer_InitialConnect;
            _currentModuleType = moduleType;

            _connectionTimer.Start();
        }

        private void _connectionTimer_InitialConnect(object sender, ElapsedEventArgs e)
        {
            if(!_connectionInstance.IsConnected)
            {
                ConnectToHub().Wait();

                if (_connectionInstance.IsConnected)
                {
                    _connectionTimer.Stop();
                    _connectionTimer.Dispose();
                    _connectionTimer = null;
                }
                else
                    _connectionTimer.Start();
            }
        }

        #region Events 

        public event ConnectionHandler SuccessfullRegister;
        public event ConnectionHandler ConnectionRestored;
        public event ConnectionHandler Disconnect;

        private void _connectionInstance_SuccessfullRegister(string HubGUID)
        {
            SuccessfullRegister?.Invoke(HubGUID);
            _logger.LogInformation("Successfull register on hub with GUID: \'{0}\', ConnectionID: {1}", HubGUID, _connectionInstance.ConnectionId);

        }

        private void _connectionInstance_ConnectionRestored(string HubGUID)
        {
            //throw new NotImplementedException();
        }

        private void _connectionInstance_Disconnect(string HubGUID)
        {
            Disconnect?.Invoke(HubGUID);

            _logger.LogWarning("Disconnected from hub: \'{0}\'. Initialize reconnection timer.", HubGUID);

            if(_connectionTimer == null)
            {
                _connectionTimer = new Timer
                {
                    Interval = TimeSpan.FromSeconds(5).TotalMilliseconds,
                    AutoReset = true,
                    Enabled = true
                };

                _connectionTimer.Elapsed += _connectionTimer_Reconnect;

                _connectionTimer.Start();
            }
        }

        private void _connectionTimer_Reconnect(object sender, ElapsedEventArgs e)
        {
            if (!_connectionInstance.IsConnected)
            {
                ConnectToHub().Wait();

                if (_connectionInstance.IsConnected)
                {
                    _connectionTimer.Stop();
                    _connectionTimer.Dispose();
                    _connectionTimer = null;
                }
            }
        }

        #endregion

        #region Hub connection logic

        private async Task ConnectToHub()
        {
            try
            {
                if (await _connectionInstance.Connect(_configuration.GetSection("ConnectionInfo")["PrimaryHubUri"], _currentModuleType))
                {
                    _logger.LogInformation($"Successfully connected to Primary Hub which located on: {_configuration.GetSection("ConnectionInfo")["PrimaryHubUri"]}");
                    //Console.WriteLine("Connected to first hub");
                }
                else if (await _connectionInstance.Connect(_configuration.GetSection("ConnectionInfo")["SecondaryHubUri"], _currentModuleType))
                {
                    _logger.LogInformation($"Successfully connected to Secondary Hub which located on: {_configuration.GetSection("ConnectionInfo")["SecondaryHubUri"]}");
                    //Console.WriteLine("Connected to second hub");
                }
            }
            catch(Exception e)
            {
                _logger.LogCritical("Could not connect to any hub, looking for available hubs in network"); //TODO: Make this feature
            }
           
        }

        public void Dispose()
        {
            EventUnsubscribe();
            _connectionInstance.Dispose();
            _connectionTimer.Dispose();

            GC.SuppressFinalize(this);
        }

        #endregion


        #region Helpers

        private void EventSubscribe()
        {
            _connectionInstance.SuccessfullRegister += _connectionInstance_SuccessfullRegister;
            _connectionInstance.Disconnect += _connectionInstance_Disconnect;
            _connectionInstance.ConnectionRestored += _connectionInstance_ConnectionRestored;
        }

        private void EventUnsubscribe()
        {
            _connectionInstance.SuccessfullRegister -= _connectionInstance_SuccessfullRegister;
            _connectionInstance.Disconnect -= _connectionInstance_Disconnect;
            _connectionInstance.ConnectionRestored -= _connectionInstance_ConnectionRestored;
        }

        #endregion
    }
}
