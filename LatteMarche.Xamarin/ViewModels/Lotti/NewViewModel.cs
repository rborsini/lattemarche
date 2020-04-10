using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LatteMarche.Xamarin.ViewModels.Lotti
{
    public class NewViewModel : BaseViewModel
    {
        #region Fields

        private IDataStore<Lotto, string> dataStore => DependencyService.Get<IDataStore<Lotto, string>>();

        #endregion

        #region Properties

        public Lotto Item { get; set; }

        public Command SaveCommand { get; set; }

        #endregion

        #region Constructor

        public NewViewModel(INavigation navigation, Page page)
            : base(navigation, page)
        {
            this.Title = "Nuovo lotto";

            this.Item = new Lotto();
            this.Item.Id = Guid.NewGuid().ToString();
            this.Item.Codice = $"M1{DateTime.Now:ddMMyyyyHHmm}"; 

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
