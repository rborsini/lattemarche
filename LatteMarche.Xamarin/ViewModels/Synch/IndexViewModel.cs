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

        private INavigation navigation;
        private Page page;

        #endregion

        #region Properties

        public Command SynchCommand { get; set; }

        #endregion

        #region Constructor

        public IndexViewModel(INavigation navigation, Page page)
        {
            this.navigation = navigation;
            this.page = page;
            this.SynchCommand = new Command(async () => await ExecuteSynchCommand());
        }

        #endregion

        #region Methods

        private async Task ExecuteSynchCommand()
        {
            var allevamenti = await this.restService.GetAllevamenti();

            try
            {
                await this.allevamentiDataStore.DeleteAllItemsAsync();
                await this.allevamentiDataStore.AddItemAsync(allevamenti[0]);
                //await this.allevamentiDataStore.AddRangeItemAsync(allevamenti);
                await this.page.DisplayAlert("Info", "Sincronizzazione avvenuta con successo", "OK");
            }
            catch(Exception exc)
            {
                await this.page.DisplayAlert("Error", exc.Message, "OK");
            }

            
        }

        #endregion
    }
}
