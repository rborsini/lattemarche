using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Enums;
using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Rest.Dtos;
using LatteMarche.Xamarin.Rest.Interfaces;
using LatteMarche.Xamarin.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LatteMarche.Xamarin.ViewModels.Synch
{
    public class RegisterViewModel : BaseViewModel
    {
        #region Fields

        private IDevice device = DependencyService.Get<IDevice>();

        private IRestService restService = DependencyService.Get<IRestService>();

        private ITrasportatoriService trasportatoriService = DependencyService.Get<ITrasportatoriService>();

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
            try
            {
                this.IsBusy = true;

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
                        VersioneApp = appVersion
                    };

                    var dispositivo = this.restService.Register(dto).Result;
                    isActive = dispositivo.Attivo && dispositivo.IdTrasportatore.HasValue;

                    if (isActive)
                    {
                        this.trasportatoriService.DeleteAllItemsAsync().Wait();
                        this.trasportatoriService.AddItemAsync(new Db.Models.Trasportatore()
                        {
                            Id = dispositivo.IdTrasportatore.Value
                        }).Wait();
                    }

                    this.sincronizzazioneService.AddAsync(SynchType.Register).Wait();

                });

                this.IsBusy = false;
                await this.page.DisplayAlert("Info", "Registrazione inviata con successo", "OK");

                if (isActive)
                {
                    Application.Current.MainPage = new MainPage();
                    await (Application.Current.MainPage as MainPage).NavigateFromMenu((int)MenuItemType.Synch);
                }
                    

            }
            catch (Exception exc)
            {
                this.IsBusy = false;
                await this.page.DisplayAlert("Error", exc.Message, "OK");
            }

        }

        #endregion
    }
}
