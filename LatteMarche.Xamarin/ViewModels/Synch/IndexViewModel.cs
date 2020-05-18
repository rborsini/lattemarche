using AutoMapper;
using LatteMarche.Xamarin.Db;
using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Enums;
using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Rest.Dtos;
using LatteMarche.Xamarin.Rest.Interfaces;
using LatteMarche.Xamarin.Views.Synch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LatteMarche.Xamarin.ViewModels.Synch
{
    public class IndexViewModel : BaseViewModel
    {

        #region Fields

        private IDevice device = DependencyService.Get<IDevice>();
        private IRestService restService => DependencyService.Get<IRestService>();
        private IGiriService giriService => DependencyService.Get<IGiriService>();

        private ISincronizzazioneService sincronizzazioneService = DependencyService.Get<ISincronizzazioneService>();

        #endregion

        #region Properties

        public Command DownloadCommand { get; set; }

        public Command UploadCommand { get; set; }

        public Command ExportCommand { get; set; }

        public Command ResetCommand { get; set; }

        #endregion

        #region Constructor

        public IndexViewModel(INavigation navigation, Page page)
            : base(navigation, page)
        {
            this.navigation = navigation;
            this.page = page;
            this.DownloadCommand = new Command(execute: async () => await ExecuteDownloadCommand(), canExecute: () => { return this.IsOnline; });
            this.UploadCommand = new Command(async () => await ExecuteUploadCommand(), canExecute: () => { return this.IsOnline; });
            this.ExportCommand = new Command(async () => await ExecuteExportCommand());
            this.ResetCommand = new Command(async () => await ExecuteResetCommand());

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

        }


        #endregion

        #region Methods

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            (this.DownloadCommand as Command).ChangeCanExecute();
            (this.UploadCommand as Command).ChangeCanExecute();
        }

        private async Task ExecuteDownloadCommand()
        {
            try
            {
                this.IsBusy = true;                

                await Task.Run(() =>
                {
                    var dto = this.restService.Download(this.device.GetIdentifier()).Result;

                    this.sincronizzazioneService.UpdateDatabaseSync(dto).Wait();

                });

                this.IsBusy = false;
                await this.page.DisplayAlert("Info", "Download avvenuto con successo", "OK");
            }
            catch (Exception exc)
            {
                this.IsBusy = false;
                await this.page.DisplayAlert("Errore", exc.Message, "OK");
            }

        }

        private async Task ExecuteUploadCommand()
        {
            try
            {
                this.IsBusy = true;
                var location = await Geolocation.GetLastKnownLocationAsync();
                VersionTracking.Track();

                await Task.Run(() =>
                {
                    var giri = this.giriService.GetGiriApertiAsync().Result;
                    var prelievi = giri.SelectMany(g => g.Prelievi).ToList();

                    var prelieviDto = Mapper.Map<List<PrelievoLatteDto>>(prelievi);

                    var uploadDto = new UploadDto()
                    {
                        IMEI = this.device.GetIdentifier(),
                        Lat = location != null ? Convert.ToDecimal(location.Latitude) : (decimal?)null,
                        Lng = location != null ? Convert.ToDecimal(location.Longitude) : (decimal?)null,
                        VersioneApp = VersionTracking.CurrentVersion,
                        VersioneOS = DeviceInfo.VersionString,
                        Marca = DeviceInfo.Manufacturer,
                        Modello = DeviceInfo.Model,
                        Nome = DeviceInfo.Name,
                        Prelievi = Mapper.Map<List<PrelievoLatteDto>>(prelievi)
                    };

                    // chiamata REST upload dati
                    if(this.restService.Upload(uploadDto).Result)
                    {

                        // aggiornamento tabella sincronizzazioni
                        this.sincronizzazioneService.AddAsync(SynchType.Upload).Wait();
                    }
                    else
                    {
                        throw new Exception("Errore di sincronizzazione con il server");
                    }


                });

                this.IsBusy = false;
                await this.page.DisplayAlert("Info", "Upload avvenuto con successo", "OK");
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

                var imei = DependencyService.Get<IDevice>().GetIdentifier();
                var location = await Geolocation.GetLastKnownLocationAsync();

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
            try
            {
                this.IsBusy = true;

                await Task.Run(() =>
                {
                    this.sincronizzazioneService.ResetAsync().Wait();
                });

                this.IsBusy = false;
                await this.page.DisplayAlert("Info", "Reset avvenuto con successo", "OK");

                App.Current.MainPage = new RegisterPage();
                await this.page.Navigation.PopToRootAsync();

            }
            catch (Exception exc)
            {
                this.IsBusy = false;
                await this.page.DisplayAlert("Errore", exc.Message, "OK");
            }

        }

        #endregion
    }
}
