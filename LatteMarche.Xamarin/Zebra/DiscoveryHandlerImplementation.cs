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

        #region Methods

        public void DiscoveryError(string message)
        {
            Device.BeginInvokeOnMainThread(async () => {
                await this.page.DisplayAlert("Discovery Error", message, "OK");
            });
        }

        public void DiscoveryFinished()
        {
            Device.BeginInvokeOnMainThread(async () => {
                await this.page.DisplayAlert("Discovery", "Finished", "OK");
            });
        }

        public void FoundPrinter(DiscoveredPrinter printer)
        {
            Device.BeginInvokeOnMainThread(async () => {
                await this.page.DisplayAlert("Printer found", printer.Address, "OK");
            });
        }

        #endregion

    }
}
