using AutoMapper;
using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Enums;
using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Rest.Dtos;
using LatteMarche.Xamarin.Rest.Interfaces;
using LatteMarche.Xamarin.Views.Giri;
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
        private ITemplateGiroService templateService => DependencyService.Get<ITemplateGiroService>();
        private IGiriService giriService => DependencyService.Get<IGiriService>();
        private IPrelieviService prelieviService => DependencyService.Get<IPrelieviService>();
        private ITemplateGiroService templateGiroService => DependencyService.Get<ITemplateGiroService>();
        private IRestService restService => DependencyService.Get<IRestService>();

        private ISincronizzazioneService sincronizzazioneService = DependencyService.Get<ISincronizzazioneService>();

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
            var result = await MaterialDialog.Instance.SelectChoiceAsync(title: "Seleziona giro", dismissiveText: "Annulla", choices: templateList.Select(t => t.Descrizione).ToArray());

            if (result < 0)
                return;

            var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Salvataggio in corso", lottieAnimation: "LottieLogo1.json");

            try
            {
                await Task.Run(() =>
                {
                    var giro = new Giro()
                    {
                        IdTemplateGiro = this.templateList[result].Id,
                        DataCreazione = DateTime.Now,
                        Titolo = this.templateList[result].Descrizione
                    };

                    this.giriService.AddItemAsync(giro).Wait();

                    var item = Mapper.Map<ItemViewModel>(giro);

                    item.OnItem_Closing += Item_OnItem_Closing;
                    item.OnItem_Opening += Item_OnItem_Opening;
                    item.OnItem_Printing += Item_OnItem_Printing;
                    item.OnItem_Sending += Item_OnItem_Sending;
                    item.OnItem_Deleting += Item_OnItem_Deleting;

                    this.Items.Add(item);

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

        private void Item_OnItem_Printing(object sender, EventArgs e)
        {
            
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
                    var giri = this.giriService.GetGiriApertiAsync().Result;
                    foreach (var giro in giri)
                    {
                        var item = Mapper.Map<ItemViewModel>(giro);

                        item.OnItem_Closing += Item_OnItem_Closing;
                        item.OnItem_Opening += Item_OnItem_Opening;
                        item.OnItem_Printing += Item_OnItem_Printing;
                        item.OnItem_Sending += Item_OnItem_Sending;
                        item.OnItem_Deleting += Item_OnItem_Deleting;

                        this.Items.Add(item);
                    }

                    this.templateList = this.templateService.GetItemsAsync().Result.ToList();
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
        /// Evento chiusura giro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Item_OnItem_Closing(object sender, EventArgs e)
        {
            var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Chiusura giro", lottieAnimation: "LottieLogo1.json");

            try
            {
                await Task.Run(() =>
                {
                    var item = sender as ItemViewModel;
                    var templateGiro = GetTemplateGiro(item.IdTemplateGiro).Result;

                    // Salvataggio codice lotto
                    var giro = this.giriService.GetItemAsync(item.Id).Result;
                    giro.DataConsegna = DateTime.Now;
                    giro.CodiceLotto = $"{templateGiro?.Codice}{giro.DataConsegna:ddMMyyHHmm}";
                    this.giriService.UpdateItemAsync(giro).Wait();

                    var prelievi = this.prelieviService.GetByGiro(giro.Id).Result;
                    foreach (var prelievo in prelievi)
                    {
                        prelievo.DataConsegna = giro.DataConsegna;
                        this.prelieviService.UpdateItemAsync(prelievo).Wait();
                    }
                        

                    
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
                    var templateGiro = GetTemplateGiro(item.IdTemplateGiro).Result;

                    // Salvataggio codice lotto
                    var giro = this.giriService.GetItemAsync(item.Id).Result;
                    giro.DataConsegna = (DateTime?)null;
                    giro.CodiceLotto = $"{templateGiro?.Codice}{giro.DataConsegna:ddMMyyHHmm}";
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
            var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Rimozione giro", lottieAnimation: "LottieLogo1.json");
            try
            {
                bool reply = await this.page.DisplayAlert("Attenzione", $"Sei sicuro di voler eliminare il giro selezionato?", "Si", "No");
                if (reply == false)
                    return;

                await Task.Run(() =>
                {                   
                    var item = sender as ItemViewModel;
                    this.giriService.DeleteItemAsync(item.Id).Wait();
                    this.Items.Remove(item);
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

        private Location GetLocation()
        {
            var permissionStatus = Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>().Result;
            return permissionStatus == PermissionStatus.Granted ? Geolocation.GetLastKnownLocationAsync().Result : null;
        }

        /// <summary>
        /// Invio giro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Item_OnItem_Sending(object sender, EventArgs e)
        {

            var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Invio giro", lottieAnimation: "LottieLogo1.json");

            try
            {
                var location = GetLocation();
                VersionTracking.Track();

                await Task.Run(() =>
                {
                    var item = sender as ItemViewModel;
                    var prelievi = this.prelieviService.GetByGiro(item.Id).Result;

                    var prelieviDto = Mapper.Map<List<PrelievoLatteDto>>(prelievi);

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

                    var json = JsonConvert.SerializeObject(uploadDto);

                    // chiamata REST upload dati
                    if (this.restService.Upload(uploadDto).Result)
                    {
                        var giro = this.giriService.GetItemAsync(item.Id).Result;
                        giro.DataUpload = DateTime.Now;
                        this.giriService.UpdateItemAsync(giro).Wait();

                        // aggiornamento tabella sincronizzazioni
                        this.sincronizzazioneService.AddAsync(SynchType.Upload).Wait();
                    }
                    else
                    {
                        throw new Exception("Errore di sincronizzazione con il server");
                    }

                });

                await loadingDialog.DismissAsync();

                Analytics.TrackEvent("Invio lotto");
                SentrySdk.CaptureMessage("Invio lotto", Sentry.Protocol.SentryLevel.Info);

                await this.page.DisplayAlert("Info", "Invio effettuato con successo", "OK");
                await ExecuteLoadItemsCommand();
            }
            catch (Exception exc)
            {
                await loadingDialog.DismissAsync();

                SentrySdk.CaptureException(exc);
                Crashes.TrackError(exc);

                await this.page.DisplayAlert("Errore", "Si è verificato un errore imprevisto. Contattare l'amministratore", "OK");
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

        #endregion

    }
}
