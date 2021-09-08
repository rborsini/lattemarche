using LatteMarche.Xamarin.ViewModels.Prelievi;
using LatteMarche.Xamarin.Zebra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LatteMarche.Xamarin.Views.Prelievi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrintPreviewPage : ContentPage
    {
        #region Fields

        private PrintPreviewViewModel viewModel;

        #endregion

        #region Constructor

        public PrintPreviewPage(RegistroConsegna registroConsegna)
        {
            InitializeComponent();

            BindingContext = this.viewModel = new PrintPreviewViewModel(Navigation, this, registroConsegna);
        }

        #endregion
    }
}