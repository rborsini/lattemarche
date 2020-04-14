using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Models;
using LatteMarche.Xamarin.Services;
using LatteMarche.Xamarin.Views.Prelievi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LatteMarche.Xamarin.ViewModels.Prelievi
{
    public class IndexViewModel : BaseViewModel
    {

        #region Fields

        private Lotto lotto;

        private ILottiService lottiService => DependencyService.Get<ILottiService>();

        #endregion

        #region Properties

        public ObservableCollection<Prelievo> Items { get; set; }

        public Command LoadItemsCommand { get; set; }

        public Command AddCommand { get; set; }

        public Command PrintCommand { get; set; }

        #endregion

        #region Constructor

        public IndexViewModel(INavigation navigation, Page page, Lotto lotto)
            : base(navigation, page)
        {
            this.Title = "Browse";
            this.Items = new ObservableCollection<Prelievo>();
            this.lotto = lotto;

            this.AddCommand = new Command(async () => await ExecuteAddCommand());
            this.LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            this.PrintCommand = new Command(async () => await ExecutePrintCommand());

        }

        #endregion

        #region Methods

        private async Task ExecutePrintCommand()
        {
            Debug.WriteLine("Print Command");

            var printer = DependencyService.Get<IPrinter>();
            printer.MacAddress = "00:03:7A:30:B0:4D";

            var registroRaccolta = new RegistroRaccolta();

            registroRaccolta.Acquirente = new Acquirente() { CAP = "63021", Comune = "AMANDOLA", SiglaProvincia = "AP", Indirizzo = "ZONA IND.LE PIAN DI CONTRO", RagioneSociale = "SIBILLA SOC.COOP.AGR.", Piva = "00100010446" };
            registroRaccolta.Destinatario = new Destinatario() { CAP = "63021", Comune = "AMANDOLA", SiglaProvincia = "AP", Indirizzo = "LOC. PIANDICONTRO", RagioneSociale = "FATTORIE MARCHIGIANE CONS.COOP.", P_IVA = "00433920410" };
            registroRaccolta.Trasportatore = new Trasportatore() { TargaAutomezzo = "CD182ZZ", RagioneSociale = "LATTE MARCHE SOC.COOP.AGR", Indirizzo = "VIA S.TOTTI, 7 - 60100 ANCONA (AN)", P_IVA = "008880425" };
            registroRaccolta.Giro = new Giro() { Denominazione = "PESARO-ANCONA" };
            registroRaccolta.Data = DateTime.Now;
            registroRaccolta.Lotto = new Lotto()
            {
                Codice = "P10302200920",
                Prelievi = new List<Prelievo>()
                {
                    new Prelievo()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DataConsegna = DateTime.Today.AddDays(1),
                        DataPrelievo = DateTime.Today,
                        DataUltimaMungitura = DateTime.Today.AddDays(-1),
                        NumeroMungiture = 1,
                        Quantita_kg = Convert.ToDecimal(5.5),
                        Scomparto = "1",
                        Temperatura = Convert.ToDecimal(26.7),
                        Allevamento = new Allevamento() { RagioneSociale = "TRIONFI HONORATI ANTONIO S.R.L. (testo per farlo lungo)", P_IVA = "00136660420", Prov = "AN" },
                        TipoLatte = new TipoLatte() { DescrizioneBreve = "QM-AQ" }
                    },
                    new Prelievo()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DataConsegna = DateTime.Today.AddDays(1),
                        DataPrelievo = DateTime.Today,
                        DataUltimaMungitura = DateTime.Today.AddDays(-1),
                        NumeroMungiture = 3,
                        Quantita_kg = Convert.ToDecimal(4.4),
                        Scomparto = "2",
                        Temperatura = Convert.ToDecimal(32.3),
                        Allevamento = new Allevamento() { RagioneSociale = "SOCIETA' AGR. MARINELLI FABRIZIO E LUCA", P_IVA = "00398130435", Prov = "MC" },
                        TipoLatte = new TipoLatte() { DescrizioneBreve = "QM-AQ" }
                    }
                }
            };

            try
            {
                await printer.PrintLabel(registroRaccolta);
                await this.page.DisplayAlert("Info", "Stampa effettuata", "OK");
            }
            catch (Exception exc)
            {
                await this.page.DisplayAlert("Error", exc.Message, "OK");
            }

        }

        private async Task ExecuteLoadItemsCommand()
        {
            if (this.IsBusy)
                return;

            this.IsBusy = true;

            try
            {
                this.Items.Clear();

                var lotto = await this.lottiService.GetItemAsync(this.lotto.Id);
                foreach (var item in lotto.Prelievi)
                {
                    this.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private async Task ExecuteAddCommand()
        {
            await this.navigation.PushAsync(new EditPage(new EditViewModel(this.navigation, this.page, this.lotto.Id, "")));
        }

        #endregion

    }
}
