using AutoMapper;
using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Enums;
using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Rest.Dtos;
using LatteMarche.Xamarin.Rest.Interfaces;
using LatteMarche.Xamarin.Views.Giri;
using LatteMarche.Xamarin.Zebra.Interfaces;
using LatteMarche.Xamarin.Zebra.Models;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Sentry;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.Models;
using XF.Material.Forms.UI.Dialogs;

namespace LatteMarche.Xamarin.ViewModels.Giri
{
    public class IndexViewModel : BaseViewModel
    {
        #region Fields

        private IDevice device = DependencyService.Get<IDevice>();
        private IAcquirentiService acquirentiService => DependencyService.Get<IAcquirentiService>();
        private IAllevamentiService allevamentiService => DependencyService.Get<IAllevamentiService>();
        private IAutoCisterneService autocisterneService => DependencyService.Get<IAutoCisterneService>();
        private IDestinatariService destinatariService => DependencyService.Get<IDestinatariService>();
        private ICessionariService cessionariService => DependencyService.Get<ICessionariService>();
        private ITemplateGiroService templateService => DependencyService.Get<ITemplateGiroService>();
        private IGiriService giriService => DependencyService.Get<IGiriService>();
        private IPrelieviService prelieviService => DependencyService.Get<IPrelieviService>();
        private ITemplateGiroService templateGiroService => DependencyService.Get<ITemplateGiroService>();
        private IRestService restService => DependencyService.Get<IRestService>();

        private ISincronizzazioneService sincronizzazioneService = DependencyService.Get<ISincronizzazioneService>();

        private ITrasportatoriService trasportatoriService => DependencyService.Get<ITrasportatoriService>();

        private List<TemplateGiro> templateList;

        #endregion

        #region Properties

        public ObservableCollection<ItemViewModel> Items { get; set; }

        public Command AddCommand { get; set; }

        public Command LoadItemsCommand { get; set; }

        #endregion

        #region Constructor

