using AutoMapper;
using LatteMarche.Xamarin.Db;
using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Interfaces;
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
        private const string USER = "02102002";
        private const string PWD = "giorgia2";

        #region Fields

        private IDevice device = DependencyService.Get<IDevice>();
        private IRestService restService => DependencyService.Get<IRestService>();
        private IEntityService<Allevamento, int> allevamentiService => DependencyService.Get<IEntityService<Allevamento, int>>();
        private IEntityService<AutoCisterna, int> autocisterneService => DependencyService.Get<IEntityService<AutoCisterna, int>>();
        private IEntityService<Acquirente, int> acquirentiService => DependencyService.Get<IEntityService<Acquirente, int>>();
        private IEntityService<Destinatario, int> destinatariService => DependencyService.Get<IEntityService<Destinatario, int>>();
        private IEntityService<TipoLatte, int> tipiLatteService => DependencyService.Get<IEntityService<TipoLatte, int>>();
        private IEntityService<Trasportatore, int> trasportatoriService => DependencyService.Get<IEntityService<Trasportatore, int>>();
        private IEntityService<TemplateGiro, int> templateGiriService => DependencyService.Get<IEntityService<TemplateGiro, int>>();

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

                await Task.Run(() =>
                {

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
