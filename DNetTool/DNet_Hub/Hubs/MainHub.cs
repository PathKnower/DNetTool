using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DNet_Hub.Hubs
{
    public class MainHub : Hub
    {
        //Store module connection id (key) and module group (value)
        private Dictionary<string, string> UserGroup { get; set; } 

        public MainHub()
        {
            UserGroup = new Dictionary<string, string>();
        }

        #region Register Logic

        /// <summary>
        /// Register any module on their group
        /// </summary>
        /// <param name="groupName">Module group</param>
        /// <returns></returns>
        public async Task RegisterModule(string groupName)
        {
            await this.Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            UserGroup.Add(Context.ConnectionId, groupName);

            await this.Clients.Caller.SendAsync("OnRegister", "Ok"); //Callback that successfull register module
        }
        
        /// <summary>
        /// Unregister module from group
        /// </summary>
        /// <returns></returns>
        public async Task UnregisterModule()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, UserGroup[Context.ConnectionId]);
            UserGroup.Remove(Context.ConnectionId);
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

        public async Task ModuleActivity()
        {

        }
    }
}
