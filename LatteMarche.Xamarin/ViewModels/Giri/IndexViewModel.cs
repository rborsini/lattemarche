using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Views.Giri;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

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
            this.Title = "Giri";
            this.Items = new ObservableCollection<Giro>();
            this.NoData = true;
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
            this.NoData = false;
            try
            {
                await Task.Run(() =>
                {
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
