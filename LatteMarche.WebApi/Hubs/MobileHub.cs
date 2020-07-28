using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LatteMarche.WebApi.Hubs
{
    public class MobileHub : Hub
    {
        public void Send(string deviceId)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(deviceId);
        }
    }
}