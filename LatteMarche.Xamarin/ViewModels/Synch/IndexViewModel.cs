using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Models;
using LatteMarche.Xamarin.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

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

        #endregion

        #region Constructor

        public IndexViewModel(INavigation navigation, Page page)
            : base(navigation, page)
        {
            this.navigation = navigation;
            this.page = page;
            this.SynchCommand = new Command(async () => await ExecuteSynchCommand());
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

        #endregion
    }
}
