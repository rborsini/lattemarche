using AutoMapper;
using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Enums;
using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Rest.Dtos;
using LatteMarche.Xamarin.Rest.Interfaces;
using LatteMarche.Xamarin.Views.Prelievi;
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
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.Models;
using XF.Material.Forms.UI.Dialogs;

namespace LatteMarche.Xamarin.ViewModels.Prelievi
{
    public class IndexViewModel : BaseViewModel
    {

        #region Fields

        private Giro giro;

        private IAcquirentiService acquirentiService => DependencyService.Get<IAcquirentiService>();
        private IAllevamentiService allevamentiService => DependencyService.Get<IAllevamentiService>();
        private IDestinatariService destinatariService => DependencyService.Get<IDestinatariService>();
        private IGiriService giriService => DependencyService.Get<IGiriService>();
        private ITrasportatoriService trasportatoriService => DependencyService.Get<ITrasportatoriService>();
        private IPrelieviService prelieviService => DependencyService.Get<IPrelieviService>();
        private ITemplateGiroService templateGiroService => DependencyService.Get<ITemplateGiroService>();
        private IStampantiService stampantiService => DependencyService.Get<IStampantiService>();

        private ObservableCollection<ItemViewModel> prelievi;


        #endregion

        #region Properties

        public bool IsReadOnly => this.giro != null && this.giro.DataConsegna.HasValue;

        public bool IsEditable => !this.IsReadOnly;

        public ObservableCollection<ItemViewModel> Prelievi 
        {
            get { return this.prelievi; }
            set { SetProperty<ObservableCollection<ItemViewModel>>(ref this.prelievi, value); }
        }

        public Command LoadItemsCommand { get; set; }

        public Command AddCommand { get; set; }

        public Command PrintCommand { get; set; }


        #endregion

        #region Constructor

        public IndexViewModel(INavigation navigation, Page page, Giro giro)
            : base(navigation, page)
        {
            this.Title = "Prelievi";
            this.Prelievi = new ObservableCollection<ItemViewModel>();
            this.giro = giro;

            this.AddCommand = new Command(async () => await ExecuteAddCommand(), canExecute: () => { return !this.IsReadOnly; });
            this.LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            this.PrintCommand = new Command(async () => await ExecutePrintCommand());

        }

        #endregion

        #region Methods

        /// <summary>
        /// Caricamento elenco prelievi presenti nel giro
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteLoadItemsCommand()
        {
            if (this.IsBusy)
                return;

            this.IsBusy = true;
            this.NoData = false;

            var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Prelievi in caricamento", lottieAnimation: "LottieLogo1.json");

            try
            {
                await Task.Run(() =>
                {
                    this.Prelievi.Clear();
                    var prelievi = this.prelieviService.GetByGiro(this.giro.Id).Result;
                    var items = Mapper.Map<List<ItemViewModel>>(prelievi.ToList());
                    this.Prelievi = new ObservableCollection<ItemViewModel>(items);

                    this.NoData = this.Prelievi.Count == 0;

                    foreach(var item in items)
                    {
                        item.OnItem_Deleting += Item_OnItem_Deleting;
                    }


                    (this.AddCommand as Command).ChangeCanExecute();
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

        private async void Item_OnItem_Deleting(object sender, EventArgs e)
        {
            var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Rimozione prelievo", lottieAnimation: "LottieLogo1.json");
            try
            {
                bool reply = await this.page.DisplayAlert("Attenzione", $"Sei sicuro di voler eliminare il prelievo selezionato?", "Si", "No");
                if (reply == false)
                    return;

                await Task.Run(() =>
                {
                    var item = sender as ItemViewModel;
                    this.prelieviService.DeleteItemAsync(item.Id).Wait();
                    this.Prelievi.Remove(item);
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
        /// Comando stampa ricevuta raccolta
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
                    var stampante = this.stampantiService.GetDefaultAsync().Result;

                    if (stampante == null)
                        throw new Exception("Nessuna stampante associata");

                    var printer = DependencyService.Get<IPrinter>();
                    printer.MacAddress = stampante.MacAddress;

                    this.giro = this.giriService.GetItemAsync(this.giro.Id).Result;
                    var templateGiro = GetTemplateGiro(this.giro.IdTemplateGiro).Result;

                    var registroRaccolta = new RegistroRaccolta();

                    registroRaccolta.Acquirente = GetAcquirente(this.giro).Result;
                    registroRaccolta.Destinatario = GetDestinatario(this.giro).Result;
                    registroRaccolta.Giro = templateGiro;
                    registroRaccolta.Trasportatore = this.trasportatoriService.GetCurrent().Result;
                    registroRaccolta.CodiceLotto = this.giro.CodiceLotto;
                    registroRaccolta.Data = DateTime.Now;

                    foreach (var prelievo in this.giro.Prelievi)
                    {
                        var allevamento = GetAllevamento(prelievo.IdAllevamento).Result;

                        registroRaccolta.Items.Add(new RegistroRaccolta.Item()
                        {
                            Scomparto = prelievo.Scomparto,
                            Allevamento = allevamento,
                            DataPrelievo = prelievo.DataPrelievo,
                            Quantita_kg = prelievo.Quantita_kg,
                            TipoLatte = allevamento != null ? allevamento.TipoLatte : null
                        });
                    }

                    printer.PrintLabel(registroRaccolta);

                });

                this.IsBusy = false;

                Analytics.TrackEvent("Stampa ricevuta consegna");
                SentrySdk.CaptureMessage("Stampa ricevuta consegna", Sentry.Protocol.SentryLevel.Info);

                await this.page.DisplayAlert("Info", "Stampa effettuata", "OK");
            }
            catch (Exception exc)
            {
                this.IsBusy = false;

                SentrySdk.CaptureException(exc);
                Crashes.TrackError(exc);

                await this.page.DisplayAlert("Errore", "Si è verificato un errore imprevisto. Contattare l'amministratore", "OK");
            }

        }    

        /// <summary>
        /// Apertura pagina nuovo prelievo
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteAddCommand()
        {
            await this.navigation.PushAsync(new EditPage(new EditViewModel(this.navigation, this.page, this.giro.Id, "")));
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
