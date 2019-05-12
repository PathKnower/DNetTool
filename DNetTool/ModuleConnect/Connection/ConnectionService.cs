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
        private ModuleTypes _currentModuleType;

        private Timer _connectionTimer;

        public ConnectionService(IConfiguration configuration, IConnect connectionInstance, ILogger<ConnectionService> logger)
        {
            _configuration = configuration;
            _connectionInstance = connectionInstance;
            _logger = logger;
        }


        public void ScheduleConnectionInitialize(TimeSpan interval, ModuleTypes moduleType)
        {
            _connectionTimer = new Timer
            {
                AutoReset = false, //trigger only once
                Interval = interval.TotalMilliseconds
            };

            _connectionTimer.Elapsed += _connectionTimer_Elapsed;
            _currentModuleType = moduleType;

            _connectionTimer.Start();
        }

        private void _connectionTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(!_connectionInstance.IsConnected)
            {
                ConnectToHub().Wait();
            }


        }

        public async Task SendTask(DNet_DataContracts.Processing.Task task, ModuleTypes requiredModule)
        {
            throw new NotImplementedException();
        }


        #region Hub connection logic

        private async Task ConnectToHub()
        {
            if (await _connectionInstance.Connect(_configuration.GetSection("ConnectionInfo")["PrimaryHubUri"], ModuleTypes.Processing))
            {
                _logger.LogInformation($"Successfully connected to Primary Hub which located on: {_configuration.GetSection("ConnectionInfo")["PrimaryHubUri"]}");
                //Console.WriteLine("Connected to first hub");
            }
            else if (await _connectionInstance.Connect(_configuration.GetSection("ConnectionInfo")["SecondaryHubUri"], ModuleTypes.Processing))
            {
                _logger.LogInformation($"Successfully connected to Secondary Hub which located on: {_configuration.GetSection("ConnectionInfo")["SecondaryHubUri"]}");
                //Console.WriteLine("Connected to second hub");
            }
            else
            {
                _logger.LogCritical("Could not connect to any hub, looking for available hubs in network"); //TODO: Make this feature
            }
        }

        public void Dispose()
        {
            _connectionInstance.Dispose();
            _connectionTimer.Dispose();

            GC.SuppressFinalize(this);
        }

        #endregion


        #region Helpers

        #endregion
    }
}
