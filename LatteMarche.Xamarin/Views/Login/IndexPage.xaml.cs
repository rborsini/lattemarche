using LatteMarche.Xamarin.ViewModels.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LatteMarche.Xamarin.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IndexPage : ContentPage
    {
        private IndexViewModel viewModel;

        public IndexPage()
        {
            InitializeComponent();

            BindingContext = this.viewModel = new IndexViewModel(Navigation, this);
        }
    }
}