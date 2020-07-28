using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LatteMarche.WebApi.Hubs;
using LatteMarche.Application.Mobile.Services;

namespace LatteMarche.WebApi.App_Start
{
    public class MobileHubConfig
    {
        internal static void Configure()
        {
            PushNotificationsService.Instance.OnNotification += Instance_OnNotification;
        }

        private static void Instance_OnNotification(object sender, MobileEventArgs e)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<MobileHub>();
            context.Clients.All.broadcastMessage(e.DispositivoId);
        }
    }
}