using LatteMarche.Xamarin.ViewModels.Giri;
using LatteMarche.Xamarin.Zebra.Models;
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
    public partial class PrintPreviewPage : ContentPage
    {
        #region Fields

        private PrintPreviewViewModel viewModel;

        #endregion

        #region Constructor

        public PrintPreviewPage(RegistroRaccolta registroRaccolta)
        {
            InitializeComponent();

            BindingContext = this.viewModel = new PrintPreviewViewModel(Navigation, this, registroRaccolta);
        }

        #endregion

    }
}