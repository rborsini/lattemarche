using AutoMapper;
using LatteMarche.Xamarin.Db;
using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Enums;
using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Rest.Dtos;
using LatteMarche.Xamarin.Rest.Interfaces;
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
        private IAllevamentiService allevamentiService => DependencyService.Get< IAllevamentiService > ();
        private IAutoCisterneService autocisterneService => DependencyService.Get<IAutoCisterneService>();
        private IAcquirentiService acquirentiService => DependencyService.Get<IAcquirentiService>();
        private IDestinatariService destinatariService => DependencyService.Get<IDestinatariService>();
        private IGiriService giriService => DependencyService.Get<IGiriService>();
        private ITipiLatteService tipiLatteService => DependencyService.Get<ITipiLatteService>();
        private ITrasportatoriService trasportatoriService => DependencyService.Get<ITrasportatoriService>();
        private ITemplateGiroService templateGiriService => DependencyService.Get<ITemplateGiroService>();

        private ISincronizzazioneService sincronizzazioneService = DependencyService.Get<ISincronizzazioneService>();

        #endregion

        #region Properties

        public Command DownloadCommand { get; set; }

        public Command UploadCommand { get; set; }

        public Command ExportCommand { get; set; }

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

                    if(dto != null)
                    {
                        // pulizia database
                        this.allevamentiService.DeleteAllItemsAsync().Wait();
                        this.templateGiriService.DeleteAllItemsAsync().Wait();
                        this.autocisterneService.DeleteAllItemsAsync().Wait();
                        this.tipiLatteService.DeleteAllItemsAsync().Wait();
                        this.acquirentiService.DeleteAllItemsAsync().Wait();
                        this.destinatariService.DeleteAllItemsAsync().Wait();


                        // trasportatore
                        var trasportatore = Mapper.Map<Trasportatore>(dto.Trasportatore);
                        this.trasportatoriService.UpdateItemAsync(trasportatore).Wait();

                        // autocisterna
                        var autocisterna = Mapper.Map<AutoCisterna>(dto.Autocisterna);
                        this.autocisterneService.AddItemAsync(autocisterna).Wait();

                        // tipi latte
                        var tipiLatte = Mapper.Map<List<TipoLatte>>(dto.TipiLatte);
                        this.tipiLatteService.AddRangeItemAsync(tipiLatte).Wait();

                        // acquirenti
                        var acquirenti = Mapper.Map<List<Acquirente>>(dto.Acquirenti);
                        this.acquirentiService.AddRangeItemAsync(acquirenti).Wait();

                        // destinatari
                        var destinatari = Mapper.Map<List<Destinatario>>(dto.Destinatari);
                        this.destinatariService.AddRangeItemAsync(destinatari).Wait();

                        // template giro
                        var giri = Mapper.Map<List<TemplateGiro>>(dto.Giri);

                        giri.ForEach(g => g.IdTrasportatore = trasportatore.Id);
                        this.templateGiriService.AddRangeItemAsync(giri).Wait();

                        this.sincronizzazioneService.AddAsync(SynchType.Download).Wait();

                    }

                });

                this.IsBusy = false;
                await this.page.DisplayAlert("Info", "Download avvenuto con successo", "OK");
            }
            catch (Exception exc)
            {
                this.IsBusy = false;
                await this.page.DisplayAlert("Error", exc.Message, "OK");
            }

        }

        private async Task ExecuteUploadCommand()
        {
            try
            {
                this.IsBusy = true;
                var location = await Geolocation.GetLastKnownLocationAsync();
                VersionTracking.Track();
                var appVersion = VersionTracking.CurrentVersion;

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
                        VersioneApp = appVersion,
                        Prelievi = Mapper.Map<List<PrelievoLatteDto>>(prelievi)
                    };

                    // chiamata REST upload dati
                    if(this.restService.Upload(uploadDto).Result)
                    {
                        // chiusura giri 
                        foreach (var giro in giri)
                        {
                            giro.DataUpload = DateTime.Now;
                            this.giriService.UpdateItemAsync(giro).Wait();
                        }

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
                await this.page.DisplayAlert("Error", exc.Message, "OK");
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
                await this.page.DisplayAlert("Error", exc.Message, "OK");
            }

        }

        #endregion
    }
}