        public IndexViewModel(INavigation navigation, Page page)
            : base(navigation, page)
        {
            this.Items = new ObservableCollection<ItemViewModel>();
            this.NoData = false;
            this.AddCommand = new Command(async () => await ExecuteAddCommand());
            this.LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Apertura pagina nuovo lotto
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteAddCommand()
        {
            
            // selezione tipo giro
            var result = await MaterialDialog.Instance.SelectChoiceAsync(title: "Seleziona giro", dismissiveText: "Annulla", choices: templateList.Select(t => t.Descrizione).ToArray());

            if (result < 0)
                return;

            // check lavaggio cisterna
            var resultLavaggioCisterna = await MaterialDialog.Instance.ConfirmAsync(message: "E' stato effettuato il lavaggio della cisterna?", confirmingText: "Si", dismissiveText: "No");


            var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Salvataggio in corso", lottieAnimation: "LottieLogo1.json");

            try
            {
                await Task.Run(() =>
                {
                    var giro = new Giro()
                    {
                        IdTemplateGiro = this.templateList[result].Id,
                        DataCreazione = DateTime.Now,
                        Titolo = this.templateList[result].Descrizione,
                        LavaggioCisterna = resultLavaggioCisterna.HasValue ? resultLavaggioCisterna.Value : false
                    };

                    this.giriService.AddItemAsync(giro).Wait();

                    var item = Mapper.Map<ItemViewModel>(giro);

                    item.OnItem_Closing += Item_OnItem_Closing;
                    item.OnItem_Transfering += Item_OnItem_Transfering;
                    item.OnItem_Opening += Item_OnItem_Opening;
                    item.OnItem_Printing += Item_OnItem_Printing;
                    item.OnItem_Deleting += Item_OnItem_Deleting;

                    this.Items.Add(item);

                    // archiviazione eventuale giro (chiuso) precedente => scompare dalla lista
                    this.giriService.ArchiviaGiroPrecedenteAsync(giro.IdTemplateGiro.Value).Wait();

                    Analytics.TrackEvent("Nuovo giro", new Dictionary<string, string>() {
                        { "giro", JsonConvert.SerializeObject(giro) },
                        { "device", DependencyService.Get<IDevice>().GetIdentifier() }
                    });
                    SentrySdk.CaptureMessage("Nuovo giro", Sentry.Protocol.SentryLevel.Info);

                });

                await loadingDialog.DismissAsync();
                await ExecuteLoadItemsCommand();
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
        /// Stampa ricevuta consegna
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Item_OnItem_Printing(object sender, EventArgs e)
        {

            try
            {
                var item = sender as ItemViewModel;

                var giro = this.giriService.GetItemAsync(item.Id).Result;
                giro.Prelievi = this.prelieviService.GetByGiro(item.Id).Result.ToList();
                var templateGiro = GetTemplateGiro(item.IdTemplateGiro).Result;

                var registroRaccolta = new RegistroRaccolta();

                registroRaccolta.Acquirente = GetAcquirente(giro).Result;
                registroRaccolta.Cessionario = GetCessionario(giro).Result;
                registroRaccolta.Destinatario = GetDestinatario(giro).Result;
                registroRaccolta.Giro = templateGiro;
                registroRaccolta.Trasportatore = this.trasportatoriService.GetCurrent().Result;
                registroRaccolta.CodiceLotto = giro.CodiceLotto;
                registroRaccolta.Data = DateTime.Now;
                registroRaccolta.LavaggioCisterna = giro.LavaggioCisterna;

                foreach (var prelievo in giro.Prelievi)
                {
                    var allevamento = GetAllevamento(prelievo.IdAllevamento).Result;

                    registroRaccolta.Items.Add(new RegistroRaccolta.Item()
                    {
                        Scomparto = prelievo.Scomparto,
                        Allevamento = allevamento,
                        DataPrelievo = prelievo.DataPrelievo,
                        Quantita_kg = prelievo.Quantita_kg,
                        TipoLatte = allevamento != null ? allevamento.TipoLatte : null,
                        Trasbordo = prelievo.Trasbordo
                    });
                }

                var previewPage = new PrintPreviewPage(registroRaccolta);
                await this.navigation.PushAsync(previewPage);

            }
            catch (Exception exc)
            {
                SentrySdk.CaptureException(exc);
                Crashes.TrackError(exc);

                await this.page.DisplayAlert("Errore", "Si è verificato un errore imprevisto. Contattare l'amministratore", "OK");
            }
        }

        /// <summary>
        /// Caricamento elenco lotti
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteLoadItemsCommand()
        {
            if (this.IsBusy)
                return;

            this.IsBusy = true;

            var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Caricamento giri", lottieAnimation: "LottieLogo1.json");
            this.NoData = false;

            try
            {
                await Task.Run(() =>
                {
                    this.Items.Clear();
                    var giri = this.giriService.GetGiriNonArchiviatiAsync().Result;
                    foreach (var giro in giri)
                    {
                        var item = Mapper.Map<ItemViewModel>(giro);

                        item.OnItem_Closing += Item_OnItem_Closing;
                        item.OnItem_Transfering += Item_OnItem_Transfering;
                        item.OnItem_Opening += Item_OnItem_Opening;
                        item.OnItem_Printing += Item_OnItem_Printing;
                        item.OnItem_Deleting += Item_OnItem_Deleting;

                        this.Items.Add(item);
                    }

                    this.templateList = GetTemplateList();
                    this.NoData = this.Items.Count == 0;
                });
            }
            catch (Exception exc)
            {
                SentrySdk.CaptureException(exc);
                Crashes.TrackError(exc);
            }
            finally
            {
                await loadingDialog.DismissAsync();
                this.IsBusy = false;
            }
        }

        /// <summary>
        /// Caricamento elenco definizioni giro
        /// </summary>
        /// <returns></returns>
        private List<TemplateGiro> GetTemplateList()
        {
            var giriAperti = this.giriService.GetGiriApertiAsync().Result;
            var templateAperti = giriAperti.Select(g => g.IdTemplateGiro).Distinct().ToList();
            var trasportatore = this.trasportatoriService.GetCurrent().Result;

            return this.templateService.GetItemsAsync(trasportatore.Id)
                .Result
                .Where(t => !templateAperti.Contains(t.Id))
                .OrderBy(t => t.Descrizione)
                .ToList();
        }

        /// <summary>
        /// Caricamento trasbordo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Item_OnItem_Transfering(object sender, EventArgs e)
        {
            var targhe = this.autocisterneService.GetItemsAsync().Result.Where(a => !a.Selezionata).Select(a => a.Targa).ToArray();
            var index = await MaterialDialog.Instance.SelectChoiceAsync(title: "Autocisterna di destinazione", choices: targhe);

            if (index == -1)
                return;

            var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Trasbordo giro", lottieAnimation: "LottieLogo1.json");
            try
            {
                VersionTracking.Track();

                await Task.Run(() =>
                {
                    var location = GeolocationService.GetLocation();
                    TrasbordaGiro(sender as ItemViewModel, location, targhe[index]);
                });

                Analytics.TrackEvent("Giro trasbordato", new Dictionary<string, string>());
                SentrySdk.CaptureMessage("Giro trasbordato", Sentry.Protocol.SentryLevel.Info);

                await loadingDialog.DismissAsync();
                await this.page.DisplayAlert("Info", "Giro trasbordato", "OK");
                await ExecuteLoadItemsCommand();
            }
            catch (Exception exc)
            {
                await loadingDialog.DismissAsync();

                SentrySdk.CaptureException(exc);
                Crashes.TrackError(exc);
            }
        }

        /// <summary>
        /// Evento chiusura giro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Item_OnItem_Closing(object sender, EventArgs e)
        {
            var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Chiusura giro", lottieAnimation: "LottieLogo1.json");

            try
            {
                VersionTracking.Track();

                await Task.Run(() =>
                {
                    var location = GeolocationService.GetLocation();

                    ChiudiGiro(sender).Wait();
                    InviaGiro(sender, location);
                });

                Analytics.TrackEvent("Giro chiuso", new Dictionary<string, string>());
                SentrySdk.CaptureMessage("Giro chiuso", Sentry.Protocol.SentryLevel.Info);

                await loadingDialog.DismissAsync();
                await this.page.DisplayAlert("Info", "Giro chiuso", "OK");
                await ExecuteLoadItemsCommand();
            }
            catch (Exception exc)
            {
                await loadingDialog.DismissAsync();

                SentrySdk.CaptureException(exc);
                Crashes.TrackError(exc);
            }

        }

        /// <summary>
        /// Trasbordo giro
        /// </summary>
        /// <param name="item"></param>
        /// <param name="location"></param>
        private void TrasbordaGiro(ItemViewModel item, Location location, string targaDestinazione)
        {
            var giro = this.giriService.GetItemAsync(item.Id).Result;

            var prelievi = this.prelieviService.GetByGiro(item.Id).Result;

            foreach (var prelievo in prelievi)
                prelievo.Giro = giro;

            var prelieviDto = Mapper.Map<List<PrelievoLatteDto>>(prelievi);

            var trasportatore = this.trasportatoriService.GetCurrent().Result;

            foreach (var prelievoDto in prelieviDto)
                prelievoDto.IdTrasportatore = trasportatore.Id;

            var autocisterna = this.autocisterneService.GetDefaultAsync().Result;

            var trasbordoDto = new TrasbordoDto()
            {
                IMEI_Origine = this.device.GetIdentifier(),
                Targa_Origine = autocisterna.Targa,
                Targa_Destinazione = targaDestinazione,
                Data = DateTime.Now,
                IdTemplateGiro = giro.IdTemplateGiro.Value,
                Lat = location != null ? Convert.ToDecimal(location.Latitude) : (decimal?)null,
                Lng = location != null ? Convert.ToDecimal(location.Longitude) : (decimal?)null,
                Prelievi = prelieviDto
            };

            // chiamata REST upload dati
            if (this.restService.UploadTrasbordo(trasbordoDto).Result != null)
            {
                giro.DataConsegna = DateTime.Now;
                giro.DataUpload = DateTime.Now;
                this.giriService.UpdateItemAsync(giro).Wait();
            }
        }

        /// <summary>
        /// Chiusura giro
        /// </summary>
        /// <param name="sender"></param>
        private async Task ChiudiGiro(object sender)
        {
            var item = sender as ItemViewModel;
            var templateGiro = await GetTemplateGiro(item.IdTemplateGiro);

            var giro = await this.giriService.GetItemAsync(item.Id);
            giro.DataConsegna = DateTime.Now;
            giro.CodiceLotto = $"{templateGiro.Codice}{giro.DataConsegna:ddMMyyHHmm}";
            this.giriService.UpdateItemAsync(giro).Wait();

            var prelievi = this.prelieviService.GetByGiro(giro.Id).Result;
            foreach (var prelievo in prelievi)
            {
                prelievo.DataConsegna = giro.DataConsegna;
                this.prelieviService.UpdateItemAsync(prelievo).Wait();
            }
        }

        /// <summary>
        /// Invio giro al portale
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="location"></param>
        private void InviaGiro(object sender, Location location)
        {
            var item = sender as ItemViewModel;

            var giro = this.giriService.GetItemAsync(item.Id).Result;

            var prelievi = this.prelieviService.GetByGiro(item.Id).Result;

            foreach (var prelievo in prelievi)
                prelievo.Giro = giro;

            var prelieviDto = Mapper.Map<List<PrelievoLatteDto>>(prelievi);

            var trasportatore = this.trasportatoriService.GetCurrent().Result;

            foreach (var prelievoDto in prelieviDto)
                prelievoDto.IdTrasportatore = trasportatore.Id;

            var uploadDto = new UploadDto()
            {
                IMEI = this.device.GetIdentifier(),
                Lat = location != null ? Convert.ToDecimal(location.Latitude) : (decimal?)null,
                Lng = location != null ? Convert.ToDecimal(location.Longitude) : (decimal?)null,
                VersioneApp = VersionTracking.CurrentVersion,
                VersioneOS = DeviceInfo.VersionString,
                Marca = DeviceInfo.Manufacturer,
                Modello = DeviceInfo.Model,
                Nome = DeviceInfo.Name,
                Prelievi = prelieviDto
            };

            // chiamata REST upload dati
            if (this.restService.UploadPrelievi(uploadDto).Result)
            {
                giro.DataUpload = DateTime.Now;
                this.giriService.UpdateItemAsync(giro).Wait();

                // aggiornamento tabella sincronizzazioni
                this.sincronizzazioneService.AddAsync(SynchType.Upload).Wait();
            }
            else
            {
                throw new Exception("Errore di sincronizzazione con il server");
            }

            // aggiornamento database
            var dto = restService.DownloadDb(device.GetIdentifier()).Result;
            sincronizzazioneService.UpdateDatabaseSync(dto).Wait();
        }

        /// <summary>
        /// Evento ri-apertura giro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Item_OnItem_Opening(object sender, EventArgs e)
        {
            var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Riapertura giro", lottieAnimation: "LottieLogo1.json");

            try
            {
                await Task.Run(() =>
                {
                    var item = sender as ItemViewModel;

                    // Salvataggio codice lotto
                    var giro = this.giriService.GetItemAsync(item.Id).Result;
                    giro.DataConsegna = (DateTime?)null;
                    giro.CodiceLotto = String.Empty;
                    this.giriService.UpdateItemAsync(giro).Wait();
                });

                await loadingDialog.DismissAsync();
                await this.page.DisplayAlert("Info", "Giro riaperto", "OK");
                await ExecuteLoadItemsCommand();
            }
            catch (Exception exc)
            {
                await loadingDialog.DismissAsync();

                SentrySdk.CaptureException(exc);
                Crashes.TrackError(exc);
            }

        }

        /// <summary>
        /// Rimozione giro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Item_OnItem_Deleting(object sender, EventArgs e)
        {
            IMaterialModalPage loadingDialog = null;
            try
            {
                bool reply = await this.page.DisplayAlert("Attenzione", $"Sei sicuro di voler eliminare il giro selezionato?", "Si", "No");
                if (reply == false)
                    return;

                loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Rimozione giro", lottieAnimation: "LottieLogo1.json");
                await Task.Run(() =>
                {                   
                    var item = sender as ItemViewModel;
                    this.giriService.DeleteItemAsync(item.Id).Wait();
                    this.Items.Remove(item);

                    this.templateList = GetTemplateList();
                });

                await loadingDialog.DismissAsync();
            }
            catch (Exception exc)
            {
                await loadingDialog.DismissAsync();

                SentrySdk.CaptureException(exc);
                Crashes.TrackError(exc);

                await this.page.DisplayAlert("Errore", exc.Message, "OK");
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
        /// Lookup acquirente
        /// </summary>
        /// <param name="lotto"></param>
        /// <returns></returns>
        private async Task<Acquirente> GetAcquirente(Giro giro)
        {
            if (giro.Prelievi.Count(p => p.IdAcquirente.HasValue) > 0)
            {
                var idAcquirente = giro.Prelievi.First(p => p.IdAcquirente.HasValue).IdAcquirente.Value;
                return await this.acquirentiService.GetItemAsync(idAcquirente);
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
        private async Task<Cessionario> GetCessionario(Giro giro)
        {
            if (giro.Prelievi.Count(p => p.IdCessionario.HasValue) > 0)
            {
                var idCessionario = giro.Prelievi.First(p => p.IdCessionario.HasValue).IdCessionario.Value;
                return await this.cessionariService.GetItemAsync(idCessionario);
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
        private async Task<Destinatario> GetDestinatario(Giro giro)
        {
            if (giro.Prelievi.Count(p => p.IdDestinatario.HasValue) > 0)
            {
                var idDestinatario = giro.Prelievi.First(p => p.IdDestinatario.HasValue).IdDestinatario.Value;
                return await this.destinatariService.GetItemAsync(idDestinatario);
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
