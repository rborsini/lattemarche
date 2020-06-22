using AutoMapper;
using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Enums;
using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Rest.Dtos;
using LatteMarche.Xamarin.Rest.Interfaces;
using LatteMarche.Xamarin.Views.Prelievi;
using LatteMarche.Xamarin.Zebra.Interfaces;
using LatteMarche.Xamarin.Zebra.Models;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Sentry;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.Models;
using XF.Material.Forms.UI.Dialogs;

namespace LatteMarche.Xamarin.ViewModels.Prelievi
{
    public class IndexViewModel : BaseViewModel
    {

        #region Fields

        private Giro giro;

        private IPrelieviService prelieviService => DependencyService.Get<IPrelieviService>();

        private ObservableCollection<ItemViewModel> prelievi;

        #endregion

        #region Properties

        public bool IsReadOnly => this.giro != null && this.giro.DataConsegna.HasValue;

        public bool IsEditable => !this.IsReadOnly;

        public ObservableCollection<ItemViewModel> Prelievi 
        {
            get { return this.prelievi; }
            set { SetProperty<ObservableCollection<ItemViewModel>>(ref this.prelievi, value); }
        }

        public Command LoadItemsCommand { get; set; }

        public Command AddCommand { get; set; }

        public Command PrintCommand { get; set; }


        #endregion

        #region Constructor

        public IndexViewModel(INavigation navigation, Page page, Giro giro)
            : base(navigation, page)
        {
            
            this.Prelievi = new ObservableCollection<ItemViewModel>();
            this.giro = giro;

            this.Title = this.giro.Titolo;

            this.AddCommand = new Command(async () => await ExecuteAddCommand(), canExecute: () => { return !this.IsReadOnly; });
            this.LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

        }

        #endregion

        #region Methods

        /// <summary>
        /// Caricamento elenco prelievi presenti nel giro
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteLoadItemsCommand()
        {
            if (this.IsBusy)
                return;

            this.IsBusy = true;
            this.NoData = false;

            var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Prelievi in caricamento", lottieAnimation: "LottieLogo1.json");

            try
            {
                await Task.Run(() =>
                {
                    this.Prelievi.Clear();
                    var prelievi = this.prelieviService.GetByGiro(this.giro.Id).Result;
                    var items = Mapper.Map<List<ItemViewModel>>(prelievi.ToList());

                    items.ForEach(i => { i.ReadOnly = this.giro.DataConsegna.HasValue; });

                    this.Prelievi = new ObservableCollection<ItemViewModel>(items);

                    this.NoData = this.Prelievi.Count == 0;

                    foreach(var item in items)
                    {
                        item.OnItem_Deleting += Item_OnItem_Deleting;
                    }
                });

                (this.AddCommand as Command).ChangeCanExecute();
                (this.PrintCommand as Command).ChangeCanExecute();
            }
            catch (Exception exc)
            {
                SentrySdk.CaptureException(exc);
                Crashes.TrackError(exc);
            }
            finally
            {
                await loadingDialog.DismissAsync();
                this.IsBusy = false;
            }
        }

        /// <summary>
        /// Rimozione prelievo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Item_OnItem_Deleting(object sender, EventArgs e)
        {
            IMaterialModalPage loadingDialog = null;
            try
            {
                bool reply = await this.page.DisplayAlert("Attenzione", $"Sei sicuro di voler eliminare il prelievo selezionato?", "Si", "No");
                if (reply == false)
                {
                    return;
                }

                loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Rimozione prelievo", lottieAnimation: "LottieLogo1.json");
                await Task.Run(() =>
                {
                    var item = sender as ItemViewModel;
                    this.prelieviService.DeleteItemAsync(item.Id).Wait();
                    this.Prelievi.Remove(item);
                });

                await loadingDialog.DismissAsync();
            }
            catch (Exception exc)
            {
                await loadingDialog.DismissAsync();

                SentrySdk.CaptureException(exc);
                Crashes.TrackError(exc);

                await this.page.DisplayAlert("Errore", exc.Message, "OK");
            }
        }


        /// <summary>
        /// Apertura pagina nuovo prelievo
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteAddCommand()
        {
            await this.navigation.PushAsync(new EditPage(new EditViewModel(this.navigation, this.page, this.giro.Id, "")));
        }


        #endregion

    }
}
