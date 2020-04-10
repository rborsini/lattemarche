using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Models;
using LatteMarche.Xamarin.Views.Lotti;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LatteMarche.Xamarin.ViewModels.Lotti
{
    public class IndexViewModel : BaseViewModel
    {
        #region Fields

        private IDataStore<Lotto, string> dataStore => DependencyService.Get<IDataStore<Lotto, string>>();

        #endregion

        #region Properties

        public ObservableCollection<Lotto> Items { get; set; }

        public Command AddCommand { get; set; }

        public Command LoadItemsCommand { get; set; }

        #endregion

        #region Constructor

        public IndexViewModel(INavigation navigation, Page page)
            : base(navigation, page)
        {
            this.Title = "Lotti";
            this.Items = new ObservableCollection<Lotto>();

            this.AddCommand = new Command(async () => await ExecuteAddCommand());
            this.LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        #endregion

        #region Methods

        private async Task ExecuteAddCommand()
        {
            await this.navigation.PushAsync(new NewPage(new NewViewModel(this.navigation, this.page)));
        }

        private async Task ExecuteLoadItemsCommand()
        {
            if (this.IsBusy)
                return;

            this.IsBusy = true;

            try
            {
                this.Items.Clear();
                var items = await this.dataStore.GetItemsAsync();
                foreach (var item in items)
                {
                    this.Items.Add(item);
                }
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
