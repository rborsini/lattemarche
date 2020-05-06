using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Views.Giri;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace LatteMarche.Xamarin.ViewModels.Giri
{
    public class IndexViewModel : BaseViewModel
    {
        #region Fields

        private IGiriService giriService => DependencyService.Get<IGiriService>();

        #endregion

        #region Properties

        public ObservableCollection<Giro> Items { get; set; }

        public Command AddCommand { get; set; }

        public Command LoadItemsCommand { get; set; }

        #endregion

        #region Constructor

        public IndexViewModel(INavigation navigation, Page page)
            : base(navigation, page)
        {
            this.Items = new ObservableCollection<Giro>();
            this.NoData = false;
            this.AddCommand = new Command(async () => await ExecuteAddCommand());
            this.LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Apertura pagina nuovo lotto
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteAddCommand()
        {


            //try
            //{
            //    Analytics.TrackEvent("My custom event");
            //    Crashes.GenerateTestCrash();
            //    // your code here.
            //}
            //catch (Exception exception)
            //{
            //    var properties = new Dictionary<string, string> {
            //        { "Category", "Music" },
            //        { "Wifi", "On" }
            //    };
            //    Crashes.TrackError(exception, properties);
            //}

            await this.navigation.PushAsync(new NewPage(new NewViewModel(this.navigation, this.page)));
        }

        /// <summary>
        /// Caricamento elenco lotti
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteLoadItemsCommand()
        {
            if (this.IsBusy)
                return;

            this.IsBusy = true;

            var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Giri in caricamento", lottieAnimation: "LottieLogo1.json");
            this.NoData = false;

            try
            {
                await Task.Run(async () =>
                {
                    await loadingDialog.DismissAsync();
                    this.Items.Clear();
                    var items = this.giriService.GetItemsAsync().Result;
                    foreach (var item in items)
                    {
                        this.Items.Add(item);
                    }

                    if (this.Items.Count == 0)
                    {
                        this.NoData = true;
                    }
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

        #endregion

    }
}
