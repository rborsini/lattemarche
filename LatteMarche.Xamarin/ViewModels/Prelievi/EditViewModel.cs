using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Models;
using LatteMarche.Xamarin.Services;
using LatteMarche.Xamarin.Zebra;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;

namespace LatteMarche.Xamarin.ViewModels.Prelievi
{
    public class EditViewModel : BaseViewModel
    {
        #region Fields

        private IDataStore<Prelievo, string> dataStore => DependencyService.Get<IDataStore<Prelievo, string>>();

        #endregion

        #region Properties

        public Prelievo Item { get; set; }

        public Command PrintCommand { get; set; }

        public Command DeleteItemCommand { get; set; }

        public Command SaveItemCommand { get; set; }

        #endregion

        #region Constructor

        public EditViewModel(INavigation navigation, Page page, Prelievo item = null)
            : base(navigation, page)
        {
            this.Title = item?.Scomparto;
            this.Item = item;
            this.navigation = navigation;
            this.page = page;

            this.PrintCommand = new Command(async () => await ExecutePrintCommand());
            this.SaveItemCommand = new Command(async () => await ExecuteSaveItemCommand());
            this.DeleteItemCommand = new Command(async () => await ExecuteDeleteItemCommand());            
        }

        #endregion

        #region Methods

        private async Task ExecutePrintCommand()
        {
            Debug.WriteLine("Print Command");

            var printer = DependencyService.Get<IPrinter>();
            printer.MacAddress = "00:03:7A:30:B0:4D";

            var registroConsegna = new RegistroConsegna();

            registroConsegna.Acquirente = new Acquirente() { CAP = "63021", Comune = "AMANDOLA", SiglaProvincia = "AP", Indirizzo = "ZONA IND.LE PIAN DI CONTRO", RagioneSociale = "SIBILLA SOC.COOP.AGR.", Piva = "00100010446" };
            registroConsegna.Destinatario = new Destinatario() { CAP = "63021", Comune = "AMANDOLA", SiglaProvincia = "AP", Indirizzo = "LOC. PIANDICONTRO", RagioneSociale = "FATTORIE MARCHIGIANE CONS.COOP.", P_IVA = "00433920410" };
            registroConsegna.Trasportatore = new Trasportatore() { TargaAutomezzo = "CD182ZZ", RagioneSociale = "LATTE MARCHE SOC.COOP.AGR", Indirizzo = "VIA S.TOTTI, 7 - 60100 ANCONA (AN)", P_IVA = "008880425" };
            registroConsegna.Giro = new Giro() { Denominazione = "PESARO-ANCONA" };
            registroConsegna.Data = DateTime.Now;

            registroConsegna.Prelievo = new Prelievo()
            {
                Id = Guid.NewGuid().ToString(),
                DataConsegna = DateTime.Today.AddDays(1),
                DataPrelievo = DateTime.Today,
                DataUltimaMungitura = DateTime.Today.AddDays(-1),
                NumeroMungiture = 1,
                Quantita_kg = Convert.ToDecimal(5.5),
                Quantita_lt = Convert.ToDecimal(6.6),
                Scomparto = "1",
                Temperatura = Convert.ToDecimal(26.7),
                Allevamento = new Allevamento() { RagioneSociale = "TRIONFI HONORATI ANTONIO S.R.L. (testo per farlo lungo)", P_IVA = "00136660420", Prov = "AN" },
                TipoLatte = new TipoLatte() { DescrizioneBreve = "QM-AQ", Descrizione = "QM-ALTA QUALITA'" }
            };

            registroConsegna.Comunicazione = "Comunicazione di prova lorem ipsum";

            try
            {
                await printer.PrintLabel(registroConsegna);
                await this.page.DisplayAlert("Info", "Stampa effettuata", "OK");
            }
            catch (Exception exc)
            {
                await this.page.DisplayAlert("Error", exc.Message, "OK");
            }

        }


        /// <summary>
        /// Salvataggio prelievo
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteSaveItemCommand()
        {
            try
            {
                await dataStore.UpdateItemAsync(Item);
                await navigation.PopAsync();
            }
            catch(Exception exc)
            {
                await this.page.DisplayAlert("Error", exc.Message, "OK");
            }
        }

        /// <summary>
        /// Eliminazione prelievo
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteDeleteItemCommand()
        {
            await dataStore.DeleteItemAsync(Item.Id);
            await navigation.PopAsync();
        }

        #endregion

    }
}
