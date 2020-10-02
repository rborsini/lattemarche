using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Zebra.Interfaces;
using LatteMarche.Xamarin.Zebra.Models;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Sentry;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI;
using XF.Material.Forms.UI.Dialogs;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;

namespace LatteMarche.Xamarin.ViewModels.Prelievi
{
    public class EditViewModel : BaseViewModel
    {
        #region Fields

        private Prelievo prelievo;

        private bool isNew = false;
        private bool isEditable = false;

        private bool isAllevamentoEditable = false;
        private bool isAcquirenteEditable = false;
        private bool isCessionarioEditable = false;
        private bool isDestinatarioEditable = false;

        private int idGiro;

        private string id;

        private int? idAllevamento;
        private Allevamento allevamentoSelezionato;
        private DateTime dataPrelievo;
        private TimeSpan oraPrelievo;
        private string scomparto;
        private string kg;
        private string lt;
        private string temperatura;
        private int? numeroMungiture;
        private DateTime dataUltimaMungitura;
        private TimeSpan oraUltimaMungitura;
        private int? idAcquirente;
        private Acquirente acquirenteSelezionato;
        private int? idDestinatario;
        private Destinatario destinatarioSelezionato;
        private int? idCessionario;
        private Cessionario cessionarioSelezionato;
        private TipoLatte tipoLatte;

        private ObservableCollection<Allevamento> allevamenti;
        private ObservableCollection<Acquirente> acquirenti;
        private ObservableCollection<Cessionario> cessionari;
        private ObservableCollection<Destinatario> destinatari;

        private IAcquirentiService acquirentiService => DependencyService.Get<IAcquirentiService>();
        private IAllevamentiService allevamentiService => DependencyService.Get<IAllevamentiService>();
        private IAutoCisterneService autocisterneService => DependencyService.Get<IAutoCisterneService>();
        private ICessionariService cessionariService => DependencyService.Get<ICessionariService>();
        private IDestinatariService destinatariService => DependencyService.Get<IDestinatariService>();
        private IGiriService giriService => DependencyService.Get<IGiriService>();
        private IPrelieviService prelieviService => DependencyService.Get<IPrelieviService>();
        private IStampantiService stampantiService => DependencyService.Get<IStampantiService>();
        private ITemplateGiroService templateGiroService => DependencyService.Get<ITemplateGiroService>();
        private ITipiLatteService tipiLatteService => DependencyService.Get<ITipiLatteService>();
        private ITrasportatoriService trasportatoriService => DependencyService.Get<ITrasportatoriService>();

        #endregion

        #region Properties

        public Prelievo Prelievo
        {
            get { return this.prelievo; }
            set { this.prelievo = value; }
        }

        public bool IsEditable
        {
            get { return this.isEditable; }
            set { SetProperty(ref this.isEditable, value); }
        }

        public bool IsAllevamentoEditable
        {
            get { return this.isAllevamentoEditable; }
            set { SetProperty(ref this.isAllevamentoEditable, value); }
        }

        public bool IsAcquirenteEditable
        {
            get { return this.isAcquirenteEditable; }
            set { SetProperty(ref this.isAcquirenteEditable, value); }
        }

        public bool IsCessionarioEditable
        {
            get { return this.isCessionarioEditable; }
            set { SetProperty(ref this.isCessionarioEditable, value); }
        }

        public bool IsDestinatarioEditable
        {
            get { return this.isDestinatarioEditable; }
            set { SetProperty(ref this.isDestinatarioEditable, value); }
        }

        public string Id
        {
            get { return this.id; }
            set { SetProperty(ref this.id, value); }
        }

        public ObservableCollection<Allevamento> Allevamenti
        {
            get { return this.allevamenti; }
            set { SetProperty<ObservableCollection<Allevamento>>(ref this.allevamenti, value); }
        }

        public int? IdAllevamento
        {
            get { return this.idAllevamento; }
            set { SetProperty(ref this.idAllevamento, value); }
        }

