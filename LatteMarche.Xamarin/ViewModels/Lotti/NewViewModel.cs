using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;

namespace LatteMarche.Xamarin.ViewModels.Lotti
{
    public class NewViewModel : BaseViewModel
    {
        #region Fields

        private IDataStore<Lotto, string> lottiDataStore => DependencyService.Get<IDataStore<Lotto, string>>();
        private ITrasportatoriService trasportatoriDataStore => DependencyService.Get<ITrasportatoriService>();
        private IGiriService giriDataStore => DependencyService.Get<IGiriService>();

        #endregion

        #region Properties

        public Lotto Item { get; set; }

        public ObservableCollection<Giro> Giri { get; set; }

        public Giro GiroSelezionato { get; set; }

        public Command SaveCommand { get; set; }

        #endregion

        #region Constructor

        public NewViewModel(INavigation navigation, Page page)
            : base(navigation, page)
        {
            this.Title = "Nuovo lotto";

            this.Item = new Lotto();
            this.Item.Id = Guid.NewGuid().ToString();

            var trasportatore = this.trasportatoriDataStore.GetSelected().Result;
            var giriList = this.giriDataStore.GetGiriTrasportatore(trasportatore.Id).Result;

            this.Giri = new ObservableCollection<Giro>(giriList);

            this.SaveCommand = new Command(async () => await ExecuteSaveCommand());
        }

        #endregion

        #region Methods

        private async Task ExecuteSaveCommand()
        {
            try
            {
                this.Item.Codice = $"{this.GiroSelezionato.CodiceGiro}{DateTime.Now:ddMMyyyyHHmm}";

                await lottiDataStore.AddItemAsync(this.Item);
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
