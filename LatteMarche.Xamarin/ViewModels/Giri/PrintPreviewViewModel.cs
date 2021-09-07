using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Zebra.Interfaces;
using LatteMarche.Xamarin.Zebra.Models;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Sentry;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace LatteMarche.Xamarin.ViewModels.Giri
{
    public class PrintPreviewViewModel : BaseViewModel
    {
        #region Fields

        private RegistroRaccolta registroRaccolta;

        private IStampantiService stampantiService => DependencyService.Get<IStampantiService>();

        #endregion

        #region Properties

        public RegistroRaccolta RegistroRaccolta
        {
            get { return this.registroRaccolta; }
            set { SetProperty(ref this.registroRaccolta, value); }
        }

        public Command PrintCommand { get; set; }

        #endregion

        #region Constructor

        public PrintPreviewViewModel(INavigation navigation, Page page, RegistroRaccolta registroRaccolta)
            : base(navigation, page)
        {
            this.navigation = navigation;
            this.page = page;
            this.RegistroRaccolta = registroRaccolta;

            this.PrintCommand = new Command(async () => await ExecutePrintCommand());
        }

        #endregion

        #region Methods

        private async Task ExecutePrintCommand()
        {
            var choices = new string[] { "1", "2", "3", "4", "5" };
            var index = await MaterialDialog.Instance.SelectChoiceAsync(title: "Numero copie", choices: choices);

            if (index == -1)
                return;

            var input = choices[index];

            var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Stampa in corso", lottieAnimation: "LottieLogo1.json");

            this.registroRaccolta.NumeroCopie = Convert.ToInt32(input);

            try
            {

                var stampante = this.stampantiService.GetDefaultAsync().Result;

                if (stampante == null)
                    throw new Exception("Nessuna stampante associata");

                var printer = DependencyService.Get<IPrinter>();
                printer.MacAddress = stampante.MacAddress;

                await printer.PrintLabel(this.registroRaccolta);
                await loadingDialog.DismissAsync();

                Analytics.TrackEvent("Stampa ricevuta consegna");
                SentrySdk.CaptureMessage("Stampa ricevuta consegna", Sentry.Protocol.SentryLevel.Info);

                await this.page.DisplayAlert("Info", "Stampa effettuata", "OK");
            }
            catch (Exception exc)
            {
                this.IsBusy = false;
                //await loadingDialog.DismissAsync();

                SentrySdk.CaptureException(exc);
                Crashes.TrackError(exc);

                await this.page.DisplayAlert("Errore", "Si è verificato un errore imprevisto. Contattare l'amministratore", "OK");
            }
        }

        #endregion
    }
}
