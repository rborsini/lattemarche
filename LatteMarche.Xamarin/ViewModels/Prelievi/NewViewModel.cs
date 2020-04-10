using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LatteMarche.Xamarin.ViewModels.Prelievi
{
    public class NewViewModel : BaseViewModel
    {
        #region Fields

        private IDataStore<Prelievo, string> dataStore => DependencyService.Get<IDataStore<Prelievo, string>>();

        #endregion

        #region Properties

        public Prelievo Item { get; set; }

        public Command SaveCommand { get; set; }

        #endregion

        #region Constructor

        public NewViewModel(INavigation navigation, Page page, Lotto lotto)
            : base(navigation, page)
        {
            this.Title = "Nuovo prelievo";

            this.Item = new Prelievo();
            this.Item.Id = Guid.NewGuid().ToString();
            this.Item.IdLotto = lotto.Id;

            this.SaveCommand = new Command(async () => await ExecuteSaveCommand());
        }

        #endregion

        #region Methods

        private async Task ExecuteSaveCommand()
        {
            try
            {
                await dataStore.AddItemAsync(Item);
                Debug.WriteLine("Lotto salvato");
                await navigation.PopAsync();
            }
            catch (Exception exc)
            {
                await this.page.DisplayAlert("Error", exc.Message, "OK");
            }

        }

        #endregion
    }
}
