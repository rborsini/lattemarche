using LatteMarche.Xamarin.ViewModels.Synch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LatteMarche.Xamarin.Views.Synch
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        private RegisterViewModel viewModel;

        public RegisterPage()
        {
            InitializeComponent();

            BindingContext = this.viewModel = new RegisterViewModel(Navigation, this);
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            this.viewModel.LoadCommand.Execute(null);
        }
    }
}