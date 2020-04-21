using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Zebra;
using LatteMarche.Xamarin.Zebra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LatteMarche.Xamarin.ViewModels.Impostazioni
{
    public class IndexViewModel : BaseViewModel
    {
        #region Fields

        #endregion

        #region Properties

        public Command DiscoveryCommand { get; set; }

        #endregion

        #region Constructor

        public IndexViewModel(INavigation navigation, Page page)
            : base(navigation, page)
        {
            this.navigation = navigation;
            this.page = page;
            this.DiscoveryCommand = new Command(async () => await ExecuteDiscoveryCommand());
        }

        #endregion

        #region Methods

        private async Task ExecuteDiscoveryCommand()
        {
            try
            {
                DiscoveryHandlerImplementation discoveryHandler = new DiscoveryHandlerImplementation(this.page);
                DependencyService.Get<IConnectionManager>().FindBluetoothPrinters(discoveryHandler);
            }
            catch (Exception exc)
            {
                await this.page.DisplayAlert("Error", exc.Message, "OK");
            }
        }

        #endregion
    }
}
