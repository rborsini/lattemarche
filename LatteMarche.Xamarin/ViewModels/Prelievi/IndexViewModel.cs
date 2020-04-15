using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Models;
using LatteMarche.Xamarin.Services;
using LatteMarche.Xamarin.Views.Prelievi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LatteMarche.Xamarin.ViewModels.Prelievi
{
    public class IndexViewModel : BaseViewModel
    {

        #region Fields

        private Lotto lotto;

        private IAcquirentiService acquirentiService => DependencyService.Get<IAcquirentiService>();
        private IAllevamentiService allevamentiService => DependencyService.Get<IAllevamentiService>();
        private IDestinatariService destinatariService => DependencyService.Get<IDestinatariService>();
        private IGiriService giriService => DependencyService.Get<IGiriService>();
        private ILottiService lottiService => DependencyService.Get<ILottiService>();
        private ITrasportatoriService trasportatoriService => DependencyService.Get<ITrasportatoriService>();

        #endregion

        #region Properties

        public ObservableCollection<ItemViewModel> Items { get; set; }

        public Command LoadItemsCommand { get; set; }

        public Command AddCommand { get; set; }

        public Command PrintCommand { get; set; }

        #endregion

        #region Constructor

        public IndexViewModel(INavigation navigation, Page page, Lotto lotto)
            : base(navigation, page)
        {
            this.Title = "Prelievi";
            this.Items = new ObservableCollection<ItemViewModel>();
            this.lotto = lotto;

            this.AddCommand = new Command(async () => await ExecuteAddCommand());
            this.LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            this.PrintCommand = new Command(async () => await ExecutePrintCommand());

        }

        #endregion

        #region Methods

        /// <summary>
        /// Caricamento elenco prelievi presenti nel lotto
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteLoadItemsCommand()
        {
            if (this.IsBusy)
                return;

            this.IsBusy = true;

            try
            {
                await Task.Run(() =>
                {
                    this.Items.Clear();

                    var allevamenti = this.allevamentiService.GetItemsAsync().Result;

                    var lotto = this.lottiService.GetItemAsync(this.lotto.Id).Result;
                    foreach (var prelievo in lotto.Prelievi)
                    {
                        var allevamento = prelievo.IdAllevamento.HasValue ? allevamenti.FirstOrDefault(a => a.Id == prelievo.IdAllevamento.Value) : null;

                        this.Items.Add(new ItemViewModel() 
                        { 
                            Id = prelievo.Id,
                            IdLotto = prelievo.IdLotto,
                            Title = $"{allevamento?.RagioneSociale}"
                        });
                    }
                });
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
                    var printer = DependencyService.Get<IPrinter>();
                    printer.MacAddress = "00:03:7A:30:B0:4D";

                    this.lotto = this.lottiService.GetItemAsync(this.lotto.Id).Result;

                    var registroRaccolta = new RegistroRaccolta();

                    registroRaccolta.Acquirente = GetAcquirente(this.lotto).Result;
                    registroRaccolta.Destinatario = GetDestinatario(this.lotto).Result;
                    registroRaccolta.Giro = GetGiro(this.lotto.IdGiro).Result;
                    registroRaccolta.Trasportatore = GetTrasportatore(registroRaccolta.Giro?.IdTrasportatore).Result;

                    registroRaccolta.Data = DateTime.Now;
                    registroRaccolta.CodiceLotto = this.lotto.Codice;

                    foreach(var prelievo in this.lotto.Prelievi)
                    {
                        registroRaccolta.Items.Add(new RegistroRaccolta.Item()
                        {
                            Scomparto = prelievo.Scomparto,
                            Allevamento = GetAllevamento(prelievo.IdAllevamento).Result,
                            DataPrelievo = prelievo.DataPrelievo,
                            Quantita_kg = prelievo.Quantita_kg,
                            TipoLatte = prelievo.TipoLatte
                        });
                    }

                    printer.PrintLabel(registroRaccolta);

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
        /// Apertura pagina nuovo prelievo
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteAddCommand()
        {
            await this.navigation.PushAsync(new EditPage(new EditViewModel(this.navigation, this.page, this.lotto.Id, "")));
        }

        /// <summary>
        /// Lookup acquirente
        /// </summary>
        /// <param name="lotto"></param>
        /// <returns></returns>
        private async Task<Acquirente> GetAcquirente(Lotto lotto)
        {
            if (lotto.Prelievi.Count(p => p.IdAcquirente.HasValue) > 0)
            {
                var idAcquirente = lotto.Prelievi.First(p => p.IdAcquirente.HasValue).IdAcquirente.Value;
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
        private async Task<Destinatario> GetDestinatario(Lotto lotto)
        {
            if (lotto.Prelievi.Count(p => p.IdDestinatario.HasValue) > 0)
            {
                var idDestinatario = lotto.Prelievi.First(p => p.IdDestinatario.HasValue).IdDestinatario.Value;
                return await this.destinatariService.GetItemAsync(idDestinatario);
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
