using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Enums;
using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Rest.Dtos;
using LatteMarche.Xamarin.Rest.Interfaces;
using LatteMarche.Xamarin.Views;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Sentry;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace LatteMarche.Xamarin.ViewModels.Synch
{
    public class RegisterViewModel : BaseViewModel
    {
        #region Fields

        private IDevice device = DependencyService.Get<IDevice>();

        private IRestService restService = DependencyService.Get<IRestService>();

        private ISincronizzazioneService sincronizzazioneService = DependencyService.Get<ISincronizzazioneService>();

        private IAmbientiService ambientiService = DependencyService.Get<IAmbientiService>();

        private Ambiente ambienteSelezionato;
        private ObservableCollection<Ambiente> ambienti;
        private bool registrazionePendente;
        private bool registrazioneNonPendente;

        #endregion

        #region Properties

        public Ambiente AmbienteSelezionato
        {
            get { return this.ambienteSelezionato; }
            set { SetProperty<Ambiente>(ref this.ambienteSelezionato, value); }
        }

        public ObservableCollection<Ambiente> Ambienti
        {
            get { return this.ambienti; }
            set { SetProperty<ObservableCollection<Ambiente>>(ref this.ambienti, value); }
        }

        public bool RegistrazionePendente
        {
            get { return this.registrazionePendente; }
            set { SetProperty<bool>(ref this.registrazionePendente, value); }
        }

        public bool RegistrazioneNonPendente
        {
            get { return this.registrazioneNonPendente; }
            set { SetProperty<bool>(ref this.registrazioneNonPendente, value); }
        }

        public Command LoadCommand { get; set; }

        public Command RegisterCommand { get; set; }

        #endregion

        #region Constructor

        public RegisterViewModel(INavigation navigation, Page page)
            : base(navigation, page)
        {
            this.navigation = navigation;
            this.page = page;
            this.RegisterCommand = new Command(async () => await ExecuteRegisterCommand(), canExecute: () => { return this.IsOnline; });
            this.LoadCommand = new Command(async () => await ExecuteLoadCommand());

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        #endregion

        #region Methods

        private async Task ExecuteLoadCommand()
        {
            var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Caricamento in corso", lottieAnimation: "LottieLogo1.json");

            try
            {
                var isActive = false;

                await Task.Run(() =>
                {
                    this.Ambienti = new ObservableCollection<Ambiente>(this.ambientiService.Init());
                    this.AmbienteSelezionato = this.Ambienti.First(a => a.Selezionato);

                    var ultimaRegistrazione = this.sincronizzazioneService.GetLastAysnc(SynchType.Register).Result;
                    this.RegistrazionePendente = ultimaRegistrazione != null;
                    this.RegistrazioneNonPendente = !this.RegistrazionePendente;

                    if (this.RegistrazionePendente)
                    {
                        var dbDto = this.restService.Download(this.device.GetIdentifier()).Result;
                        this.sincronizzazioneService.UpdateDatabaseSync(dbDto).Wait();
                        isActive = dbDto != null;
                    }

                });

                await loadingDialog.DismissAsync();

                if (isActive)
                {
                    Application.Current.MainPage = new MainPage();
                    await (Application.Current.MainPage as MainPage).NavigateFromMenu((int)MenuItemType.Giri);
                }

            }
            catch (Exception exc)
            {
                await loadingDialog.DismissAsync();

                SentrySdk.CaptureException(exc);
                Crashes.TrackError(exc);

                await this.page.DisplayAlert("Errore", "Si è verificato un errore imprevisto. Contattare l'amministratore", "OK");
            }

        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            (this.RegisterCommand as Command).ChangeCanExecute();
        }

        private Location GetLocation()
        {
            var permissionStatus = Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>().Result;
            return permissionStatus == PermissionStatus.Granted ? Geolocation.GetLastKnownLocationAsync().Result : null;
        }

        private async Task ExecuteRegisterCommand()
        {

            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Registrazione in corso", lottieAnimation: "LottieLogo1.json");

            try
            {
                var location = GetLocation();
                VersionTracking.Track();
                var appVersion = VersionTracking.CurrentVersion;
                var isActive = false;

                this.ambientiService.SetDefault(this.AmbienteSelezionato.Id);

                await Task.Run(() =>
                {
                    var dto = new DispositivoDto()
                    {
                        Id = this.device.GetIdentifier(),
                        Lat = location != null ? Convert.ToDecimal(location.Latitude) : (decimal?)null,
                        Lng = location != null ? Convert.ToDecimal(location.Longitude) : (decimal?)null,
                        VersioneApp = appVersion,
                        VersioneOS = DeviceInfo.VersionString,
                        Marca = DeviceInfo.Manufacturer,
                        Modello = DeviceInfo.Model,
                        Nome = DeviceInfo.Name
                    };

                    var dispositivo = this.restService.Register(dto).Result;
                    isActive = dispositivo.Attivo && dispositivo.IdTrasportatore.HasValue;

                    if (isActive)
                    {
                        var dbDto = this.restService.Download(this.device.GetIdentifier()).Result;
                        this.sincronizzazioneService.UpdateDatabaseSync(dbDto).Wait();
                    }

                    this.sincronizzazioneService.AddAsync(SynchType.Register).Wait();

                    Analytics.TrackEvent("Registrazione inviata con successo", new Dictionary<string, string>() { { "dto", JsonConvert.SerializeObject(dto) } });
                    SentrySdk.CaptureMessage("Registrazione inviata con successo", Sentry.Protocol.SentryLevel.Info);

                });

                await loadingDialog.DismissAsync();
                await this.page.DisplayAlert("Info", "Registrazione inviata con successo", "OK");

                if (isActive)
                {
                    Application.Current.MainPage = new MainPage();
                    await (Application.Current.MainPage as MainPage).NavigateFromMenu((int)MenuItemType.Giri);
                }
                else
                {
                    this.RegistrazionePendente = true;
                    this.RegistrazioneNonPendente = !this.RegistrazionePendente;
                }


            }
            catch (Exception exc)
            {
                await loadingDialog.DismissAsync();

                SentrySdk.CaptureException(exc);
                Crashes.TrackError(exc);

                await this.page.DisplayAlert("Errore", "Si è verificato un errore imprevisto. Contattare l'amministratore", "OK");
            }

        }

        //private async Task<bool> RegisterAsync()
        //{
        //    var location = GetLocation();
        //    VersionTracking.Track();
        //    var appVersion = VersionTracking.CurrentVersion;
        //    var isActive = false;

        //    var dto = new DispositivoDto()
        //    {
        //        Id = this.device.GetIdentifier(),
        //        Lat = location != null ? Convert.ToDecimal(location.Latitude) : (decimal?)null,
        //        Lng = location != null ? Convert.ToDecimal(location.Longitude) : (decimal?)null,
        //        VersioneApp = appVersion,
        //        VersioneOS = DeviceInfo.VersionString,
        //        Marca = DeviceInfo.Manufacturer,
        //        Modello = DeviceInfo.Model,
        //        Nome = DeviceInfo.Name
        //    };

        //    await Task.Run(() =>
        //    {

        //        var dispositivo = this.restService.Register(dto).Result;
        //        isActive = dispositivo.Attivo && dispositivo.IdTrasportatore.HasValue;

        //        if (isActive)
        //        {
        //            var dbDto = this.restService.Download(this.device.GetIdentifier()).Result;
        //            this.sincronizzazioneService.UpdateDatabaseSync(dbDto).Wait();
        //        }

        //        this.sincronizzazioneService.AddAsync(SynchType.Register).Wait();
        //    });

        //    Analytics.TrackEvent("Registrazione inviata con successo", new Dictionary<string, string>() { { "dto", JsonConvert.SerializeObject(dto) } });
        //    SentrySdk.CaptureMessage("Registrazione inviata con successo", Sentry.Protocol.SentryLevel.Info);
        //    return isActive;
        //}

        #endregion
    }
}
