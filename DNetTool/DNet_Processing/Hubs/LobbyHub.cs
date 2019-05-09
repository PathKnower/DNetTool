using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using DNet_Communication.Connection;

using DNet_DataContracts;

namespace DNet_Processing.Hubs
{
    public class LobbyHub : Hub
    {
        public delegate void ConnectionEvent(ModuleTypes targetModule);
        public event ConnectionEvent ConnectionInitialized;
        public event ConnectionEvent DisconnectFromModule;

        private readonly IConnect _connectionInstance;
        private readonly ILogger<LobbyHub> _logger;
        private readonly IConfiguration _configuration;

        public LobbyHub(IConfiguration configuration, IConnect connectionInstance, ILogger<LobbyHub> logger)
        {
            _configuration = configuration;
            _connectionInstance = connectionInstance;
            _logger = logger;

            _connectionInstance.Disconnect += _connectionInstance_Disconnect;
            _connectionInstance.SuccessfullRegister += _connectionInstance_SuccessfullRegister;

            ConnectToHub().RunSynchronously();
        }

        private void _connectionInstance_SuccessfullRegister(string uri)
        {

            //throw new NotImplementedException();
        }

        private void _connectionInstance_Disconnect(string uri)
        {
            //throw new NotImplementedException();
        }

        private async Task ConnectToHub()
        {
            if (await _connectionInstance.Connect(_configuration.GetSection("ConnectionInfo")["PrimaryHubUri"], ModuleTypes.Manager))
            {
                
            } else if (await _connectionInstance.Connect(_configuration.GetSection("ConnectionInfo")["SecondaryHubUri"], ModuleTypes.Manager))
            {

            }
            _logger.LogCritical("Could not connect to any hub, looking for available hubs in network"); //TODO: Make this feature
        }
    }
}
