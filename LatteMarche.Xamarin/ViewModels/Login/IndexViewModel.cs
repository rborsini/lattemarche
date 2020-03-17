using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LatteMarche.Xamarin.ViewModels.Login
{
    public class IndexViewModel : BaseViewModel
    {
        private const string USER = "02102002";
        private const string PWD = "giorgia2";

        #region Fields

        private IRestService restService => DependencyService.Get<IRestService>();

        private INavigation navigation;
        private Page page;

        #endregion

        #region Properties

        public string Username { get; set; }

        public string Password { get; set; }

        public Command LoginCommand { get; set; }

        #endregion

        #region Constructor

        public IndexViewModel(INavigation navigation, Page page)
        {
            this.navigation = navigation;
            this.page = page;

            this.Username = USER;
            this.Password = PWD;
            this.LoginCommand = new Command(async () => await ExecuteLoginCommand());
        }

        #endregion

        #region Methods

        private async Task ExecuteLoginCommand()
        {
            var isLogged = await this.restService.GetToken(USER, PWD);

            var msg = isLogged ? "Accesso riuscito!" : "Username o password errati";

            await this.page.DisplayAlert("Info", msg, "OK");




        }

        #endregion
    }
}
