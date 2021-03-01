using AutoMapper;
using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Rest.Interfaces;
using Microsoft.AppCenter.Crashes;
using Sentry;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace LatteMarche.Xamarin.ViewModels.Trasbordi
{
    public class IndexViewModel : BaseViewModel
    {
        #region Fields

        private IDevice device = DependencyService.Get<IDevice>();
        
        private ITrasbordiService trasbordiService => DependencyService.Get<ITrasbordiService>();
        private IRestService restService => DependencyService.Get<IRestService>();

        #endregion

        #region Properties

        public ObservableCollection<ItemViewModel> Items { get; set; }

        public Command LoadItemsCommand { get; set; }

        #endregion

        #region Constructor

        public IndexViewModel(INavigation navigation, Page page)
            : base(navigation, page)
        {
            this.Items = new ObservableCollection<ItemViewModel>();
            this.NoData = false;
            this.LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());            
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

            var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Caricamento trasbordi attivi", lottieAnimation: "LottieLogo1.json");
            this.NoData = false;

            try
            {
                await Task.Run(() =>
                {
                    this.Items.Clear();
                    var imei = this.device.GetIdentifier();
                    var trasbordi = this.restService.DownloadTrasbordi(imei).Result;
                    foreach (var trasbordo in trasbordi)
                    {
                        var item = Mapper.Map<ItemViewModel>(trasbordo);
                        item.OnItem_Importing += Item_OnItem_Importing;
                        this.Items.Add(item);
                    }
                    this.NoData = this.Items.Count == 0;
                });
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
        /// Evento importazione trasbordo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Item_OnItem_Importing(object sender, EventArgs e)
        {
            var item = sender as ItemViewModel;
            if (this.IsBusy)
                return;

            this.IsBusy = true;

            var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Importazione trasbordo", lottieAnimation: "LottieLogo1.json");
            this.NoData = false;

            try
            {
                await Task.Run(() =>
                {
                    this.trasbordiService.Import(item.Dto).Wait();
                    this.restService.ChiudiTrasbordo(item.Dto.Id);
                });

                await ExecuteLoadItemsCommand();
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



        #endregion

    }
}
