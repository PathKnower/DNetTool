using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DNet_DataContracts.Maintance;
using DNet_DataContracts;

using DNet_Hub.Communication;
using Microsoft.Extensions.Logging;

namespace DNet_Hub.Hubs
{
    public class MainHub : Hub
    {
        private static string HubGUID = null;

        public delegate void MachineHardwareEvent(string id, MachineSpecifications info);
        public event MachineHardwareEvent UpdatedMachineInfo;

        private ILogger<MainHub> _logger;
        
        public MainHub(ILogger<MainHub> logger)
        {
            _logger = logger;

            if(string.IsNullOrEmpty(HubGUID))
            {
                HubGUID = Guid.NewGuid().ToString();
                _logger.LogInformation($"Guid of current Hub for this session: \'{HubGUID}\'");
            }

            if (UserGroup == null)
                UserGroup = new Dictionary<string, string>();

            if (Modules == null)
                Modules = new List<ModuleHubWrapper>();
        }

        #region Properties

        /// <summary>
        /// Store module connection id (key) and module group (value)
        /// </summary>
        private static Dictionary<string, string> UserGroup { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private static List<ModuleHubWrapper> Modules { get; set; } 

        #endregion

        #region Register Logic

        /// <summary>
        /// Register any module on their group
        /// </summary>
        /// <param name="groupName">Module group</param>
        /// <returns></returns>
        public async Task RegisterModule(ModuleTypes moduleType)
        {
            ModuleHubWrapper newModule = new ModuleHubWrapper(Context.ConnectionId, this);
            newModule.TargedModule.ModuleType = moduleType;
            Modules.Add(newModule); //add new module to maintance module info

            await this.Groups.AddToGroupAsync(Context.ConnectionId, moduleType.ToString());
            UserGroup.Add(Context.ConnectionId, moduleType.ToString());

            _logger.LogInformation($"Successfully registered module: {moduleType.ToString()}, Connection ID: {Context.ConnectionId}");

            await this.Clients.Caller.SendAsync("OnRegister", "Ok", HubGUID); //Callback that successfull register module
        }
        
        /// <summary>
        /// Unregister module from group
        /// </summary>
        /// <returns></returns>
        public async Task UnregisterModule(string connectionID)
        {
            string connectionId = connectionID;
            await Groups.RemoveFromGroupAsync(connectionId, UserGroup[connectionId]);

            _logger.LogInformation($"Unregistering module: {UserGroup[connectionId]}, Connection ID: {connectionId}");
            UserGroup.Remove(connectionId);

            var module = Modules.FirstOrDefault(x => x.ConnectionId == connectionId);
            if(module != null)
                Modules.Remove(module);
        }

        /// <summary>
        /// Handle module disconnect and remove it from group
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            UnregisterModule(Context.ConnectionId).Wait();
            _logger.LogInformation($"Module disconnected: ConnectionId: \'{Context.ConnectionId}\'");
            //Console.WriteLine($"{Context.ConnectionId} disconnected");
            return base.OnDisconnectedAsync(exception);
        }

        #endregion



        #region Maintance region

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        public async Task RecieveMachineInfo(MachineSpecifications moduleInfo)
        {
            UpdatedMachineInfo?.Invoke(Context.ConnectionId, moduleInfo);
        }

        public async Task ModuleActivity(MachineSpecifications moduleInfo)
        {
            UpdatedMachineInfo?.Invoke(Context.ConnectionId, moduleInfo);
        }

        #endregion

        #region Helpers

        //TODO: Set IP adress to module

        #endregion
    }
}
