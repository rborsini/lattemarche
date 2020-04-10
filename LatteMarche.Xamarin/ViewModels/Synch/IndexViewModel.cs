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

                var allevamenti = await this.restService.GetAllevamenti();

                await this.allevamentiDataStore.DeleteAllItemsAsync();
                await this.allevamentiDataStore.AddRangeItemAsync(allevamenti);

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
                var bytes = System.IO.File.ReadAllBytes(internalFile);
                
                var folderPath = DependencyService.Get<IFileSystem>().GetExternalStorage();
                var fileCopyName = Path.Combine(folderPath, $"Database_{DateTime.Now:dd-MM-yyyy_HH-mm-ss-tt}.db");

                System.IO.File.WriteAllBytes(fileCopyName, bytes);

                this.IsBusy = false;
                await this.page.DisplayAlert("Info", "Sincronizzazione avvenuta con successo", "OK");
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
