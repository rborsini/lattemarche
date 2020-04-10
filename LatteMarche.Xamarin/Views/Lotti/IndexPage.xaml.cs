using LatteMarche.Xamarin.Models;
using LatteMarche.Xamarin.ViewModels.Lotti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LatteMarche.Xamarin.Views.Lotti
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IndexPage : ContentPage
    {
        #region Fields

        private IndexViewModel viewModel;

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
            var item = args.SelectedItem as Lotto;
            if (item == null)
                return;

            await Navigation.PushAsync(new Prelievi.IndexPage(new ViewModels.Prelievi.IndexViewModel(Navigation, this, item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadItemsCommand.Execute(null);
        }

        #endregion

    }
}