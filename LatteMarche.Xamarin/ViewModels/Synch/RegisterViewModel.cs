using LatteMarche.Xamarin.Db.Interfaces;
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

        #endregion

        #region Properties

        public Command RegisterCommand { get; set; }

        #endregion

        #region Constructor

        public RegisterViewModel(INavigation navigation, Page page)
            : base(navigation, page)
        {
            this.navigation = navigation;
            this.page = page;
            this.RegisterCommand = new Command(async () => await ExecuteRegisterCommand(), canExecute: () => { return this.IsOnline; });

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        #endregion

        #region Methods

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            (this.RegisterCommand as Command).ChangeCanExecute();
        }


        private async Task ExecuteRegisterCommand()
        {
            var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Registrazione in corso", lottieAnimation: "LottieLogo1.json");

            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                VersionTracking.Track();
                var appVersion = VersionTracking.CurrentVersion;
                var isActive = false;

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
                    

            }
            catch (Exception exc)
            {
                await loadingDialog.DismissAsync();

                SentrySdk.CaptureException(exc);
                Crashes.TrackError(exc);

                await this.page.DisplayAlert("Errore", "Si è verificato un errore imprevisto. Contattare l'amministratore", "OK");
            }

        }

        #endregion
    }
}
