using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

using DNet_Communication.Connection;
using Microsoft.Extensions.Logging;

namespace DNet_Manager.Hubs
{
    public class LobbyHub : Hub
    {
        public delegate void ConnectionEvent();
        public event ConnectionEvent ConnectionInitialized;

        private readonly IConnect _connectionInstance;
        private readonly ILogger<LobbyHub> _logger;
        private readonly IConfiguration _configuration;

        public LobbyHub(IConfiguration configuration, IConnect connectionInstance, ILogger<LobbyHub> logger)
        {
            _configuration = configuration;
            _connectionInstance = connectionInstance;
            _logger = logger;

            ConnectToHub().RunSynchronously();
        }

        private async Task ConnectToHub()
        {
            var connectTask = _connectionInstance.Connect(_configuration.GetSection("ConnectionInfo")["PrimaryHubUri"], DNet_DataContracts.ModuleTypes.Controller);
            if (!await connectTask)
            {
                connectTask = _connectionInstance.Connect(_configuration.GetSection("ConnectionInfo")["SecondaryHubUri"], DNet_DataContracts.ModuleTypes.Controller);
                if (!await connectTask)
                {
                    _logger.LogCritical("Could not connect to any hub, looking for available hubs in network"); //TODO: Make this feature
                }
            }
        }
    }
}
