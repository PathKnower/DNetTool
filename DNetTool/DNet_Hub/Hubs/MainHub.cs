using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DNet_Hub.Hubs
{
    public class MainHub : Hub
    {
        /// <summary>
        /// Register any module on their group
        /// </summary>
        /// <param name="groupName">Module group</param>
        /// <returns></returns>
        public async Task RegisterModule(string groupName)
        {
            await this.Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await this.Clients.Caller.SendAsync("OnRegister", "Ok"); //Callback that successfull register module
        }
        
        public async Task UnregisterModule()
        {

        }

    }
}
