using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DNet_DataContracts.Maintance;
using DNet_DataContracts;

using DNet_Hub.Communication;

namespace DNet_Hub.Hubs
{
    public class MainHub : Hub
    {
        public delegate void MachineHardwareEvent(string id, MachineSpecifications info);
        public event MachineHardwareEvent UpdatedMachineInfo;


        /// <summary>
        /// Store module connection id (key) and module group (value)
        /// </summary>
        private Dictionary<string, string> UserGroup { get; set; } 
        
        IList<ModuleHubWrapper> Modules { get; set; }

        public MainHub()
        {
            UserGroup = new Dictionary<string, string>();
            Modules = new List<ModuleHubWrapper>();
        }

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

            await this.Clients.Caller.SendAsync("OnRegister", "Ok"); //Callback that successfull register module
        }
        
        /// <summary>
        /// Unregister module from group
        /// </summary>
        /// <returns></returns>
        public async Task UnregisterModule()
        {
            string connectionId = Context.ConnectionId;
            await Groups.RemoveFromGroupAsync(connectionId, UserGroup[connectionId]);
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
            UnregisterModule();
            return base.OnDisconnectedAsync(exception);
        }

        #endregion

        #region Maintance region

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
