using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Db.Interfaces;

namespace LatteMarche.Xamarin.ViewModels.Giri
{
    public class NewViewModel : BaseViewModel
    {
        #region Fields

        private ObservableCollection<TemplateGiro> modelli;

        private ITemplateGiroService templateService => DependencyService.Get<ITemplateGiroService>();

        private IGiriService giriService => DependencyService.Get<IGiriService>();

        #endregion

        #region Properties

        public Giro Item { get; set; }

        public ObservableCollection<TemplateGiro> Modelli
        {
            get { return this.modelli; }
            set { SetProperty<ObservableCollection<TemplateGiro>>(ref this.modelli, value); }
        }

        public TemplateGiro ModelloSelezionato { get; set; }

        public Command LoadItemsCommand { get; set; }

        public Command SaveCommand { get; set; }

        #endregion

        #region Constructor

        public NewViewModel(INavigation navigation, Page page)
            : base(navigation, page)
        {
            this.Title = "Nuovo giro";

            this.Item = new Giro();

            this.LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            this.SaveCommand = new Command(async () => await ExecuteSaveCommand());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Caricamento elenco modelli
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
                    var templateList = this.templateService.GetItemsAsync().Result;
                    this.Modelli = new ObservableCollection<TemplateGiro>(templateList);
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
        /// Salvataggio nuovo giro
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteSaveCommand()
        {
            try
            {
                this.IsBusy = true;

                await Task.Run(() =>
                {
                    this.Item.IdTemplateGiro = this.ModelloSelezionato.Id;
                    this.Item.DataCreazione = DateTime.Now;
                    this.Item.Titolo = this.ModelloSelezionato.Descrizione;

                    this.giriService.AddItemAsync(this.Item).Wait();
                });

                this.IsBusy = false;
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
