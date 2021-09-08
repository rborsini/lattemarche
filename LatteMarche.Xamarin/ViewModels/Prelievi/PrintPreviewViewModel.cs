using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Zebra.Interfaces;
using LatteMarche.Xamarin.Zebra.Models;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Sentry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace LatteMarche.Xamarin.ViewModels.Prelievi
{
    public class PrintPreviewViewModel : BaseViewModel
    {
        #region Fields

        private RegistroConsegna registroConsegna;

        private IStampantiService stampantiService => DependencyService.Get<IStampantiService>();

        #endregion

        #region Properties

        public RegistroConsegna RegistroConsegna
        {
            get { return this.registroConsegna; }
            set { SetProperty(ref this.registroConsegna, value); }
        }

        public Command PrintCommand { get; set; }

        #endregion

        #region Constructor

        public PrintPreviewViewModel(INavigation navigation, Page page, RegistroConsegna registroConsegna)
            : base(navigation, page)
        {
            this.navigation = navigation;
            this.page = page;
            this.RegistroConsegna = registroConsegna;

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

            this.registroConsegna.NumeroCopie = Convert.ToInt32(input);

            try
            {
                Debug.WriteLine("Print Command");
                this.IsBusy = true;

                var stampante = this.stampantiService.GetDefaultAsync().Result;
                if (stampante == null)
                {
                    await loadingDialog.DismissAsync();
                    await this.page.DisplayAlert("Attenzione", "Nessuna stampante selezionata", "OK");
                    return;
                }

                await Task.Run(() =>
                {
                    var printer = DependencyService.Get<IPrinter>();
                    printer.MacAddress = stampante.MacAddress;

                    printer.PrintLabel(registroConsegna);
                });

                this.IsBusy = false;
                await loadingDialog.DismissAsync();

                Analytics.TrackEvent("Stampa ricevuta raccolta");
                SentrySdk.CaptureMessage("Stampa ricevuta raccolta", Sentry.Protocol.SentryLevel.Info);

                await this.page.DisplayAlert("Info", "Stampa effettuata", "OK");
                await navigation.PopAsync();
            }
            catch (Exception exc)
            {
                this.IsBusy = false;
                await loadingDialog.DismissAsync();

                SentrySdk.CaptureException(exc);
                Crashes.TrackError(exc);

                await this.page.DisplayAlert("Errore", "Si è verificato un errore imprevisto. Contattare l'amministratore", "OK");
            }
        }

        #endregion
    }
}
