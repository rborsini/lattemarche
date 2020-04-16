using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Models;
using LatteMarche.Xamarin.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace LatteMarche.Xamarin.ViewModels.Synch
{
    public class IndexViewModel : BaseViewModel
    {
        private const string USER = "02102002";
        private const string PWD = "giorgia2";

        #region Fields

        private IRestService restService => DependencyService.Get<IRestService>();
        private IDataStore<Allevamento, int> allevamentiDataStore => DependencyService.Get<IDataStore<Allevamento, int>>();
        private IDataStore<AutoCisterna, int> autocisterneDataStore => DependencyService.Get<IDataStore<AutoCisterna, int>>();
        private IDataStore<Acquirente, int> acquirentiDataStore => DependencyService.Get<IDataStore<Acquirente, int>>();
        private IDataStore<Destinatario, int> destinatariDataStore => DependencyService.Get<IDataStore<Destinatario, int>>();
        private IDataStore<Giro, int> giriDataStore => DependencyService.Get<IDataStore<Giro, int>>();
        private IDataStore<GiroItem, string> giroItemsDataStore => DependencyService.Get<IDataStore<GiroItem, string>>();
        private IDataStore<TipoLatte, int> tipiLatteDataStore => DependencyService.Get<IDataStore<TipoLatte, int>>();
        private IDataStore<Trasportatore, int> trasportatoriDataStore => DependencyService.Get<IDataStore<Trasportatore, int>>();

        #endregion

        #region Properties

        public Command SynchCommand { get; set; }

        public Command ExportCommand { get; set; }

        #endregion

        #region Constructor

        public IndexViewModel(INavigation navigation, Page page)
            : base(navigation, page)
        {
            this.navigation = navigation;
            this.page = page;
            this.SynchCommand = new Command(async () => await ExecuteSynchCommand());
            this.ExportCommand = new Command(async () => await ExecuteExportCommand());
        }

        #endregion

        #region Methods

        private async Task ExecuteSynchCommand()
        {
            try
            {
                this.IsBusy = true;

                await Task.Run(() =>
                {



                    // trasportatori
                    var trasportatori = this.restService.GetTrasportatori().Result;
                    trasportatori[0].Selezionato = true;
                    this.trasportatoriDataStore.DeleteAllItemsAsync();
                    this.trasportatoriDataStore.AddRangeItemAsync(trasportatori);

                    // autocisterne
                    var autocisterne = this.restService.GetAutoCisterne().Result;
                    this.autocisterneDataStore.DeleteAllItemsAsync();
                    this.autocisterneDataStore.AddRangeItemAsync(autocisterne);

                    // allevamenti
                    var allevamenti = this.restService.GetAllevamenti().Result;
                    this.allevamentiDataStore.DeleteAllItemsAsync();
                    this.allevamentiDataStore.AddRangeItemAsync(allevamenti);

                    // tipi latte
                    var tipiLatte = this.restService.GetTipiLatte().Result;
                    this.tipiLatteDataStore.DeleteAllItemsAsync();
                    this.tipiLatteDataStore.AddRangeItemAsync(tipiLatte);

                    // acquirenti
                    var acquirenti = this.restService.GetAcquirenti().Result;
                    this.acquirentiDataStore.DeleteAllItemsAsync();
                    this.acquirentiDataStore.AddRangeItemAsync(acquirenti);

                    // destinatari
                    var destinatari = this.restService.GetDestinatari().Result;
                    this.destinatariDataStore.DeleteAllItemsAsync();
                    this.destinatariDataStore.AddRangeItemAsync(destinatari);

                    // giri
                    var giri = this.restService.GetGiri().Result;
                    this.giriDataStore.DeleteAllItemsAsync();
                    this.giriDataStore.AddRangeItemAsync(giri);

                    // giro items
                    var giroItems = this.restService.GetGiro(giri[0].Id).Result;
                    this.giroItemsDataStore.DeleteAllItemsAsync();

                    foreach (var item in giroItems)
                        item.Id = Guid.NewGuid().ToString();

                    this.giroItemsDataStore.AddRangeItemAsync(giroItems);
                });

                this.IsBusy = false;
                await this.page.DisplayAlert("Info", "Sincronizzazione avvenuta con successo", "OK");
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

                //await Task.Run(() =>
                //{
                //    var internalFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "database.db");
                //    DependencyService.Get<IFileSystem>().ExportDb(internalFile);
                //});

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
