using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Models;
using LatteMarche.Xamarin.Services;
using LatteMarche.Xamarin.Zebra;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        private Prelievo prelievo;

        private bool isNew = false;
        private string idLotto;

        private string id;

        private int? idAllevamento;
        private GiroItem allevamentoSelezionato;
        private DateTime dataPrelievo;
        private TimeSpan oraPrelievo;
        private string scomparto;
        private decimal? kg;
        private decimal? lt;
        private decimal? temperatura;
        private int? numeroMungiture;
        private DateTime dataUltimaMungitura;
        private TimeSpan oraUltimaMungitura;
        private int? idAcquirente;
        private Acquirente acquirenteSelezionato;
        private int? idDestinatario;
        private Destinatario destinatarioSelezionato;

        private ObservableCollection<GiroItem> allevamenti;
        private ObservableCollection<Acquirente> acquirenti;
        private ObservableCollection<Destinatario> destinatari;

        private IAcquirentiService acquirentiService => DependencyService.Get<IAcquirentiService>();
        private IAllevamentiService allevamentiService => DependencyService.Get<IAllevamentiService>();
        private IDestinatariService destinatariService => DependencyService.Get<IDestinatariService>();
        private IGiriService giriService => DependencyService.Get<IGiriService>();
        private IGiroItemsService giroItemsService => DependencyService.Get<IGiroItemsService>();
        private ILottiService lottiService => DependencyService.Get<ILottiService>();
        private IPrelieviService prelieviService => DependencyService.Get<IPrelieviService>();
        private ITrasportatoriService trasportatoriService => DependencyService.Get<ITrasportatoriService>();

        #endregion

        #region Properties

        public string Id
        {
            get { return this.id; }
            set { SetProperty(ref this.id, value); }
        }

        public ObservableCollection<GiroItem> Allevamenti
        {
            get { return this.allevamenti; }
            set { SetProperty<ObservableCollection<GiroItem>>(ref this.allevamenti, value); }
        }

        public int? IdAllevamento
        {
            get { return this.idAllevamento; }
            set { SetProperty(ref this.idAllevamento, value); }
        }

        public GiroItem AllevamentoSelezionato
        {
            get { return this.allevamentoSelezionato; }
            set { SetProperty<GiroItem>(ref this.allevamentoSelezionato, value); }
        }

        public DateTime DataPrelievo
        {
            get { return GetDateWhitoutTime(this.dataPrelievo); }
            set { SetProperty<DateTime>(ref this.dataPrelievo, value); }
        }

        public TimeSpan OraPrelievo
        {
            get { return this.oraPrelievo; }
            set { SetProperty<TimeSpan>(ref this.oraPrelievo, value); }
        }

        public string Scomparto
        {
            get { return this.scomparto; }
            set { SetProperty(ref this.scomparto, value); }
        }

        public decimal? Kg
        {
            get { return this.kg; }
            set { SetProperty(ref this.kg, value); }
        }

        public decimal? Lt
        {
            get { return this.lt; }
            set { SetProperty(ref this.lt, value); }
        }

        public decimal? Temperatura
        {
            get { return this.temperatura; }
            set { SetProperty(ref this.temperatura, value); }
        }

        public int? NumeroMungiture
        {
            get { return this.numeroMungiture; }
            set { SetProperty(ref this.numeroMungiture, value); }
        }

        public DateTime DataUltimaMungitura
        {
            get { return GetDateWhitoutTime(this.dataUltimaMungitura); }
            set { SetProperty<DateTime>(ref this.dataUltimaMungitura, value); }
        }

        public TimeSpan OraUltimaMungitura
        {
            get { return this.oraUltimaMungitura; }
            set { SetProperty<TimeSpan>(ref this.oraUltimaMungitura, value); }
        }

        public ObservableCollection<Acquirente> Acquirenti
        {
            get { return this.acquirenti; }
            set { SetProperty<ObservableCollection<Acquirente>>(ref this.acquirenti, value); }
        }

        public int? IdAcquirente
        {
            get { return this.idAcquirente; }
            set { SetProperty(ref this.idAcquirente, value); }
        }

        public Acquirente AcquirenteSelezionato
        {
            get { return this.acquirenteSelezionato; }
            set { SetProperty<Acquirente>(ref this.acquirenteSelezionato, value); }
        }

        public ObservableCollection<Destinatario> Destinatari
        {
            get { return this.destinatari; }
            set { SetProperty<ObservableCollection<Destinatario>>(ref this.destinatari, value); }
        }

        public int? IdDestinatario
        {
            get { return this.idDestinatario; }
            set { SetProperty(ref this.idDestinatario, value); }
        }

        public Destinatario DestinatarioSelezionato
        {
            get { return this.destinatarioSelezionato; }
            set { SetProperty<Destinatario>(ref this.destinatarioSelezionato, value); }
        }

        public Command LoadCommand { get; set; }

        public Command PrintCommand { get; set; }

        public Command DeleteItemCommand { get; set; }

        public Command SaveItemCommand { get; set; }

        #endregion

        #region Constructor

        public EditViewModel(INavigation navigation, Page page, string idLotto, string idPrelievo)
            : base(navigation, page)
        {

            this.navigation = navigation;
            this.page = page;

            this.isNew = String.IsNullOrEmpty(idPrelievo);

            this.idLotto = idLotto;
            this.Id = isNew ? Guid.NewGuid().ToString() : idPrelievo;
            this.Title = "";

            this.IsBusy = true;

            this.LoadCommand = new Command(async () => await ExecuteLoadCommand());
            this.PrintCommand = new Command(async () => await ExecutePrintCommand()); 
            this.SaveItemCommand = new Command(async () => await ExecuteSaveItemCommand()); 
            this.DeleteItemCommand = new Command(async () => await ExecuteDeleteItemCommand()); 

        }

        #endregion

        #region Methods

        /// <summary>
        /// Comando caricamento dati singolo prelievo e tabelle lookup
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteLoadCommand()
        {
            try
            {
                this.IsBusy = true;

                await Task.Run(() =>
                {
                    if (isNew)
                    {
                        this.prelievo = new Prelievo()
                        {
                            Id = this.Id,
                            IdLotto = this.idLotto,
                            DataPrelievo = DateTime.Now
                        };
                    }
                    else
                    {
                        this.prelievo = this.prelieviService.GetItemAsync(this.Id).Result;
                    }

                    var lotto = this.lottiService.GetItemAsync(this.prelievo.IdLotto).Result;
                    var acquirenti = this.acquirentiService.GetItemsAsync().Result;
                    var destinatari = this.destinatariService.GetItemsAsync().Result;
                    var allevamenti = this.giroItemsService.GetItems(lotto.IdGiro).Result;

                    // Data prelievo
                    this.DataPrelievo = GetDateWhitoutTime(this.prelievo.DataPrelievo);

                    // Ora prelievo
                    this.OraPrelievo = this.prelievo.DataPrelievo.HasValue ? this.prelievo.DataPrelievo.Value - this.DataPrelievo : new TimeSpan();

                    // Allevamento
                    this.Allevamenti = new ObservableCollection<GiroItem>(allevamenti);
                    this.AllevamentoSelezionato = this.prelievo.IdAllevamento.HasValue ? this.Allevamenti.FirstOrDefault(a => a.IdAllevamento == this.prelievo.IdAllevamento.Value) : null;

                    this.Title = this.AllevamentoSelezionato != null ? this.AllevamentoSelezionato.RagioneSociale : "";

                    // Scomparto
                    this.Scomparto = this.prelievo.Scomparto;

                    // Kg
                    this.Kg = this.prelievo.Quantita_kg;

                    // Lt
                    this.Lt = this.prelievo.Quantita_lt;

                    // temperatura
                    this.Temperatura = this.prelievo.Temperatura;

                    // num mungiture
                    this.NumeroMungiture = this.prelievo.NumeroMungiture;

                    // data ultima mungitura
                    this.DataUltimaMungitura = GetDateWhitoutTime(this.prelievo.DataUltimaMungitura);

                    // Ora ultima mungitura
                    this.OraUltimaMungitura = this.prelievo.DataUltimaMungitura.HasValue ? this.prelievo.DataUltimaMungitura.Value - this.DataPrelievo : new TimeSpan();

                    // acquirente
                    this.Acquirenti = new ObservableCollection<Acquirente>(acquirenti);
                    this.AcquirenteSelezionato = this.prelievo.IdAcquirente.HasValue ? this.Acquirenti.FirstOrDefault(a => a.Id == this.prelievo.IdAcquirente.Value) : null;

                    // destinatario
                    this.Destinatari = new ObservableCollection<Destinatario>(destinatari);
                    this.DestinatarioSelezionato = this.prelievo.IdDestinatario.HasValue ? this.Destinatari.FirstOrDefault(a => a.Id == this.prelievo.IdDestinatario.Value) : null;

                });

                this.IsBusy = false;
            }
            catch (Exception exc)
            {
                this.IsBusy = false;
                await this.page.DisplayAlert("Error", exc.Message, "OK");
            }

        }

        /// <summary>
        /// Comando stampa ricevuta consegna
        /// </summary>
        /// <returns></returns>
        private async Task ExecutePrintCommand()
        {
            try
            {
                Debug.WriteLine("Print Command");
                this.IsBusy = true;

                await Task.Run(() =>
                {
                    var printer = DependencyService.Get<IPrinter>();
                    printer.MacAddress = "00:03:7A:30:B0:4D";

                    this.prelievo = this.prelieviService.GetItemAsync(this.Id).Result;
                    this.prelievo.DataConsegna = DateTime.Now;
                    this.prelieviService.UpdateItemAsync(this.prelievo);

                    var registroConsegna = new RegistroConsegna();

                    registroConsegna.Acquirente = GetAcquirente(this.prelievo.IdAcquirente).Result;
                    registroConsegna.Destinatario = GetDestinatario(this.prelievo.IdDestinatario).Result;
                    registroConsegna.Giro = GetGiro(this.prelievo.Lotto.IdGiro).Result;
                    registroConsegna.Trasportatore = GetTrasportatore(registroConsegna.Giro?.IdTrasportatore).Result;
                    registroConsegna.Allevamento = GetAllevamento(this.prelievo.IdAllevamento).Result;
                    registroConsegna.Data = this.prelievo.DataConsegna.Value;

                    registroConsegna.DataPrelievo = this.prelievo.DataPrelievo;
                    registroConsegna.Scomparto = this.prelievo.Scomparto;
                    registroConsegna.Quantita_kg = this.prelievo.Quantita_kg;
                    registroConsegna.Quantita_lt = this.prelievo.Quantita_lt;
                    registroConsegna.NumeroMungiture = this.prelievo.NumeroMungiture;
                    registroConsegna.DataUltimaMungitura = this.prelievo.DataUltimaMungitura;
                    registroConsegna.TipoLatte = this.prelievo.TipoLatte;

                    //printer.PrintLabel(registroConsegna);
                });

                this.IsBusy = false;
                await this.page.DisplayAlert("Info", "Stampa effettuata", "OK");
            }
            catch (Exception exc)
            {
                this.IsBusy = false;
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
                this.IsBusy = true;

                await Task.Run(() =>
                {
                    this.prelievo.IdAllevamento = this.AllevamentoSelezionato != null ? this.AllevamentoSelezionato.IdAllevamento : (int?)null;
                    this.prelievo.IdLotto = this.idLotto;

                    this.prelievo.DataPrelievo = this.DataPrelievo.Add(this.OraPrelievo);
                    this.prelievo.Scomparto = this.Scomparto;
                    this.prelievo.Quantita_kg = this.Kg;
                    this.prelievo.Quantita_lt = this.Lt;

                    this.prelievo.Temperatura = this.Temperatura;
                    this.prelievo.NumeroMungiture = this.NumeroMungiture;
                    this.prelievo.DataUltimaMungitura = this.DataUltimaMungitura.Add(this.OraUltimaMungitura);
                    this.prelievo.IdAcquirente = this.AcquirenteSelezionato != null ? this.AcquirenteSelezionato.Id : (int?)null;
                    this.prelievo.IdDestinatario = this.DestinatarioSelezionato != null ? this.DestinatarioSelezionato.Id : (int?)null;

                    if (this.isNew)
                        prelieviService.AddItemAsync(this.prelievo);
                    else
                        prelieviService.UpdateItemAsync(this.prelievo);
                });

                this.IsBusy = false;
                await navigation.PopAsync();
            }
            catch (Exception exc)
            {
                this.IsBusy = false;
                await this.page.DisplayAlert("Error", exc.Message, "OK");
            }
        }

        /// <summary>
        /// Eliminazione prelievo
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteDeleteItemCommand()
        {
            try
            {
                this.IsBusy = true;

                await Task.Run(() =>
                {
                    prelieviService.DeleteItemAsync(this.Id);
                });
                
                this.IsBusy = false;
                await navigation.PopAsync();
            }
            catch(Exception exc)
            {
                this.IsBusy = false;
                await this.page.DisplayAlert("Error", exc.Message, "OK");
            }
        }

        /// <summary>
        /// Estrazione della data senza ora
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private DateTime GetDateWhitoutTime(DateTime? dateTime)
        {
            return dateTime.HasValue ? new DateTime(dateTime.Value.Year, dateTime.Value.Month, dateTime.Value.Day) : DateTime.Today;
        }

        /// <summary>
        /// Lookup acquirente
        /// </summary>
        /// <param name="idAcquirente"></param>
        /// <returns></returns>
        private async Task<Acquirente> GetAcquirente(int? idAcquirente)
        {
            if(idAcquirente.HasValue)
            {
                return await this.acquirentiService.GetItemAsync(idAcquirente.Value);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Lookup destinatario
        /// </summary>
        /// <param name="idDestinatario"></param>
        /// <returns></returns>
        private async Task<Destinatario> GetDestinatario(int? idDestinatario)
        {
            if (idDestinatario.HasValue)
            {
                return await this.destinatariService.GetItemAsync(idDestinatario.Value);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Lookup trasportatore
        /// </summary>
        /// <param name="idTrasportatore"></param>
        /// <returns></returns>
        private async Task<Trasportatore> GetTrasportatore(int? idTrasportatore)
        {
            if (idTrasportatore.HasValue)
            {
                return await this.trasportatoriService.GetItemAsync(idTrasportatore.Value);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Lookup giro
        /// </summary>
        /// <param name="idGiro"></param>
        /// <returns></returns>
        private async Task<Giro> GetGiro(int? idGiro)
        {
            if (idGiro.HasValue)
            {
                return await this.giriService.GetItemAsync(idGiro.Value);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Lookup allevamento
        /// </summary>
        /// <param name="idAllevamento"></param>
        /// <returns></returns>
        private async Task<Allevamento> GetAllevamento(int? idAllevamento)
        {
            if (idAllevamento.HasValue)
            {
                return await this.allevamentiService.GetItemAsync(idAllevamento.Value);
            }
            else
            {
                return null;
            }
        }

        #endregion

    }
}
