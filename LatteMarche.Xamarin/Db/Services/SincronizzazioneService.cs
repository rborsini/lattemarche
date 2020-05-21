using AutoMapper;
using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Enums;
using LatteMarche.Xamarin.Rest.Dtos;
using Microsoft.EntityFrameworkCore;
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
        private IDestinatariService destinatariService => DependencyService.Get<IDestinatariService>();
        private ITipiLatteService tipiLatteService => DependencyService.Get<ITipiLatteService>();
        private ITrasportatoriService trasportatoriService => DependencyService.Get<ITrasportatoriService>();
        private ITemplateGiroService templateGiriService => DependencyService.Get<ITemplateGiroService>();

        #endregion

        #region Methods

        public async Task<bool> AddAsync(SynchType tipo)
        {
            using (var context = CrateContext())
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
            using (var context = CrateContext())
            {
                return await context.Set<Sincronizzazione>()
                    .Where(s => s.Tipo == tipo.ToString())
                    .OrderBy(s => s.Timestamp)
                    .LastOrDefaultAsync();
            }
        }

        public async Task<bool> ResetAsync()
        {
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
                    // pulizia database
                    this.allevamentiService.DeleteAllItemsAsync().Wait();
                    this.templateGiriService.DeleteAllItemsAsync().Wait();
                    this.autocisterneService.DeleteAllItemsAsync().Wait();
                    this.tipiLatteService.DeleteAllItemsAsync().Wait();
                    this.acquirentiService.DeleteAllItemsAsync().Wait();
                    this.destinatariService.DeleteAllItemsAsync().Wait();
                    this.trasportatoriService.DeleteAllItemsAsync().Wait();

                    // trasportatore
                    var trasportatore = Mapper.Map<Trasportatore>(dto.Trasportatore);
                    this.trasportatoriService.AddItemAsync(trasportatore).Wait();

                    // autocisterna
                    var autocisterna = Mapper.Map<AutoCisterna>(dto.Autocisterna);
                    this.autocisterneService.AddItemAsync(autocisterna).Wait();

                    // tipi latte
                    var tipiLatte = Mapper.Map<List<TipoLatte>>(dto.TipiLatte);
                    this.tipiLatteService.AddRangeItemAsync(tipiLatte).Wait();

                    // acquirenti
                    var acquirenti = Mapper.Map<List<Acquirente>>(dto.Acquirenti);
                    this.acquirentiService.AddRangeItemAsync(acquirenti).Wait();

                    // destinatari
                    var destinatari = Mapper.Map<List<Destinatario>>(dto.Destinatari);
                    this.destinatariService.AddRangeItemAsync(destinatari).Wait();

                    // template giro
                    var giri = Mapper.Map<List<TemplateGiro>>(dto.Giri);

                    giri.ForEach(g => g.IdTrasportatore = trasportatore.Id);

                    this.templateGiriService.AddRangeItemAsync(giri).Wait();

                    this.AddAsync(SynchType.Download).Wait();
                }
                catch(Exception exc)
                {
                    this.allevamentiService.DeleteAllItemsAsync().Wait();
                    this.templateGiriService.DeleteAllItemsAsync().Wait();
                    this.autocisterneService.DeleteAllItemsAsync().Wait();
                    this.tipiLatteService.DeleteAllItemsAsync().Wait();
                    this.acquirentiService.DeleteAllItemsAsync().Wait();
                    this.destinatariService.DeleteAllItemsAsync().Wait();
                    this.trasportatoriService.DeleteAllItemsAsync().Wait();

                    throw exc;
                }


                return await Task.FromResult(true);

            }

            return await Task.FromResult(false);
        }

        protected LatteMarcheDbContext CrateContext()
        {
            LatteMarcheDbContext databaseContext = (LatteMarcheDbContext)Activator.CreateInstance(typeof(LatteMarcheDbContext));
            //databaseContext.Database.EnsureCreated();
            //databaseContext.Database.Migrate();
            return databaseContext;
        }

        #endregion

    }
}
