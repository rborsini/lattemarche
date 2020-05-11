using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.ViewModels.Prelievi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace LatteMarche.Xamarin.Views.Prelievi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IndexPage : ContentPage
    {
        #region Fields

        private IndexViewModel viewModel;

        #endregion

        #region Constructor

        public IndexPage(IndexViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        #endregion

        #region Methods

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as ItemViewModel;
            if (item == null)
                return;

            var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Caricamento", lottieAnimation: "LottieLogo1.json");

            await Navigation.PushAsync(new EditPage(new EditViewModel(Navigation, this, item.IdGiro.Value, item.Id)));
            
            await loadingDialog.DismissAsync();
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