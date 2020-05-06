using LatteMarche.Xamarin.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Zebra.Sdk.Printer.Discovery;

namespace LatteMarche.Xamarin.Zebra
{
    public class DiscoveryHandlerImplementation : DiscoveryHandler
    {

        #region Fields

        private Page page;

        #endregion

        #region Constructor

        public DiscoveryHandlerImplementation(Page page)
        {
            this.page = page;
        }

        #endregion

        #region Events

        public event EventHandler<string> OnDiscoveryError;
        public event EventHandler OnDiscoveryFinished;
        public event EventHandler<Stampante> OnFoundPrinter;

        #endregion

        #region Methods

        public void DiscoveryError(string message)
        {
            if(this.OnDiscoveryError != null)
            {
                this.OnDiscoveryError(this, message);
            }
        }

        public void DiscoveryFinished()
        {
            if (this.OnDiscoveryFinished != null)
            {
                this.OnDiscoveryFinished(this, null);
            }
        }

        public void FoundPrinter(DiscoveredPrinter printer)
        {
            if (this.OnFoundPrinter != null)
            {
                this.OnFoundPrinter(this, new Stampante() { MacAddress = printer.Address, Nome = printer.DiscoveryDataMap["FRIENDLY_NAME"] });
            }
        }

        #endregion

    }
}
