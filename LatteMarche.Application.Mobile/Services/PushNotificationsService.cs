using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Mobile.Services
{
    /// <summary>
    /// Classe delegata al dispacciamento delle notifiche 
    /// </summary>
    public sealed class PushNotificationsService
    {
        private static readonly Lazy<PushNotificationsService> lazy = new Lazy<PushNotificationsService>(() => new PushNotificationsService());

        public static PushNotificationsService Instance => lazy.Value;

        public PushNotificationsService()
        { }

        public event EventHandler<MobileEventArgs> OnNotification;

        public void Push(string dispositivoId)
        {
            if (this.OnNotification != null)
                this.OnNotification(this, new MobileEventArgs(dispositivoId));
        }

    }

    public class MobileEventArgs : EventArgs
    {
        private string dispositivoId;

        public string DispositivoId => this.dispositivoId;

        public MobileEventArgs(string dispositivoId)
        {
            this.dispositivoId = dispositivoId;
        }

    }
}