        public Allevamento AllevamentoSelezionato
        {
            get { return this.allevamentoSelezionato; }
            set { SetProperty<Allevamento>(ref this.allevamentoSelezionato, value); }
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

        public IList<string> ElencoScomparti => new List<string>
        {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6"
        };

        public string Kg
        {
            get { return this.kg; }
            set { SetProperty(ref this.kg, value); }
        }

        public string Lt
        {
            get { return this.lt; }
            set { SetProperty(ref this.lt, value); }
        }

        public string Temperatura
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

        public ObservableCollection<Cessionario> Cessionari
        {
            get { return this.cessionari; }
            set { SetProperty<ObservableCollection<Cessionario>>(ref this.cessionari, value); }
        }

        public int? IdCessionario
        {
            get { return this.idCessionario; }
            set { SetProperty(ref this.idCessionario, value); }
        }

        public Cessionario CessionarioSelezionato
        {
            get { return this.cessionarioSelezionato; }
            set { SetProperty<Cessionario>(ref this.cessionarioSelezionato, value); }
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

        public TipoLatte TipoLatte
        {
            get
            {
                if (this.AllevamentoSelezionato != null &&
                    this.AllevamentoSelezionato.IdTipoLatte.HasValue &&
                    (this.tipoLatte == null || this.tipoLatte.Id != this.AllevamentoSelezionato.IdTipoLatte))
                {
                    this.tipoLatte = this.tipiLatteService.GetItemAsync(this.AllevamentoSelezionato.IdTipoLatte.Value).Result;
                }

                return this.tipoLatte;
            }
        }

        public Command LoadCommand { get; set; }

        public Command PrintCommand { get; set; }

        public Command SaveItemCommand { get; set; }

        #endregion

        #region Constructor

        public EditViewModel(INavigation navigation, Page page, int idGiro, string idPrelievo)
            : base(navigation, page)
        {

            this.navigation = navigation;
            this.page = page;

            this.isNew = String.IsNullOrEmpty(idPrelievo);

            this.idGiro = idGiro;
            this.Id = idPrelievo;

            this.IsBusy = true;

            this.LoadCommand = new Command(async () => await ExecuteLoadCommand());
            this.PrintCommand = new Command(async () => await ExecutePrintCommand(), canExecute: () => { return !String.IsNullOrEmpty(this.Id); });
            this.SaveItemCommand = new Command(async () => await ExecuteSaveItemCommand(), canExecute: () => { return this.IsEditable; });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Comando caricamento dati singolo prelievo e tabelle lookup
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteLoadCommand()
        {
            this.IsBusy = true;

            var permissionStatus = Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>().Result;

            var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Caricamento", lottieAnimation: "LottieLogo1.json");
            try
            {
                await Task.Run(() =>
                {
                    var location = GeolocationService.GetLocation();

                    if (isNew)
                    {
                        this.prelievo = new Prelievo()
                        {
                            Id = Guid.NewGuid().ToString(),
                            IdGiro = this.idGiro,
                            DataPrelievo = DateTime.Now
                        };
                    }
                    else
                    {
                        this.prelievo = this.prelieviService.GetItemAsync(this.Id).Result;
                    }

                    var giro = this.giriService.GetItemAsync(this.idGiro).Result;

                    this.IsEditable = !giro.DataConsegna.HasValue;

                    var acquirenti = this.acquirentiService.GetItemsAsync().Result;
                    var cessionari = this.cessionariService.GetItemsAsync().Result;
                    var destinatari = this.destinatariService.GetItemsAsync().Result;
                    var allevamenti = this.allevamentiService.GetByTemplate(giro.IdTemplateGiro.Value).Result.ToList();

                    // #326117 rimozione degli allevamenti già presenti nel giro
                    var prelievi = this.prelieviService.GetByGiro(giro.Id).Result;
                    var idAllevamentiGiro = prelievi.Select(p => p.IdAllevamento).ToList();
                    allevamenti = allevamenti.Where(a => a.IdAllevamento == this.prelievo.IdAllevamento || !idAllevamentiGiro.Contains(a.IdAllevamento)).ToList();

                    this.IsAllevamentoEditable = this.IsEditable && allevamenti.Count() > 0;
                    this.IsAcquirenteEditable = this.IsEditable && acquirenti.Count() > 0;
                    this.IsCessionarioEditable = this.IsEditable && cessionari.Count() > 0;
                    this.IsDestinatarioEditable = this.IsEditable && destinatari.Count() > 0;

                    // Data prelievo
                    this.DataPrelievo = GetDateWhitoutTime(prelievo.DataPrelievo);

                    // Ora prelievo
                    this.OraPrelievo = this.prelievo.DataPrelievo.HasValue ? this.prelievo.DataPrelievo.Value - this.DataPrelievo : new TimeSpan();

                    // Allevamento
                    this.Allevamenti = new ObservableCollection<Allevamento>(allevamenti);
                    this.AllevamentoSelezionato = GetAllevamentoSelezionato(location);

                    this.Title = giro.Titolo;  // #325923

                    // Scomparto
                    this.Scomparto = this.prelievo.Scomparto;

                    // Kg
                    this.Kg = this.prelievo.Quantita_kg?.ToString();

                    // Lt
                    this.Lt = this.prelievo.Quantita_lt?.ToString();

                    // temperatura
                    this.Temperatura = this.prelievo.Temperatura?.ToString();

                    // num mungiture
                    this.NumeroMungiture = this.prelievo.NumeroMungiture;

                    // data ultima mungitura
                    this.DataUltimaMungitura = GetDateWhitoutTime(prelievo.DataUltimaMungitura);

                    // Ora ultima mungitura
                    this.OraUltimaMungitura = this.prelievo.DataUltimaMungitura.HasValue ? this.prelievo.DataUltimaMungitura.Value - this.DataPrelievo : new TimeSpan();

                    // acquirente
                    this.Acquirenti = new ObservableCollection<Acquirente>(acquirenti);
                    this.AcquirenteSelezionato = GetAcquirenteSelezionato();

                    // cessionario
                    this.Cessionari = new ObservableCollection<Cessionario>(cessionari);
                    this.CessionarioSelezionato = GetCessionarioSelezionato();

                    // destinatario
                    this.Destinatari = new ObservableCollection<Destinatario>(destinatari);
                    this.DestinatarioSelezionato = GetDestinatarioSelezionato();

                });

                (this.PrintCommand as Command).ChangeCanExecute();
                (this.SaveItemCommand as Command).ChangeCanExecute();
                await loadingDialog.DismissAsync();
            }
            catch (Exception exc)
            {
                SentrySdk.CaptureException(exc);
                Crashes.TrackError(exc);
                await loadingDialog.DismissAsync();
                await this.page.DisplayAlert("Errore", exc.Message, "OK");
            }

        }

        /// <summary>
        /// Salvataggio prelievo
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteSaveItemCommand()
        {
            this.IsBusy = true;
            var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Salvataggio in corso", lottieAnimation: "LottieLogo1.json");
            try
            {
                // rilevamento posizione
                var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                var location = GeolocationService.GetLocation();
                if (location == null)
                {
                    await loadingDialog.DismissAsync();
                    await this.page.DisplayAlert("Attenzione", "E' necessario abilitare la geolocalizzazione.", "OK");
                    return;
                }

                // validazione campi obbligatori
                if (!MandatoryFieldsAreFilled())
                {
                    await loadingDialog.DismissAsync();
                    await this.page.DisplayAlert("Attenzione", "E' necessario compilare tutti i valori indicati.", "OK");
                    return;
                }

                // warning campi fuori soglia
                var warningMsg = "";
                if (!AllFieldsInThreshold(out warningMsg))
                {
                    bool reply = await this.page.DisplayAlert("Attenzione", $"Alcuni valori sono fuori soglia.\n\n{warningMsg}\nSei sicuro di voler salvare ugualmente?", "Si", "No");

                    if (reply == false)
                    {
                        await loadingDialog.DismissAsync();
                        return;
                    }
                }

                await Task.Run(() =>
                {
                    this.prelievo.IdAllevamento = this.AllevamentoSelezionato != null ? this.AllevamentoSelezionato.IdAllevamento : (int?)null;
                    this.prelievo.IdGiro = this.idGiro;

                    this.prelievo.DataPrelievo = this.DataPrelievo.Add(this.OraPrelievo);
                    this.prelievo.Scomparto = this.Scomparto;
                    this.prelievo.Quantita_kg = Convert.ToDecimal(this.Kg);
                    this.prelievo.Quantita_lt = Convert.ToDecimal(this.Lt);

                    this.prelievo.Temperatura = Convert.ToDecimal(this.Temperatura);
                    this.prelievo.NumeroMungiture = this.NumeroMungiture;
                    this.prelievo.DataUltimaMungitura = this.DataUltimaMungitura.Add(this.OraUltimaMungitura);
                    this.prelievo.IdAcquirente = this.AcquirenteSelezionato != null ? this.AcquirenteSelezionato.Id : (int?)null;
                    this.prelievo.IdCessionario = this.CessionarioSelezionato != null ? this.CessionarioSelezionato.Id : (int?)null;
                    this.prelievo.IdDestinatario = this.DestinatarioSelezionato != null ? this.DestinatarioSelezionato.Id : (int?)null;

                    this.prelievo.IdAutocisterna = this.autocisterneService.GetDefaultAsync().Result.Id;
                    this.prelievo.Lat = Convert.ToDecimal(location.Latitude);
                    this.prelievo.Lng = Convert.ToDecimal(location.Longitude);

                    this.prelievo.Titolo = this.AllevamentoSelezionato?.RagioneSociale;

                    if (this.isNew)
                        prelieviService.AddItemAsync(this.prelievo).Wait();
                    else
                        prelieviService.UpdateItemAsync(this.prelievo).Wait();

                    this.Id = this.prelievo.Id;
                    this.isNew = false;
                });

                (this.PrintCommand as Command).ChangeCanExecute();

                this.IsBusy = false;
                await loadingDialog.DismissAsync();
                await this.page.DisplayAlert("Info", "Salvataggio avvenuto con successo", "OK");
                //await navigation.PopAsync();
            }
            catch (Exception exc)
            {
                this.IsBusy = false;

                SentrySdk.CaptureException(exc);
                Crashes.TrackError(exc);

                await this.page.DisplayAlert("Errore", "Si è verificato un errore imprevisto. Contattare l'amministratore", "OK");
                await loadingDialog.DismissAsync();
            }

        }

        /// <summary>
        /// Comando stampa ricevuta consegna
        /// </summary>
        /// <returns></returns>
        private async Task ExecutePrintCommand()
        {

            var choices = new string[] { "1", "2", "3", "4", "5" };

            var index = await MaterialDialog.Instance.SelectChoiceAsync(title: "Numero copie", choices: choices);

            if (index == -1)
                return;

            var input = choices[index];


            var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Stampa in corso", lottieAnimation: "LottieLogo1.json");

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

                    this.prelievo = this.prelieviService.GetItemAsync(this.Id).Result;
                    this.prelievo.DataConsegna = DateTime.Now;
                    this.prelieviService.UpdateItemAsync(this.prelievo);

                    var registroConsegna = new RegistroConsegna();

                    registroConsegna.NumeroCopie = Convert.ToInt32(input);

                    var giro = this.giriService.GetItemAsync(this.idGiro).Result;

                    registroConsegna.Acquirente = GetAcquirente(this.prelievo.IdAcquirente).Result;
                    registroConsegna.Cessionario = GetCessionario(this.prelievo.IdCessionario).Result;
                    registroConsegna.Destinatario = GetDestinatario(this.prelievo.IdDestinatario).Result;
                    registroConsegna.Giro = GetTemplateGiro(giro?.IdTemplateGiro).Result;
                    registroConsegna.Trasportatore = this.trasportatoriService.GetCurrent().Result;

                    registroConsegna.Allevamento = GetAllevamento(this.prelievo.IdAllevamento).Result;
                    registroConsegna.Data = this.prelievo.DataConsegna.Value;

                    registroConsegna.DataPrelievo = this.prelievo.DataPrelievo;
                    registroConsegna.Scomparto = this.prelievo.Scomparto;
                    registroConsegna.Quantita_kg = this.prelievo.Quantita_kg;
                    registroConsegna.Quantita_lt = this.prelievo.Quantita_lt;
                    registroConsegna.NumeroMungiture = this.prelievo.NumeroMungiture;
                    registroConsegna.Temperatura = this.prelievo.Temperatura;
                    registroConsegna.DataUltimaMungitura = this.prelievo.DataUltimaMungitura;
                    registroConsegna.TipoLatte = registroConsegna.Allevamento != null ? registroConsegna.Allevamento.TipoLatte : null;

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

        /// <summary>
        /// Verifica compilazione campi obbligatori
        /// </summary>
        /// <returns></returns>
        private bool MandatoryFieldsAreFilled()
        {
            var result = true;

            if (this.AllevamentoSelezionato == null)
                return false;

            if (String.IsNullOrEmpty(this.Scomparto))
                return false;

            if (String.IsNullOrEmpty(this.Kg))
                return false;

            if (String.IsNullOrEmpty(this.Lt))
                return false;

            if (String.IsNullOrEmpty(this.Temperatura))
                return false;

            if (!this.NumeroMungiture.HasValue)
                return false;

            if (this.AcquirenteSelezionato == null)
                return false;

            if (this.DestinatarioSelezionato == null)
                return false;

            return result;
        }

        /// <summary>
        /// Verifica campi fuori soglia
        /// </summary>
        /// <returns></returns>
        private bool AllFieldsInThreshold(out string message)
        {
            var result = true;
            message = "";

            var allevamento = this.AllevamentoSelezionato;
            var kg = Convert.ToDecimal(this.Kg);
            var lt = Convert.ToDecimal(this.Lt);

            // quantità
            if (allevamento.Quantita_Min.HasValue && allevamento.Quantita_Max.HasValue &&
                allevamento.Quantita_Min.Value != 0 && allevamento.Quantita_Max.Value != 0)
            {
                if (kg < allevamento.Quantita_Min)
                {
                    message += $"Qta < {allevamento.Quantita_Min:#.0} kg (5% percentile) \n";
                    result = false;
                }

                if (kg > allevamento.Quantita_Max)
                {
                    message += $"Qta > {allevamento.Quantita_Max:#.0} kg (95% percentile) \n";
                    result = false;
                }
            }

            // temperatura
            if (allevamento.Temperatura_Min.HasValue && allevamento.Temperatura_Max.HasValue &&
                allevamento.Temperatura_Min.Value != 0 && allevamento.Temperatura_Max.Value != 0)
            {
                if (lt < allevamento.Temperatura_Min)
                {
                    message += $"Temp. < {allevamento.Temperatura_Min:#.0} °C (5% percentile) \n";
                    result = false;
                }

                if (lt > allevamento.Temperatura_Max)
                {
                    message += $"Temp. > {allevamento.Temperatura_Max:#.0} °C (95% percentile) \n";
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Evento selezione quantità in kg
        /// </summary>
        /// <param name="kg"></param>
        public void OnKgChanged(string kg)
        {
            this.Kg = kg;
            this.Lt = ConvertToLt(kg);
        }

        /// <summary>
        /// Evento selezione quantità in lt
        /// </summary>
        /// <param name="lt"></param>
        public void OnLtChanged(string lt)
        {
            this.Lt = lt;
            this.Kg = ConvertToKg(lt);
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
        /// <param name="lotto"></param>
        /// <returns></returns>
        private async Task<Acquirente> GetAcquirente(int? idAcquirente)
        {
            if (idAcquirente.HasValue)
            {
                return await this.acquirentiService.GetItemAsync(idAcquirente.Value);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Lookup cessionario
        /// </summary>
        /// <param name="lotto"></param>
        /// <returns></returns>
        private async Task<Cessionario> GetCessionario(int? idCessionario)
        {
            if (idCessionario.HasValue)
            {
                return await this.cessionariService.GetItemAsync(idCessionario.Value);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Lookup destinatario
        /// </summary>
        /// <param name="lotto"></param>
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

        /// <summary>
        /// Lookup template giro
        /// </summary>
        /// <param name="idTemplateGiro"></param>
        /// <returns></returns>
        private async Task<TemplateGiro> GetTemplateGiro(int? idTemplateGiro)
        {
            if (idTemplateGiro.HasValue)
            {
                return await this.templateGiroService.GetItemAsync(idTemplateGiro.Value);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Recupero acquirente selezionato o di default
        /// </summary>
        /// <returns></returns>
        public Acquirente GetAcquirenteSelezionato()
        {
            if (this.prelievo.IdAcquirente.HasValue)
                return this.Acquirenti.FirstOrDefault(a => a.Id == this.prelievo.IdAcquirente.Value);

            if (this.AllevamentoSelezionato != null && this.AllevamentoSelezionato.IdAcquirenteDefault.HasValue)
            {
                return this.Acquirenti.FirstOrDefault(a => a.Id == this.AllevamentoSelezionato.IdAcquirenteDefault.Value);
            }

            return null;
        }

        /// <summary>
        /// Recupero cessionario selezionato o di default
        /// </summary>
        /// <returns></returns>
        public Cessionario GetCessionarioSelezionato()
        {
            if (this.prelievo.IdCessionario.HasValue)
                return this.Cessionari.FirstOrDefault(a => a.Id == this.prelievo.IdCessionario.Value);

            if (this.AllevamentoSelezionato != null && this.AllevamentoSelezionato.IdCessionarioDefault.HasValue)
            {
                return this.Cessionari.FirstOrDefault(a => a.Id == this.AllevamentoSelezionato.IdCessionarioDefault.Value);
            }

            return null;
        }

        /// <summary>
        /// Recupero destinatario selezionato o di default
        /// </summary>
        /// <returns></returns>
        public Destinatario GetDestinatarioSelezionato()
        {
            if (this.prelievo.IdDestinatario.HasValue)
                return this.Destinatari.FirstOrDefault(a => a.Id == this.prelievo.IdDestinatario.Value);

            if (this.AllevamentoSelezionato != null && this.AllevamentoSelezionato.IdDestinatarioDefault.HasValue)
            {
                return this.Destinatari.FirstOrDefault(a => a.Id == this.AllevamentoSelezionato.IdDestinatarioDefault.Value);
            }

            return null;
        }

        /// <summary>
        /// Recupero dell'allevamento selezionato o quello più vicino
        /// </summary>
        /// <returns></returns>
        private Allevamento GetAllevamentoSelezionato(Location location)
        {

            // allevamento presente nel prelievo
            if (this.prelievo.IdAllevamento.HasValue)
                return this.Allevamenti.FirstOrDefault(a => a.IdAllevamento == this.prelievo.IdAllevamento.Value);

            if (this.isNew)
            {
                if (location == null)
                    return null;

                var distanze = this.Allevamenti
                    .Where(a => a.Latitudine.HasValue && a.Longitudine.HasValue)
                    .Select(a => new { Id = a.IdAllevamento, Distance = GetDistance(a, location) })
                    .ToList();

                if (distanze.Count > 0)
                {
                    var idAllevamento = distanze.OrderBy(d => d.Distance).FirstOrDefault().Id;
                    return this.Allevamenti.FirstOrDefault(a => a.IdAllevamento == idAllevamento);
                }
            }

            return null;
        }

        /// <summary>
        /// Conversione quantità da kg a litri
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string ConvertToLt(string value)
        {
            if (!String.IsNullOrEmpty(value) &&
                this.TipoLatte != null &&
                this.TipoLatte.FattoreConversione.HasValue &&
                this.TipoLatte.FattoreConversione.Value != 0)
            {
                var kg = Convert.ToDecimal(value);

                var lt = Convert.ToInt32(kg / this.TipoLatte.FattoreConversione.Value);

                return $"{lt:#}";
            }

            return String.Empty;
        }

        /// <summary>
        /// Conversione quantità da litri a kg
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string ConvertToKg(string value)
        {
            if (!String.IsNullOrEmpty(value) &&
                this.TipoLatte != null &&
                this.TipoLatte.FattoreConversione.HasValue &&
                this.TipoLatte.FattoreConversione.Value != 0)
            {
                var lt = Convert.ToDecimal(value);

                var kg = Convert.ToInt32(lt * this.TipoLatte.FattoreConversione);

                return $"{kg:#}";
            }

            return String.Empty;
        }

        /// <summary>
        /// Calcolo della distanza tra la posizione attuale e l'allevamento
        /// </summary>
        /// <param name="a"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        private double GetDistance(Allevamento a, Location location)
        {
            return Math.Sqrt(Math.Pow(a.Latitudine.Value - location.Latitude, 2) + Math.Pow(a.Longitudine.Value - location.Longitude, 2));
        }

        #endregion

    }
}
