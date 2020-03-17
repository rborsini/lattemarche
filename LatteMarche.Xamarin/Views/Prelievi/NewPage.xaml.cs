using LatteMarche.Xamarin.Models;
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
    public partial class NewPage : ContentPage
    {
        #region Fields

        public Prelievo Item { get; set; }

        #endregion

        #region Constructor

        public NewPage()
        {
            InitializeComponent();

            this.Item = new Prelievo() 
            { 
                Id = Guid.NewGuid().ToString(),
                DataPrelievo = DateTime.Today
            };

            BindingContext = this;
        }

        #endregion

        #region Methods

        private async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopModalAsync();
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        #endregion

    }
}