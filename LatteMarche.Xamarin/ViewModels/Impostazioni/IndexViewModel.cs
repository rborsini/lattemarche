using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Rest.Interfaces;
using LatteMarche.Xamarin.Views.Synch;
using LatteMarche.Xamarin.Zebra;
using LatteMarche.Xamarin.Zebra.Interfaces;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Sentry;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace LatteMarche.Xamarin.ViewModels.Impostazioni
{
    public class IndexViewModel : BaseViewModel
    {
        #region Fields

        private IMaterialModalPage loadingDialog;

        private IStampantiService stampantiService => DependencyService.Get<IStampantiService>();
        private ISincronizzazioneService sincronizzazioneService = DependencyService.Get<ISincronizzazioneService>();
        private IAmbientiService ambientiService = DependencyService.Get<IAmbientiService>();

        private bool stampantiPresenti;

        private Stampante stampanteSelezionata;

        private ObservableCollection<Stampante> stampanti;

        private string versione;
        private string ambiente;
        private string ultimoAggiornamento;

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
                //(this.SetDefaultCommand as Command).ChangeCanExecute();
            }
        }

        public string Versione
        {
            get { return this.versione; }
            set { SetProperty(ref this.versione, value); }
        }

        public string Ambiente
        {
            get { return this.ambiente; }
            set { SetProperty(ref this.ambiente, value); }
        }

        public string UltimoAggiornamento
        {
            get { return this.ultimoAggiornamento; }
            set { SetProperty(ref this.ultimoAggiornamento, value); }
        }

        public Command LoadCommand { get; set; }

        public Command DiscoveryCommand { get; set; }

        public Command SetDefaultCommand { get; set; }

        public Command ExportCommand { get; set; }

        public Command UpdateCommand { get; set; }

        public Command ResetCommand { get; set; }

        #endregion

        #region Constructor

        public IndexViewModel(INavigation navigation, Page page)
            : base(navigation, page)
        {
            this.navigation = navigation;
            this.page = page;

            this.LoadCommand = new Command(async () => await ExecuteLoadCommand());
            this.DiscoveryCommand = new Command(async () => await ExecuteDiscoveryCommand());
            //this.SetDefaultCommand = new Command(async () => await ExecuteSetDefaultCommand(), canExecute: () => { return this.stampanteSelezionata != null; });
            this.SetDefaultCommand = new Command(async () => await ExecuteSetDefaultCommand());
            this.ExportCommand = new Command(async () => await ExecuteExportCommand());
            this.UpdateCommand = new Command(async () => await ExecuteUpdateCommand());
            this.ResetCommand = new Command(async () => await ExecuteResetCommand());
        }

        #endregion

        #region Methods


        private async Task ExecuteLoadCommand()
        {
            this.loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Caricamento elenco stampanti", lottieAnimation: "LottieLogo1.json");

            try
            {

                var stampanti = this.stampantiService.GetItemsAsync().Result;
                this.Stampanti = new ObservableCollection<Stampante>(stampanti);

                this.StampanteSelezionata = stampanti.FirstOrDefault(s => s.Selezionata);

                this.StampantiPresenti = this.Stampanti.Count > 0;

                VersionTracking.Track();

                var ambiente = this.ambientiService.GetDefault();

                this.Versione = VersionTracking.CurrentVersion;
                this.Ambiente = ambiente != null ? ambiente.Nome : "";

                var sincronizzazioneService = DependencyService.Get<ISincronizzazioneService>();
                var ultimoDownload = sincronizzazioneService.GetLastAysnc(Enums.SynchType.Download).Result;

                this.UltimoAggiornamento = $"{ultimoDownload.Timestamp:dd-MM-yyyy HH:mm:ss}";
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

                if (this.stampanteSelezionata != null)
                    this.stampantiService.SetDefaultAsync(this.stampanteSelezionata.MacAddress).Wait();

                await loadingDialog.DismissAsync();

                if(this.stampanteSelezionata != null)
                    await this.page.DisplayAlert("Info", "Impostata stampante predefinita", "OK");
            }
            catch (Exception exc)
            {
                await loadingDialog.DismissAsync();

                SentrySdk.CaptureException(exc);
                Crashes.TrackError(exc);

                await this.page.DisplayAlert("Errore", "Si è verificato un errore imprevisto. Contattare l'amministratore", "OK");
            }
        }

        private async Task ExecuteUpdateCommand()
        {
            try
            {
                var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Aggiornamento in corso", lottieAnimation: "LottieLogo1.json");
                var device = DependencyService.Get<IDevice>();
                var restService = DependencyService.Get<IRestService>();

                await Task.Run(() =>
                {
                    try
                    { 
                        var dto = restService.Download(device.GetIdentifier()).Result;
                        sincronizzazioneService.UpdateDatabaseSync(dto).Wait();

                        this.UltimoAggiornamento = $"{DateTime.Now:dd-MM-yyyy HH:mm:ss}";

                        Analytics.TrackEvent("Download avvenuto", new Dictionary<string, string>() { { "dto", JsonConvert.SerializeObject(dto) } });
                        SentrySdk.CaptureMessage("Download avvenuto", Sentry.Protocol.SentryLevel.Info);
                    }
                    catch (Exception exc)
                    {
                        SentrySdk.CaptureException(exc);
                        Crashes.TrackError(exc);
                    }

                });

                await loadingDialog.DismissAsync();
                await this.page.DisplayAlert("Info", "Aggiornamento avvenuto con successo", "OK");
            }
            catch (Exception exc)
            {
                this.IsBusy = false;
                await this.page.DisplayAlert("Errore", exc.Message, "OK");
            }

        }

        private async Task ExecuteExportCommand()
        {
            try
            {
                this.IsBusy = true;

                await Task.Run(() =>
                {
                    var internalFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "database.db");
                    DependencyService.Get<IFileSystem>().ExportDb(internalFile);
                });

                this.IsBusy = false;
                await this.page.DisplayAlert("Info", "Esportazione avvenuta con successo", "OK");
            }
            catch (Exception exc)
            {
                this.IsBusy = false;
                await this.page.DisplayAlert("Errore", exc.Message, "OK");
            }

        }

        private async Task ExecuteResetCommand()
        {
            IMaterialModalPage loadingDialog = null;
            try
            {
                bool reply = await this.page.DisplayAlert("Attenzione", $"Sei sicuro di voler resettare l'applicazione?", "Si", "No");
                if (reply == false)
                    return;

                loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Reset in corso", lottieAnimation: "LottieLogo1.json");
                await Task.Run(() =>
                {
                    this.sincronizzazioneService.ResetAsync().Wait();
                });

                this.IsBusy = false;
                await loadingDialog.DismissAsync();
                await this.page.DisplayAlert("Info", "Reset avvenuto con successo", "OK");

                App.Current.MainPage = new RegisterPage();
                await this.page.Navigation.PopToRootAsync();

            }
            catch (Exception exc)
            {
                this.IsBusy = false;
                await loadingDialog.DismissAsync();
                await this.page.DisplayAlert("Errore", exc.Message, "OK");
            }

        }

        private void DiscoveryHandler_OnFoundPrinter(object sender, Db.Models.Stampante stampante)
        {
            try
            {
                
                this.stampantiService.InsertOrUpdateAsync(stampante).Wait();

                var stampanti = this.stampantiService.GetItemsAsync().Result;
                this.Stampanti = new ObservableCollection<Stampante>(stampanti);
                this.StampantiPresenti = this.Stampanti.Count > 0;

                if (this.Stampanti.Count == 1)
                    this.StampanteSelezionata = stampante;


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
