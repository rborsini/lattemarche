using LatteMarche.Xamarin.ViewModels.Prelievi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI;

namespace LatteMarche.Xamarin.Views.Prelievi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditPage : ContentPage
    {
        private EditViewModel viewModel;

        public EditPage(EditViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            this.viewModel.LoadCommand.Execute(null);
        }

        private void Kg_Unfocused(object sender, FocusEventArgs e)
        {
            this.viewModel.OnKgChanged((sender as MaterialTextField).Text);
        }

        private void Lt_Unfocused(object sender, FocusEventArgs e)
        {
            this.viewModel.OnLtChanged((sender as MaterialTextField).Text);
        }
    }
}