using LatteMarche.Xamarin.Models;
using LatteMarche.Xamarin.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LatteMarche.Xamarin.ViewModels.Prelievi
{
    public class EditViewModel : BaseViewModel
    {
        #region Fields

        private INavigation navigation;
        private IDataStore<Prelievo> dataStore => DependencyService.Get<IDataStore<Prelievo>>();

        #endregion

        #region Properties

        public Prelievo Item { get; set; }
        
        public Command DeleteItemCommand { get; set; }

        public Command SaveItemCommand { get; set; }

        #endregion

        #region Constructor

        public EditViewModel(INavigation navigation, Prelievo item = null)
        {
            base.Title = item?.Scomparto;
            this.Item = item;
            this.navigation = navigation;

            this.SaveItemCommand = new Command(async () => await ExecuteSaveItemCommand());
            this.DeleteItemCommand = new Command(async () => await ExecuteDeleteItemCommand());            
        }

        #endregion

        #region Methods

        /// <summary>
        /// Salvataggio prelievo
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteSaveItemCommand()
        {
            await dataStore.UpdateItemAsync(Item);
            await navigation.PopAsync();
        }

        /// <summary>
        /// Eliminazione prelievo
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteDeleteItemCommand()
        {
            await dataStore.DeleteItemAsync(Item.Id);
            await navigation.PopAsync();
        }

        #endregion

    }
}
