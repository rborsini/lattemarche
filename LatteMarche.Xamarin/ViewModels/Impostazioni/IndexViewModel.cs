using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Zebra;
using LatteMarche.Xamarin.Zebra.Interfaces;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace LatteMarche.Xamarin.ViewModels.Impostazioni
{
    public class IndexViewModel : BaseViewModel
    {
        #region Fields

        private IStampantiService stampantiService => DependencyService.Get<IStampantiService>();

        private bool stampantiPresenti;

        private Stampante stampanteSelezionata;

        private ObservableCollection<Stampante> stampanti;

        #endregion

        #region Properties

        public ObservableCollection<Stampante> Stampanti
        {
            get { return this.stampanti; }
            set { SetProperty<ObservableCollection<Stampante>>(ref this.stampanti, value); }
        }

        public bool StampantiPresenti
        {
            get { return this.stampantiPresenti; }
            set { SetProperty(ref this.stampantiPresenti, value); }
        }

        public Stampante StampanteSelezionata
        {
            get { return this.stampanteSelezionata; }
            set 
            { 
                SetProperty<Stampante>(ref this.stampanteSelezionata, value);
                (this.SetDefaultCommand as Command).ChangeCanExecute();
            }
        }


        public Command LoadCommand { get; set; }

        public Command DiscoveryCommand { get; set; }

        public Command SetDefaultCommand { get; set; }

        #endregion

        #region Constructor

        public IndexViewModel(INavigation navigation, Page page)
            : base(navigation, page)
        {
            this.navigation = navigation;
            this.page = page;

            this.LoadCommand = new Command(async () => await ExecuteLoadCommand());
            this.DiscoveryCommand = new Command(async () => await ExecuteDiscoveryCommand());
            this.SetDefaultCommand = new Command(async () => await ExecuteSetDefaultCommand(), canExecute: () => { return this.stampanteSelezionata != null; });
        }

        #endregion

        #region Methods


        private async Task ExecuteLoadCommand()
        {
            this.IsBusy = true;
            
            try
            {
                await Task.Run(() =>
                {
                    var stampanti = this.stampantiService.GetItemsAsync().Result;
                    this.Stampanti = new ObservableCollection<Stampante>(stampanti);
                    this.StampantiPresenti = this.Stampanti.Count > 0;
                });

            }
            catch (Exception exc)
            {
                await this.page.DisplayAlert("Error", exc.Message, "OK");
            }
            finally
            {
                this.IsBusy = false;
            }

        }


        private async Task ExecuteDiscoveryCommand()
        {
            try
            {
                this.IsBusy = true;

                DiscoveryHandlerImplementation discoveryHandler = new DiscoveryHandlerImplementation(this.page);

                discoveryHandler.OnDiscoveryError += DiscoveryHandler_OnDiscoveryError;
                discoveryHandler.OnDiscoveryFinished += DiscoveryHandler_OnDiscoveryFinished;
                discoveryHandler.OnFoundPrinter += DiscoveryHandler_OnFoundPrinter;

                DependencyService.Get<IConnectionManager>().FindBluetoothPrinters(discoveryHandler);
            }
            catch (Exception exc)
            {
                await this.page.DisplayAlert("Error", exc.Message, "OK");
            }
        }

        private async Task ExecuteSetDefaultCommand()
        {
            try
            {
                this.IsBusy = true;

                await Task.Run(() =>
                {
                    this.stampantiService.SetDefaultAsync(this.stampanteSelezionata.MacAddress).Wait();
                });

                this.IsBusy = false;
            }
            catch (Exception exc)
            {
                await this.page.DisplayAlert("Error", exc.Message, "OK");
            }
        }

        private void DiscoveryHandler_OnFoundPrinter(object sender, Db.Models.Stampante stampante)
        {
            try
            {
                this.StampantiPresenti = true;
                this.stampantiService.InsertOrUpdateAsync(stampante).Wait();
            }
            catch(Exception exc)
            {
                var properties = new Dictionary<string, string> {{ "stampante", JsonConvert.SerializeObject(stampante) }};
                Crashes.TrackError(exc, properties);
            }
        }

        private void DiscoveryHandler_OnDiscoveryFinished(object sender, EventArgs e)
        {
            this.IsBusy = false;
        }

        private void DiscoveryHandler_OnDiscoveryError(object sender, string errorMessage)
        {
            this.IsBusy = false;
            this.page.DisplayAlert("Error", errorMessage, "OK");
        }

        #endregion
    }
}
