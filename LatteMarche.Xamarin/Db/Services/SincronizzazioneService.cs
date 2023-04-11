using AutoMapper;
using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Enums;
using LatteMarche.Xamarin.Rest.Dtos;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LatteMarche.Xamarin.Db.Services
{
    public class SincronizzazioneService : ISincronizzazioneService
    {
        #region Fields

        private IAllevamentiService allevamentiService => DependencyService.Get<IAllevamentiService>();
        private IAutoCisterneService autocisterneService => DependencyService.Get<IAutoCisterneService>();
        private IAcquirentiService acquirentiService => DependencyService.Get<IAcquirentiService>();
        private ICessionariService cessionariService => DependencyService.Get<ICessionariService>();
        private IDestinatariService destinatariService => DependencyService.Get<IDestinatariService>();
        private IGiriService giriService => DependencyService.Get<IGiriService>();
        private ITipiLatteService tipiLatteService => DependencyService.Get<ITipiLatteService>();
        private IPrelieviService prelieviService => DependencyService.Get<IPrelieviService>();
        private ITrasportatoriService trasportatoriService => DependencyService.Get<ITrasportatoriService>();
        private ITemplateGiroService templateGiriService => DependencyService.Get<ITemplateGiroService>();

        #endregion

        #region Methods

        public async Task<bool> AddAsync(SynchType tipo)
        {
            using (var context = CreateContext())
            {
                var item = new Sincronizzazione()
                {
                    Timestamp = DateTime.Now,
                    Tipo = tipo.ToString()
                };

                await context.Set<Sincronizzazione>().AddAsync(item);
                await context.SaveChangesAsync();
                return await Task.FromResult(true);
            }
        }

        public async Task<Sincronizzazione> GetLastAysnc(SynchType tipo)
        {
            using (var context = CreateContext())
            {
                return await context.Set<Sincronizzazione>()
                    .Where(s => s.Tipo == tipo.ToString())
                    .OrderBy(s => s.Timestamp)
                    .LastOrDefaultAsync();
            }
        }

        public async Task<bool> ResetAsync()
        {
            this.DeleteAllItemsAsync().Wait();
            this.prelieviService.DeleteAllItemsAsync().Wait();
            this.giriService.DeleteAllItemsAsync().Wait();
            this.allevamentiService.DeleteAllItemsAsync().Wait();
            this.templateGiriService.DeleteAllItemsAsync().Wait();
            this.autocisterneService.DeleteAllItemsAsync().Wait();
            this.tipiLatteService.DeleteAllItemsAsync().Wait();
            this.acquirentiService.DeleteAllItemsAsync().Wait();
            this.destinatariService.DeleteAllItemsAsync().Wait();
            this.trasportatoriService.DeleteAllItemsAsync().Wait();

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateDatabaseSync(DownloadDto dto)
        {
            if (dto != null)
            {
                try
                {

                    LogEvent(dto, "UpdateDataSync");

                    // pulizia database
                    this.allevamentiService.DeleteAllItemsAsync().Wait();
                    this.templateGiriService.DeleteAllItemsAsync().Wait();
                    this.autocisterneService.DeleteAllItemsAsync().Wait();
                    this.tipiLatteService.DeleteAllItemsAsync().Wait();
                    this.acquirentiService.DeleteAllItemsAsync().Wait();
                    this.cessionariService.DeleteAllItemsAsync().Wait();
                    this.destinatariService.DeleteAllItemsAsync().Wait();
                    this.trasportatoriService.DeleteAllItemsAsync().Wait();

                    // trasportatore
                    LogEvent(dto, "Add Trasportatore", dto.Trasportatore);
                    var trasportatore = Mapper.Map<Trasportatore>(dto.Trasportatore);
                    this.trasportatoriService.AddItemAsync(trasportatore).Wait();

                    // autocisterna
                    LogEvent(dto, "Add Autocisterna", dto.Autocisterna);
                    var autocisterna = Mapper.Map<AutoCisterna>(dto.Autocisterna);
                    var autocisterne = Mapper.Map<List<AutoCisterna>>(dto.Autocisterne);

                    foreach(var a in autocisterne.Where(a => a.IdTrasportatore != trasportatore.Id))                        
                    {
                        a.IdTrasportatore = (int?)null;
                    }

                    if(autocisterna != null)
                    {
                        var autocisternaSelezionata = autocisterne.First(a => a.Id == autocisterna.Id);
                        autocisternaSelezionata.Selezionata = true;
                    }

                    this.autocisterneService.AddRangeItemAsync(autocisterne).Wait();

                    // tipi latte
                    LogEvent(dto, "Add TipiLatte", dto.TipiLatte);
                    var tipiLatte = Mapper.Map<List<TipoLatte>>(dto.TipiLatte);
                    this.tipiLatteService.AddRangeItemAsync(tipiLatte).Wait();

                    // acquirenti
                    LogEvent(dto, "Add Acquirenti", dto.Acquirenti);
                    var acquirenti = Mapper.Map<List<Acquirente>>(dto.Acquirenti);
                    this.acquirentiService.AddRangeItemAsync(acquirenti).Wait();

                    // cessionari
                    LogEvent(dto, "Add Cessionari", dto.Cessionari);
                    var cessionari = Mapper.Map<List<Cessionario>>(dto.Cessionari);
                    this.cessionariService.AddRangeItemAsync(cessionari).Wait();

                    // destinatari
                    LogEvent(dto, "Add Destinatari", dto.Destinatari);
                    var destinatari = Mapper.Map<List<Destinatario>>(dto.Destinatari);
                    this.destinatariService.AddRangeItemAsync(destinatari).Wait();

                    // template giro
                    LogEvent(dto, "Add Template giri", dto.Giri);
                    var giri = Mapper.Map<List<TemplateGiro>>(dto.Giri);

                    giri.ForEach(g => g.IdTrasportatore = trasportatore.Id);

                    this.templateGiriService.AddRangeItemAsync(giri).Wait();

                    LogEvent(dto, "Add Sinch Download");
                    this.AddAsync(SynchType.Download).Wait();
                }
                catch (Exception exc)
                {
                    LogEvent(dto, "Update Synch Exc", exc);

                    SentrySdk.CaptureException(exc);
                    Crashes.TrackError(exc);

                    this.allevamentiService.DeleteAllItemsAsync().Wait();
                    this.templateGiriService.DeleteAllItemsAsync().Wait();
                    this.autocisterneService.DeleteAllItemsAsync().Wait();
                    this.tipiLatteService.DeleteAllItemsAsync().Wait();
                    this.acquirentiService.DeleteAllItemsAsync().Wait();
                    this.cessionariService.DeleteAllItemsAsync().Wait();
                    this.destinatariService.DeleteAllItemsAsync().Wait();
                    this.trasportatoriService.DeleteAllItemsAsync().Wait();

                    throw exc;
                }


                return await Task.FromResult(true);

            }

            return await Task.FromResult(false);
        }

        private void LogEvent(DownloadDto dto, string message, object payload = null)
        {
            if(dto.Trasportatore.Id == 473)
            {
                Analytics.TrackEvent(message, new Dictionary<string, string>() {
                        { "payload", JsonConvert.SerializeObject(payload) },
                    });
            }
        }

        public virtual async Task<bool> DeleteAllItemsAsync()
        {
            using (var context = CreateContext())
            {
                context.Set<Sincronizzazione>().RemoveRange(context.Set<Sincronizzazione>());
                await context.SaveChangesAsync();
                return await Task.FromResult(true);
            }
        }

        protected LatteMarcheDbContext CreateContext()
        {
            LatteMarcheDbContext databaseContext = (LatteMarcheDbContext)Activator.CreateInstance(typeof(LatteMarcheDbContext));
            return databaseContext;
        }

        #endregion

    }
}
