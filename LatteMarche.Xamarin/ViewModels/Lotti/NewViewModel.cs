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

        private ObservableCollection<Giro> giri;

        private IDataStore<Lotto, string> lottiDataStore => DependencyService.Get<IDataStore<Lotto, string>>();
        private ITrasportatoriService trasportatoriDataStore => DependencyService.Get<ITrasportatoriService>();
        private IGiriService giriDataStore => DependencyService.Get<IGiriService>();

        #endregion

        #region Properties

        public Lotto Item { get; set; }

        public ObservableCollection<Giro> Giri
        {
            get { return this.giri; }
            set { SetProperty<ObservableCollection<Giro>>(ref this.giri, value); }
        }

        public Giro GiroSelezionato { get; set; }

        public Command LoadItemsCommand { get; set; }

        public Command SaveCommand { get; set; }

        #endregion

        #region Constructor

        public NewViewModel(INavigation navigation, Page page)
            : base(navigation, page)
        {
            this.Title = "Nuovo lotto";

            this.Item = new Lotto();
            this.Item.Id = Guid.NewGuid().ToString();

            this.LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            this.SaveCommand = new Command(async () => await ExecuteSaveCommand());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Caricamento elenco lotti
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteLoadItemsCommand()
        {
            if (this.IsBusy)
                return;

            this.IsBusy = true;

            try
            {
                await Task.Run(() =>
                {
                    var trasportatore = this.trasportatoriDataStore.GetSelected().Result;
                    var giriList = this.giriDataStore.GetGiriTrasportatore(trasportatore.Id).Result;

                    this.Giri = new ObservableCollection<Giro>(giriList);
                });

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        /// <summary>
        /// Salvataggio nuovo lotto
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteSaveCommand()
        {
            try
            {
                this.IsBusy = true;

                await Task.Run(() =>
                {
                    this.Item.Codice = $"{this.GiroSelezionato.CodiceGiro}{DateTime.Now:ddMMyyyyHHmm}";
                    this.Item.IdGiro = this.GiroSelezionato.Id;

                    lottiDataStore.AddItemAsync(this.Item);
                });

                this.IsBusy = false;
                Debug.WriteLine("Lotto salvato");
                await navigation.PopAsync();
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
