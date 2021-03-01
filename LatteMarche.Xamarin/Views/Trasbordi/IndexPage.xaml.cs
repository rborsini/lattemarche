using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LatteMarche.Xamarin.Views.Trasbordi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IndexPage : ContentPage
    {
        private ViewModels.Trasbordi.IndexViewModel viewModel;

        public IndexPage()
        {
            InitializeComponent();

            BindingContext = this.viewModel = new ViewModels.Trasbordi.IndexViewModel(Navigation, this);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.viewModel.LoadItemsCommand.Execute(null);
        }

    }
}