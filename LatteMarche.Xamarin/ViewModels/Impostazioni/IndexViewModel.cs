using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Zebra;
using LatteMarche.Xamarin.Zebra.Interfaces;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Sentry;
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

        private IMaterialModalPage loadingDialog;

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
            this.loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Caricamento elenco stampanti", lottieAnimation: "LottieLogo1.json");

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
                await loadingDialog.DismissAsync();

                SentrySdk.CaptureException(exc);
                Crashes.TrackError(exc);

                await this.page.DisplayAlert("Errore", "Si è verificato un errore imprevisto. Contattare l'amministratore", "OK");
            }
            finally
            {
                await loadingDialog.DismissAsync();
            }

        }


        private async Task ExecuteDiscoveryCommand()
        {
            try
            {
                this.loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Ricerca stampanti in corso", lottieAnimation: "LottieLogo1.json");

                DiscoveryHandlerImplementation discoveryHandler = new DiscoveryHandlerImplementation(this.page);

                discoveryHandler.OnDiscoveryError += DiscoveryHandler_OnDiscoveryError;
                discoveryHandler.OnDiscoveryFinished += DiscoveryHandler_OnDiscoveryFinished;
                discoveryHandler.OnFoundPrinter += DiscoveryHandler_OnFoundPrinter;

                DependencyService.Get<IConnectionManager>().FindBluetoothPrinters(discoveryHandler);
            }
            catch (Exception exc)
            {
                SentrySdk.CaptureException(exc);
                Crashes.TrackError(exc);

                await this.page.DisplayAlert("Errore", "Si è verificato un errore imprevisto. Contattare l'amministratore", "OK");
            }
        }

        private async Task ExecuteSetDefaultCommand()
        {
            try
            {
                this.loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Salvataggio in corso", lottieAnimation: "LottieLogo1.json");

                await Task.Run(() =>
                {
                    this.stampantiService.SetDefaultAsync(this.stampanteSelezionata.MacAddress).Wait();
                });

                await loadingDialog.DismissAsync();
            }
            catch (Exception exc)
            {
                await loadingDialog.DismissAsync();

                SentrySdk.CaptureException(exc);
                Crashes.TrackError(exc);

                await this.page.DisplayAlert("Errore", "Si è verificato un errore imprevisto. Contattare l'amministratore", "OK");
            }
        }

        private void DiscoveryHandler_OnFoundPrinter(object sender, Db.Models.Stampante stampante)
        {
            try
            {
                this.StampantiPresenti = true;
                this.stampantiService.InsertOrUpdateAsync(stampante).Wait();

                Analytics.TrackEvent("Stampante associata");
                SentrySdk.CaptureMessage("Stampante associata", Sentry.Protocol.SentryLevel.Info);
            }
            catch(Exception exc)
            {
                SentrySdk.CaptureException(exc);
                Crashes.TrackError(exc);

                this.page.DisplayAlert("Errore", "Si è verificato un errore imprevisto. Contattare l'amministratore", "OK");
            }
        }

        private async void DiscoveryHandler_OnDiscoveryFinished(object sender, EventArgs e)
        {
            await loadingDialog.DismissAsync(); 
        }

        private void DiscoveryHandler_OnDiscoveryError(object sender, string errorMessage)
        {
            loadingDialog.DismissAsync().Wait();

            SentrySdk.CaptureException(new Exception(errorMessage));
            Crashes.TrackError(new Exception(errorMessage));

            this.page.DisplayAlert("Errore", "Si è verificato un errore imprevisto. Contattare l'amministratore", "OK");
        }

        #endregion
    }
}
