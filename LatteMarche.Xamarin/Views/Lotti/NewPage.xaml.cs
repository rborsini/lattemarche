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
    public partial class NewPage : ContentPage
    {
        #region Fields

        private NewViewModel viewModel;

        #endregion

        #region Constructors

        public NewPage(NewViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        #endregion

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            this.viewModel.LoadItemsCommand.Execute(null);
        }
    }
}