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

                // autocisterne
                var autocisterne = await this.restService.GetAutoCisterne();
                await this.autocisterneDataStore.DeleteAllItemsAsync();
                await this.autocisterneDataStore.AddRangeItemAsync(autocisterne);

                // trasportatori
                var trasportatori = await this.restService.GetTrasportatori();
                await this.trasportatoriDataStore.DeleteAllItemsAsync();
                await this.trasportatoriDataStore.AddRangeItemAsync(trasportatori);

                // allevamenti
                var allevamenti = await this.restService.GetAllevamenti();
                await this.allevamentiDataStore.DeleteAllItemsAsync();
                await this.allevamentiDataStore.AddRangeItemAsync(allevamenti);

                // tipi latte
                var tipiLatte = await this.restService.GetTipiLatte();
                await this.tipiLatteDataStore.DeleteAllItemsAsync();
                await this.tipiLatteDataStore.AddRangeItemAsync(tipiLatte);

                // acquirenti
                var acquirenti = await this.restService.GetAcquirenti();
                await this.acquirentiDataStore.DeleteAllItemsAsync();
                await this.acquirentiDataStore.AddRangeItemAsync(acquirenti);

                // destinatari
                var destinatari = await this.restService.GetDestinatari();
                await this.destinatariDataStore.DeleteAllItemsAsync();
                await this.destinatariDataStore.AddRangeItemAsync(destinatari);

                // giri
                var giri = await this.restService.GetGiri();
                await this.giriDataStore.DeleteAllItemsAsync();
                await this.giriDataStore.AddRangeItemAsync(giri);


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

                var internalFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "database.db");
                DependencyService.Get<IFileSystem>().ExportDb(internalFile);

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
