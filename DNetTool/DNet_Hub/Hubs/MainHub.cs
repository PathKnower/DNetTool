using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DNet_Hub.Hubs
{
    public class MainHub : Hub
    {
        public async Task OnRegister()
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("");
        }
    }
}
