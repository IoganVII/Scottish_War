using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Scottish_duel.Hubs
{
    public class RoomHub : Hub
    {

        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<RoomHub>();


        public void Send(string name, string message)
        {
            Clients.All.broadcastMessage(name, message);
        }

    }
}