using AutoMapper;
using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Views.Prelievi;
using LatteMarche.Xamarin.Zebra.Interfaces;
using LatteMarche.Xamarin.Zebra.Models;
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

        private Giro giro;

        private IAcquirentiService acquirentiService => DependencyService.Get<IAcquirentiService>();
        private IAllevamentiService allevamentiService => DependencyService.Get<IAllevamentiService>();
        private IDestinatariService destinatariService => DependencyService.Get<IDestinatariService>();
        private IGiriService giriService => DependencyService.Get<IGiriService>();
        private ITrasportatoriService trasportatoriService => DependencyService.Get<ITrasportatoriService>();
        private IPrelieviService prelieviService => DependencyService.Get<IPrelieviService>();
        private ITemplateGiroService templateGiroService => DependencyService.Get<ITemplateGiroService>();

        private ObservableCollection<Prelievo> prelievi;


        #endregion

        #region Properties

        public ObservableCollection<Prelievo> Prelievi 
        {
            get { return this.prelievi; }
            set { SetProperty<ObservableCollection<Prelievo>>(ref this.prelievi, value); }
        }

        public Command LoadItemsCommand { get; set; }

        public Command AddCommand { get; set; }

        public Command RemoveCommand { get; set; }

        public Command PrintCommand { get; set; }

        #endregion

        #region Constructor

        public IndexViewModel(INavigation navigation, Page page, Giro giro)
            : base(navigation, page)
        {
            this.Title = "Prelievi";
            this.Prelievi = new ObservableCollection<Prelievo>();
            this.giro = giro;

            this.AddCommand = new Command(async () => await ExecuteAddCommand());
            this.RemoveCommand = new Command(async () => await ExecuteRemoveCommand());
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
                    this.Prelievi.Clear();
                    var prelievi = this.prelieviService.GetByGiro(this.giro.Id).Result;
                    this.Prelievi = new ObservableCollection<Prelievo>(prelievi.ToList());
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

                    this.giro = this.giriService.GetItemAsync(this.giro.Id).Result;
                    var templateGiro = GetTemplateGiro(this.giro.IdTemplateGiro).Result;

                    // Salvataggio codice lotto
                    this.giro.DataConsegna = DateTime.Now;
                    this.giro.CodiceLotto = $"{templateGiro?.Codice}{this.giro.DataConsegna:ddMMyyHHmm}";
                    this.giriService.UpdateItemAsync(this.giro).Wait();

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

                    //printer.PrintLabel(registroRaccolta);

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
            await this.navigation.PushAsync(new EditPage(new EditViewModel(this.navigation, this.page, this.giro.Id, "")));
        }

        /// <summary>
        /// Rimozione giro
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteRemoveCommand()
        {
            try
            {
                this.IsBusy = true;

                await Task.Run(() =>
                {
                    this.giriService.DeleteItemAsync(this.giro.Id).Wait();
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
