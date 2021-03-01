using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.ViewModels.Giri;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LatteMarche.Xamarin.Views.Giri
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IndexPage : ContentPage
    {
        #region Fields

        private IndexViewModel viewModel;
        private IGiriService giriService => DependencyService.Get<IGiriService>();

        #endregion

        #region Constructor

        public IndexPage()
        {
            InitializeComponent();

            BindingContext = this.viewModel = new IndexViewModel(Navigation, this);
        }

        #endregion

        #region Methods

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as ItemViewModel;
            if (item == null)
                return;

            var giro = this.giriService.GetItemAsync(item.Id).Result;

            await Navigation.PushAsync(new Prelievi.IndexPage(new ViewModels.Prelievi.IndexViewModel(Navigation, this, giro)));

            // Manually deselect item.
            //ItemsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.viewModel.LoadItemsCommand.Execute(null);
        }

        #endregion

    }
}