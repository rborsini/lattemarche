using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Models;
using LatteMarche.Xamarin.Services;
using LatteMarche.Xamarin.Views.Prelievi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LatteMarche.Xamarin.ViewModels.Prelievi
{
    public class IndexViewModel : BaseViewModel
    {

        #region Fields

        private IDataStore<Prelievo, string> dataStore => DependencyService.Get<IDataStore<Prelievo, string>>();

        #endregion

        #region Properties

        public ObservableCollection<Prelievo> Items { get; set; }

        public Command LoadItemsCommand { get; set; }

        #endregion

        #region Constructor

        public IndexViewModel()
        {
            this.Title = "Browse";
            this.Items = new ObservableCollection<Prelievo>();

            this.LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewPage, Prelievo>(this, "AddItem", async (obj, item) =>
            {
                this.Items.Add(item as Prelievo);
                await this.dataStore.AddItemAsync(item);
            });
        }

        #endregion

        #region Methods

        private async Task ExecuteLoadItemsCommand()
        {
            if (this.IsBusy)
                return;

            this.IsBusy = true;

            try
            {
                this.Items.Clear();

                var items = await this.dataStore.GetItemsAsync(true);
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
