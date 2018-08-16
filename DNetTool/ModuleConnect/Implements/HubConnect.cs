using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using ModuleConnect.Interfaces;
using NLog;

namespace ModuleConnect.Implements
{
    public class HubConnect : IConnect
    {
        public delegate void ConnectionHandle(string uri);
        public event ConnectionHandle SuccessfullRegister;
        public event ConnectionHandle FailedRegister;

        private readonly Logger logger;
        private readonly string _connectUri;

        HubConnection hubConnection;

        public HubConnect(string connectUri)
        {
            logger = LogManager.GetCurrentClassLogger();

            _connectUri = connectUri;

            hubConnection = new HubConnectionBuilder()
                .WithUrl(_connectUri)
                .Build();

            hubConnection.On<string>("OnRegister", OnRegister);

            hubConnection.StartAsync();
        }

        #region RegisterModule logic
        public async Task Register(string moduleType)
        {
            try
            {
                await hubConnection.InvokeAsync("RegisterModule", moduleType);
            }
            catch(Exception e)
            {
                logger.Error(e, $"Error occure the refister module. Module type = {moduleType}");
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

        #region Inteface Implementation

        public async Task Connect(string moduleType)
        {
            await Register(moduleType);
        }

        #endregion
    }
}
